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
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;

namespace Nmap_MT
{
    public partial class NmapMT : Form
    {
        private ScanList g_ScanList = null;
        private bool g_stopped = false;
        private List<Task> g_scannners = null;
        private int g_scanlist_count;

        public NmapMT()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            //groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text != "Start";
            if (btnStartStop.Text == "Start")
            {
                g_stopped = false;
                btnStartStop.Text = "Stop";
                btnStartStop.ForeColor = Color.Red;
                if (g_ScanList != null && MessageBox.Show(this,"Do you want to resume last scan?", "Restore Last Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    lvScans.Items.Clear();
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
                        //groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = true;
                        return;
                    }
                    tstStatus.Text = "Generating ScanList.xml";
                    
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

                    XmlDocument doc = new XmlDocument();                    
                    doc.LoadXml("<ScanList></ScanList>");
                    XmlNode scanListNode = doc.SelectSingleNode("/ScanList");

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

                                    XmlElement Host = doc.CreateElement("Host");
                                    XmlElement IP = doc.CreateElement("IP");
                                    IP.InnerText = $"{a}.{b}.{c}.{d}";
                                    Host.AppendChild(IP);
                                    XmlElement ScanResult = doc.CreateElement("ScanResult");
                                    Host.AppendChild(ScanResult);
                                    scanListNode.AppendChild(Host);

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
                    
                    tstStatus.Text = "Saving ScanList.xml..";
                    doc.AppendChild(scanListNode);
                    doc.Save("ScanList.xml");
                    tstStatus.Text = "Saving ScanList.xml..done..";

                    tstStatus.Text = "Loading ScanList.xml..";
                    LoadScanlist();
                    tstStatus.Text = "ScanList.xml Loaded..";
                }

                g_scanlist_count = g_ScanList.Host.Count();
                List<ScanListHost> unscanned = g_ScanList.Host.Where(h => h.ScanResult == string.Empty).ToList();
                if (g_scanlist_count > 0)
                {                   
                    tstStatus.Text = $"{g_scanlist_count - unscanned.Count()} of {g_scanlist_count} Hosts Scanned!";
                }
                tstProgress.Value = 0;

                g_scannners = new List<Task>();
                int num_scans_per_thread = (int)(g_scanlist_count / numThreads.Value);
                if(num_scans_per_thread < 1)
                {
                    num_scans_per_thread = 1;
                }

                while(unscanned.Count > 0)
                {
                    var tmp = DeepCopy(unscanned.Take(num_scans_per_thread)).ToList();
                    Task t = Task.Run(() => ScanRange(tmp));
                    g_scannners.Add(t);
                    unscanned = unscanned.Skip(num_scans_per_thread).ToList();
                }

                int total = g_ScanList.Host.Count();
                while (g_scannners.Count > 0)
                {
                    if (g_stopped)
                        break;
                    g_scannners.RemoveAll(t => t.Status != TaskStatus.Running);
                    Application.DoEvents();
                    Thread.Sleep(50);
                    tstStatus.Text = $"{total - g_scanlist_count} of {total} Hosts Scanned!";
                    tstProgress.Value = (((total - g_scanlist_count) * 100) / total) ;
                }

                tstStatus.Text = "Saving ScanList.xml";
                SaveScanlist();
                tstStatus.Text = "ScanList.xml saved!";

                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.Green;
                //groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text != "Start";
            }
            else
            {
                g_stopped = true;
                btnStartStop.Enabled = false;

                while(g_scannners.Count > 0)
                {
                    g_scannners.RemoveAll(t => t.Status != TaskStatus.Running);
                    Application.DoEvents();
                    Thread.Sleep(500);
                    tstStatus.Text = $"{g_scannners.Count} Stopping..";
                }

                tstStatus.Text = "Saving ScanList.xml";
                SaveScanlist();
                tstStatus.Text = "ScanList.xml saved!";

                tstStatus.Text = $"Scan Stopped..";
                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.Green;
                btnStartStop.Enabled = true;
                //groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text != "Start";
            }
        }

        private void ScanRange(List<ScanListHost> unscanned)
        {
            string nmapArgs = txtNmapArgs.Text.Trim().Length > 0 ? txtNmapArgs.Text.Trim() : "-sn";

            List<ScanListHost> tmpScan = new List<ScanListHost>();

            while ((tmpScan = unscanned.Take((int)hostsPerThread.Value).ToList()).Count > 0)
            {
                unscanned = unscanned.Skip((int)hostsPerThread.Value).ToList();
                string ipRange = get_nmap_ip_range(tmpScan.First().IP, tmpScan.Last().IP);

                if (g_stopped)
                    break;

                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "nmap.exe",
                        Arguments = $"{nmapArgs} {ipRange}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();

                string _output = string.Empty;
                while (!proc.StandardOutput.EndOfStream)
                {
                    _output += proc.StandardOutput.ReadLine() + Environment.NewLine;
                }
                g_scanlist_count-=tmpScan.Count;

                if (g_ScanList == null)
                    break;

                bool showOffline = cbShowOffline.Checked;
                string[] tokenizedScanResult = _output.Split(new[] { '\n'}, StringSplitOptions.RemoveEmptyEntries);
                foreach(var host in tmpScan)
                {
                    string scanResult = extractResult(tokenizedScanResult, host.IP);
                    g_ScanList.Host.FirstOrDefault(h => h.IP == host.IP).ScanResult = scanResult;

                    if (scanResult == "Offline" && !showOffline)
                        continue;

                    string[] row = { host.IP, scanResult };
                    var listViewItem = new ListViewItem(row);
                    AddLvItem(listViewItem);
                }
            }
        }

        private static string extractResult(string [] scanResult, string hostIP)
        {
            if (!scanResult.Any(l => l.Contains(hostIP)))
                return "Offline";

            var _output = string.Empty;
            bool startAppendingResult = false;
            foreach(var line in scanResult)
            {
                if (startAppendingResult)
                {
                    if (line.Contains("Nmap scan report for"))
                        break;

                    _output += line;
                }
                if (line.Contains(hostIP))
                {
                    _output += line;
                    startAppendingResult = true;
                }               
            }
            
            return _output;
        }

        delegate void SetLviCallback(ListViewItem lvi);

        private void AddLvItem(ListViewItem lvi)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lvScans.InvokeRequired)
            {
                SetLviCallback d = new SetLviCallback(AddLvItem);
                this.Invoke(d, new object[] { lvi });
            }
            else
            {
                this.lvScans.Items.Add(lvi);
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
                if (t1_val < 0 || t2_val < 0 || t1_val > 255 || t2_val > 255)
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
                    LoadScanlist();

                    ScanListHost start = g_ScanList.Host.First();
                    ScanListHost end = g_ScanList.Host.Last();

                    get_octets(start.IP, ref txtFrom1, ref txtFrom2, ref txtFrom3, ref txtFrom4);
                    get_octets(end.IP, ref txtTo1, ref txtTo2, ref txtTo3, ref txtTo4);
                }
            }
        }

        private void LoadScanlist()
        {
            XmlSerializer inSrlz = new XmlSerializer(typeof(ScanList));
            StreamReader inRead = new StreamReader("ScanList.xml");
            g_ScanList = ((ScanList)inSrlz.Deserialize(inRead));
            inRead.Close();
        }

        private void SaveScanlist()
        {
            XmlSerializer outSrlz = new XmlSerializer(typeof(ScanList));
            StreamWriter outWriter = new StreamWriter("ScanList.xml");
            if(!cbShowOffline.Checked)
            {
                g_ScanList.Host = g_ScanList.Host.Where(h => h.ScanResult != "Offline").ToArray();
            }            
            outSrlz.Serialize(outWriter, g_ScanList);
            outWriter.Close();
        }

        private static string get_nmap_ip_range(string startIP, string endIP)
        {
            ScanFormatted start = new ScanFormatted();
            ScanFormatted end = new ScanFormatted();

            start.Parse(startIP, "%d.%d.%d.%d");
            end.Parse(endIP, "%d.%d.%d.%d");

            if(start.Results.Count == 4 && end.Results.Count == 4)
            {
                string range = string.Empty;
                   
                for(int k=0;k<4;k++)
                    range += (start.Results[k] == end.Results[k] ? start.Results[k].ToString() : $"{start.Results[k].ToString()}-{end.Results[k].ToString()}") + (k<3 ? "." : string.Empty);

                return range;
            }

            return string.Empty;
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

        private static T DeepCopy<T>(T lst)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(lst));
        }
    }
}
