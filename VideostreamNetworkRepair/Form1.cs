using NETWORKLIST;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
