using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Nmap_MT
{
    public partial class NmapMT : Form
    {
        private ScanList g_ScanList = null;

        public NmapMT()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = btnStartStop.Text != "Start";
            if (btnStartStop.Text == "Start")
            {
                btnStartStop.Text = "Stop";
                btnStartStop.ForeColor = Color.Red;
                if (g_ScanList != null && MessageBox.Show(this,"Do you want to resume last scan?", "Restore Last Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    g_ScanList = null;
                }

                if (g_ScanList == null)
                {
                    int from1 = 0, from2 = 0, from3 = 0, from4 = 0, to1 = 0, to2 = 0, to3 = 0, to4 = 0;
                    if (!ValidateIPRange(ref txtFrom1, ref txtTo1, ref from1, ref to1) || !ValidateIPRange(ref txtFrom2, ref txtTo2, ref from2, ref to2) || !ValidateIPRange(ref txtFrom3, ref txtTo3, ref from3, ref to3) || !ValidateIPRange(ref txtFrom4, ref txtTo4, ref from4, ref to4))
                    {
                        MessageBox.Show("Please check TO & FROM Addresses!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnStartStop.Text = "Start";
                        btnStartStop.ForeColor = Color.Green;
                        groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = true;
                        return;
                    }
                    tstStatus.Text = "Generating ScanList.xml";

                    List<ScanListHost> generatedHosts = new List<ScanListHost>();

                    int hostCount = 0;
                    for (int a = from1; a <= to1; a++)
                    {
                        for (int b = from2; b <= to2; b++)
                        {
                            for (int c = from3; c <= to3; c++)
                            {
                                for (int d = from4; d <= to4; d++)
                                {
                                    hostCount++;
                                }
                            }
                        }
                    }

                    int currCount = 0;
                    for (int a = from1; a <= to1; a++)
                    {
                        for (int b = from2; b <= to2; b++)
                        {
                            for (int c = from3; c <= to3; c++)
                            {
                                for (int d = from4; d <= to4; d++)
                                {
                                    currCount++;
                                    tstProgress.Value = ((currCount * 100) / hostCount);
                                    generatedHosts.Add(new ScanListHost
                                    {
                                        IP = $"{a}.{b}.{c}.{d}",
                                        ScanResult = string.Empty
                                    });
                                    if (currCount % 255 == 0)
                                        Application.DoEvents();
                                    if (btnStartStop.Text == "Start")
                                    {
                                        tstStatus.Text = "Idle..";
                                        tstProgress.Value = 0;
                                        return;
                                    }                                        
                                }
                            }
                        }
                    }

                    g_ScanList = new ScanList();
                    g_ScanList.Host = generatedHosts.ToArray();

                    tstStatus.Text = "Saving ScanList.xml..";
                    SerializeObject(g_ScanList, "ScanList.xml");
                    tstStatus.Text = "Saving ScanList.xml..done..";
                }

                if (g_ScanList.Host.Count() > 0)
                {
                    int scanned = g_ScanList.Host.Count(h => h.ScanResult != string.Empty);
                    tstStatus.Text = $"{scanned} of {g_ScanList.Host.Count()} Hosts Scanned!";
                }

                tstProgress.Value = 0;
            }
            else
            {
                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.Green;
            }
        }

        private bool ValidateIPRange(ref TextBox t1,ref TextBox t2, ref int from, ref int to)
        {
            if(string.IsNullOrEmpty(t1.Text) || string.IsNullOrEmpty(t2.Text))
            {
                return false;
            }

            int t1_val, t2_val;
            if(int.TryParse(t1.Text, out t1_val) && int.TryParse(t2.Text, out t2_val))
            {
                if (t1_val < 0 || t2_val <0 || t1_val > 255 || t2_val > 255)
                    return false;

                if (t2_val < t1_val)
                    return false;

                from = t1_val;
                to = t2_val;
            }
            else
            {
                return false;
            }

            return true;
        }

        private void NmapMT_Load(object sender, EventArgs e)
        {
            if (File.Exists("ScanList.xml"))
            {
                if (MessageBox.Show("Do you want to restore last scan parameters?", "Restore Last Scan?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    XmlSerializer inSrlz = new XmlSerializer(typeof(ScanList));
                    StreamReader inRead = new StreamReader("ScanList.xml");
                    g_ScanList = ((ScanList)inSrlz.Deserialize(inRead));
                    inRead.Close();

                    ScanListHost start = g_ScanList.Host.First();
                    ScanListHost end = g_ScanList.Host.Last();

                    get_octets(start.IP, ref txtFrom1, ref txtFrom2, ref txtFrom3, ref txtFrom4);
                    get_octets(end.IP, ref txtTo1, ref txtTo2, ref txtTo3, ref txtTo4);
                }
            }
        }

        private bool get_octets(string IP, ref TextBox oct1, ref TextBox oct2, ref TextBox oct3, ref TextBox oct4)
        {
            ScanFormatted sf = new ScanFormatted();
            sf.Parse(IP, "%d.%d.%d.%d");
            if(sf.Results.Count == 4)
            {
                oct1.Text = sf.Results[0].ToString();
                oct2.Text = sf.Results[1].ToString();
                oct3.Text = sf.Results[2].ToString();
                oct4.Text = sf.Results[3].ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Serializes class to XML file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject">Class to serialize</param>
        /// <param name="fileName">XML file path</param>
        private static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }
        
        private void ip_change(object sender, KeyEventArgs e)
        {
            g_ScanList = null;
        }
    }
}
