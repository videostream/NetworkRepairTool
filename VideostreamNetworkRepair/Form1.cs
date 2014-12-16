using NETWORKLIST;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Collections;
using System.Net;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Security.Principal;
using Securite.Win32;
using System.Security.AccessControl;
using System.Configuration;
using System.Reflection;
using System.Net.Configuration;
namespace VideostreamNetworkRepair
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
            {
                // windows XP. Disable profile button.
            }
            ToggleAllowUnsafeHeaderParsing(true);
            repairFirewall();
            tmrProgress.Start();
            //getInstalledApplications();
        }

        // Enable/disable useUnsafeHeaderParsing.
        // See http://o2platform.wordpress.com/2010/10/20/dealing-with-the-server-committed-a-protocol-violation-sectionresponsestatusline/
        public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }

        private void openPort(int port, string name)
        {
            INetFwOpenPort portClass;
            Type TportClass = Type.GetTypeFromProgID("HNetCfg.FWOpenPort");
            portClass = (INetFwOpenPort)Activator.CreateInstance(TportClass);
            // Set the port properties
            portClass.Scope = NetFwTypeLib.NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            portClass.Enabled = true;
            portClass.Protocol = NetFwTypeLib.NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            portClass.Name = name;
            portClass.Port = port;

            INetFwMgr icfMgr = null;
            try
            {
                Type TicfMgr = Type.GetTypeFromProgID("HNetCfg.FwMgr");
                icfMgr = (INetFwMgr)Activator.CreateInstance(TicfMgr);
            }
            catch (Exception ex)
            {
                return;
            }
            var profile = icfMgr.LocalPolicy.CurrentProfile;
            // Add the port to the ICF Permissions List
            profile.GloballyOpenPorts.Add(portClass);
        }

        private void repairFirewall()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor == 1)
            {
                // windows XP. Can't do this, yo.
            }
            else
            {
                var manager = new NetworkListManager();
                var connectedNetworks = manager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_CONNECTED).Cast<INetwork>();
                foreach (var network in connectedNetworks)
                {
                    Console.Write(network.GetName() + " ");
                    var cat = network.GetCategory();
                    if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PRIVATE)
                    {
                        Console.WriteLine("[PRIVATE]");
                    }
                    else if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PUBLIC)
                    {
                        Console.WriteLine("[PUBLIC]");
                        network.SetCategory(NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PRIVATE);
                    }
                    else if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_DOMAIN_AUTHENTICATED)
                    {
                        Console.WriteLine("[DOMAIN]");
                    }
                }
            }
           this.openPort(5556, "Videostream Desktop Application");
           this.openPort(5558, "Videostream Mobile Application");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        HttpWebRequest webRequest;
        int count = 0;

        void StartWebRequest()
        {
            webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://127.0.0.1:5556/portfix-complete"));
            //webRequest.Timeout = 20000;
            this.DoWithResponse(webRequest, (response) =>
            {
                try
                {
                    if (response != null)
                    {
                        using (var reader = new StreamReader(response.GetResponseStream()))
                        {
                            string json = reader.ReadToEnd();
                            VideostreamResponse vsReponse = jsonHelper.From<VideostreamResponse>(json);
                            Console.WriteLine(vsReponse.result);

                            prgRepair.PerformStep();

                            if (vsReponse.TryAgain && count++ < 3)
                            {
                                StartWebRequest();
                            }
                            else if (vsReponse.Success)
                            {
                                resultSuccess();
                            }
                            else if (vsReponse.NoMediaLoaded || vsReponse.ChromecastSession)
                            {
                                resultGoBackToVideostream();
                            }
                            else if (vsReponse.FirewallBlocked)
                            {
                                resultReboot();
                            }
                        }
                    }
                    else
                    {
                        resultGoBackToVideostream();
                    }
                }
                catch (Exception ex)
                {
                    resultGoBackToVideostream();
                }
            });
        }

        private void DoWithResponse(WebRequest request, Action<HttpWebResponse> responseAction)
        {
            webRequest.Proxy = null;
            webRequest.KeepAlive = false;
            webRequest.ContentType = "application/json";
            webRequest.Method = "GET";
            
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    try
                    {
                        try
                        {
                            var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);

                            this.Invoke((MethodInvoker)delegate
                            {
                                responseAction(response);
                            });
                        }
                        catch (Exception ex)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                responseAction(null);
                            });
                        }
                        
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine(ex.Message);
                        this.Invoke((MethodInvoker)delegate
                        {
                            responseAction(null);
                        });
                    }

                }), request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        Boolean hasAntivirus = false;
        ArrayList installedList = new ArrayList();
        private void getInstalledApplications()
        {
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        String displayName = (String)subkey.GetValue("DisplayName");
                        String installLocation = (String)subkey.GetValue("InstallLocation");
                        if (displayName != null && !displayName.Trim().Equals(""))
                        {
                            if (isAntivirus(displayName))
                            {
                                hasAntivirus = true;
                                return;
                            }
                        }
                    }
                }
            }

            using (Microsoft.Win32.RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        String displayName = (String)subkey.GetValue("DisplayName");
                        String installLocation = (String)subkey.GetValue("InstallLocation");
                        if (displayName != null && !displayName.Trim().Equals(""))
                        {
                            if (isAntivirus(displayName))
                            {
                                hasAntivirus = true;
                                return;
                            }
                        }
                    }
                }
            }
        }

        private bool isAntivirus(string displayName)
        {
            //Console.WriteLine("TODO: Verify if " + displayName + " is name of Antivirus");
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void tmrProgress_Tick(object sender, EventArgs e)
        {
            prgRepair.PerformStep();
            if (prgRepair.Value >= prgRepair.Maximum/2)
            {
                tmrProgress.Stop();
                prgRepair.Step = prgRepair.Maximum / 6;
                StartWebRequest();
            }
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
           // System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
        }

        private void resultSuccess()
        {
            prgRepair.Value = prgRepair.Maximum;
            prgRepair.Hide();
            lblStatus.Hide();
            btnClose.Show();

            imgStatus.Image = VideostreamNetworkRepair.Properties.Resources.Enjoy;
            imgStatus.Show();
        }

        private void resultReboot()
        {
            prgRepair.Hide();
            prgRepair.Value = prgRepair.Maximum;
            btnReboot.Show();
            btnClose.Show();
            lblStatus.Hide();

            imgStatus.Image = VideostreamNetworkRepair.Properties.Resources.Reboot;
            imgStatus.Show();
        }

        private void resultGoBackToVideostream()
        {
            prgRepair.Hide();
            prgRepair.Value = prgRepair.Maximum;
            lblStatus.Hide();
            btnClose.Show();
            imgStatus.Image = VideostreamNetworkRepair.Properties.Resources.BackToVS;
            imgStatus.Show();
        }

        private void imgStatus_Click(object sender, EventArgs e)
        {

        }
    }

    public class VideostreamResponse
    {
        private String mResult;
        public String result {
            set {
                mResult = value;

                if (result.Equals("NoMediaLoaded"))
                {
                    NoMediaLoaded = true;
                }
                else if (result.Equals("CommunicationBlocked"))
                {
                    FirewallBlocked = true;
                    TryAgain = true;
                }
                else if (result.Equals("NoSession"))
                {
                    ChromecastSession = true;
                }
                else if (result.Equals("Success"))
                {
                    Success = true;
                }
            }
            get
            {
                return mResult;
            }
        }

        public Boolean NoMediaLoaded = false;
        public Boolean FirewallBlocked = false;
        public Boolean ChromecastSession = false;
        public Boolean Success = false;
        public Boolean TryAgain = false;
    }

    public class jsonHelper
    {
        public static string To<T>(T obj)
        {
            string retVal = null;
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                retVal = Encoding.Default.GetString(ms.ToArray());
            }

            return retVal;
        }

        public static T From<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
            }

            return obj;
        }
    }
}

