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
                button2.Hide();
                label2.Hide();
            }

            getInstalledApplications();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.openPort(5556, "Videostream Desktop Application");
            this.openPort(5558, "Videostream Mobile Application");
            label1.ForeColor = Color.Green;
            label1.Text = "Ready";
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

        private void button2_Click(object sender, EventArgs e)
        {
            var manager = new NetworkListManager();
            var connectedNetworks = manager.GetNetworks(NLM_ENUM_NETWORK.NLM_ENUM_NETWORK_CONNECTED).Cast<INetwork>();
            foreach (var network in connectedNetworks)
            {
                Console.Write(network.GetName() + " ");
                var cat = network.GetCategory();
                if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PRIVATE) {
                    Console.WriteLine("[PRIVATE]");
                }
                else if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PUBLIC)
                {
                    Console.WriteLine("[PUBLIC]");
                    network.SetCategory(NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_PRIVATE);
                }
                else if (cat == NLM_NETWORK_CATEGORY.NLM_NETWORK_CATEGORY_DOMAIN_AUTHENTICATED)
                    Console.WriteLine("[DOMAIN]");
            }
            this.openPort(5556, "Videostream Desktop Application");
            this.openPort(5558, "Videostream Mobile Application");
            label2.ForeColor = Color.Green;
            label2.Text = "Ready";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }
        WebRequest webRequest = HttpWebRequest.Create(new Uri("http://127.0.0.1:5556/status"));

        void StartWebRequest()
        {
            this.DoWithResponse(webRequest, (response) =>
            {
                try
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        string json = reader.ReadToEnd();
                        VideostreamResponse f = jsonHelper.From<VideostreamResponse>(json);
                    }
                }
                catch (Exception ex)
                {
                    
                }
            });
        }

        private void DoWithResponse(WebRequest request, Action<HttpWebResponse> responseAction)
        {
            webRequest.Proxy = null;
            Action wrapperAction = () =>
            {
                request.BeginGetResponse(new AsyncCallback((iar) =>
                {
                    var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
                    responseAction(response);
                }), request);
            };
            wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
            {
                var action = (Action)iar.AsyncState;
                action.EndInvoke(iar);
            }), wrapperAction);
        }

        void FinishWebRequest(IAsyncResult result)
        {
            webRequest.EndGetResponse(result);
        }

        private void getVideostreamStatus()
        {
            WebRequest request = WebRequest.Create("http://localhost:5556/status");
            request.GetResponse();
        }

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
                        String f = (String)subkey.GetValue("DisplayName");
                        if (f != null && !f.Trim().Equals("") && !installedList.Contains(f))
                        {
                            Console.WriteLine(f);
                            installedList.Add(f);
                        }
                    }
                }
            }
        }
    }

    public class VideostreamResponse
    {
        private String result;
        public String Result {
            set {
                result = value;
                if (result.Equals("NoMediaLoaded"))
                {
                    NoMediaLoaded = true;
                }
                else if (result.Equals("CommunicationBlocked"))
                {
                    FirewallBlocked = true;
                }
                else if (result.Equals("NoSession"))
                {
                    ChromecastSession = false;
                }
                else if (result.Equals("Success"))
                {
                    Success = true;
                }
            }
            get
            {
                return result;
            }
        }
        public Boolean NoMediaLoaded;
        public Boolean FirewallBlocked;
        public Boolean ChromecastSession;
        public Boolean Success;
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

