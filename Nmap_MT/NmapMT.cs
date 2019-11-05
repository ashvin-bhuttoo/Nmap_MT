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
using IPAddressTools;
using System.Net;

namespace Nmap_MT
{
    public partial class NmapMT : Form
    {
        private ScanList g_ScanList = null;
        private bool g_stopped = false;
        private List<Task> g_scannner_tasks = null;
        private int g_remaining_hosts_count, g_total;
        private Stopwatch g_sw = new Stopwatch();

        public NmapMT()
        {
            InitializeComponent();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {            
            if (btnStartStop.Text == "Start")
            {
                g_sw.Restart();
                g_stopped = false;
                btnStartStop.Text = "Stop";
                btnStartStop.ForeColor = Color.Red;
                cbShowOffline.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text == "Start";

                lvScans.Items.Clear();
                g_ScanList = null;

                int from1 = 0, from2 = 0, from3 = 0, from4 = 0, to1 = 0, to2 = 0, to3 = 0, to4 = 0;
                if (!ValidateIPRange(ref txtFrom1, ref txtTo1, ref from1, ref to1) || !ValidateIPRange(ref txtFrom2, ref txtTo2, ref from2, ref to2) || !ValidateIPRange(ref txtFrom3, ref txtTo3, ref from3, ref to3) || !ValidateIPRange(ref txtFrom4, ref txtTo4, ref from4, ref to4))
                {
                    MessageBox.Show("Please check TO & FROM Addresses!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnStartStop.Text = "Start";
                    btnStartStop.ForeColor = Color.Green;
                    cbShowOffline.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text == "Start"; 
                    return;
                }
                tstStatus.Text = "Generating memory ScanList";

                g_ScanList = new ScanList();
                List<ScanListHost> scanlistHosts = new List<ScanListHost>();

                IEnumerable<string> ipRanges = new RangeFinder().GetIPRange(IPAddress.Parse($"{from1}.{from2}.{from3}.{from4}"), IPAddress.Parse($"{to1}.{to2}.{to3}.{to4}"));
                foreach (var ip in ipRanges)
                    scanlistHosts.Add(new ScanListHost { IP = ip, ScanResult = string.Empty });

                g_ScanList.Host = scanlistHosts.ToArray();

                List<ScanListHost> unscanned = g_ScanList.Host.Where(h => h.ScanResult == string.Empty).ToList();
                if (g_remaining_hosts_count > 0)
                {                   
                    tstStatus.Text = $"{g_remaining_hosts_count - unscanned.Count()} of {g_remaining_hosts_count} Hosts Scanned!";
                }
                tstProgress.Value = 0;
                g_scannner_tasks = new List<Task>();

                g_total = g_ScanList.Host.Count();
                g_remaining_hosts_count = g_total;
                do
                {
                    int removed = g_scannner_tasks.RemoveAll(t => t.Status == TaskStatus.RanToCompletion);
                    Application.DoEvents();
                    Thread.Sleep(50);
                    tstStatus.Text = $"{g_total - g_remaining_hosts_count} of {g_total} Hosts Scanned! Time: {g_sw.Elapsed.Minutes}m{g_sw.Elapsed.Seconds}s ScanRate: {(int)((g_total - g_remaining_hosts_count) / g_sw.Elapsed.TotalSeconds)} hosts/s";
                    tstProgress.Value = (((g_total - g_remaining_hosts_count) * 100) / g_total);

                    if (g_stopped)
                        return;

                    while (unscanned.Count > 0 && g_scannner_tasks.Count < numThreads.Value)
                    {
                        if (g_stopped)
                            return;

                        var tmp = DeepCopy(unscanned.Take((int)hostsPerThread.Value)).ToList();
                        Task t = Task.Run(() => ScanRange(tmp));
                        g_scannner_tasks.Add(t);
                        unscanned = unscanned.Skip((int)hostsPerThread.Value).ToList();
                    }
                } while (g_scannner_tasks.Count > 0 || g_remaining_hosts_count > 0 || (unscanned.Count > 0 && !g_stopped));

                if (g_stopped)
                    return;

                tstStatus.Text = "Saving ScanList.xml";
                SaveScanlist();
                tstStatus.Text = $"ScanList.xml saved! Time: {g_sw.Elapsed.Minutes}m{g_sw.Elapsed.Seconds}s ScanRate: {(int)((g_total - g_remaining_hosts_count) / g_sw.Elapsed.TotalSeconds)} hosts/s";

                g_sw.Stop();
                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.Green;
                cbShowOffline.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text == "Start";
            }
            else
            {
                g_stopped = true;
                btnStartStop.Enabled = false;

                while(g_scannner_tasks != null && g_scannner_tasks.Count > 0)
                {
                    g_scannner_tasks.RemoveAll(t => t.Status == TaskStatus.RanToCompletion);
                    Application.DoEvents();
                    Thread.Sleep(500);
                    tstStatus.Text = $"{g_scannner_tasks.Count} Stopping.. {g_total - g_remaining_hosts_count} of {g_total} Hosts Scanned!";
                    tstProgress.Value = (((g_total - g_remaining_hosts_count) * 100) / g_total);
                }

                tstStatus.Text = "Saving ScanList.xml";
                SaveScanlist();
                tstStatus.Text = "ScanList.xml saved!";

                g_sw.Stop();
                tstStatus.Text = $"Scan Stopped.. Time: {g_sw.Elapsed.Minutes}m{g_sw.Elapsed.Seconds}s ScanRate: {(int)((g_total - g_remaining_hosts_count) / g_sw.Elapsed.TotalSeconds)} hosts/s";
                btnStartStop.Text = "Start";
                btnStartStop.ForeColor = Color.Green;
                btnStartStop.Enabled = true;
                cbShowOffline.Enabled = groupBox1.Enabled = groupBox2.Enabled = groupBox3.Enabled = groupBox4.Enabled = groupBox5.Enabled = btnStartStop.Text == "Start";
            }
        }

        private void ScanRange(List<ScanListHost> unscanned)
        {           
            string _output = string.Empty;

            try
            {
                string nmapArgs = txtNmapArgs.Text.Trim().Length > 0 ? txtNmapArgs.Text.Trim() : "-sn";
                string ipRange = get_nmap_ip_range(unscanned.First().IP, unscanned.Last().IP);

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
                    
                while (!proc.StandardOutput.EndOfStream)
                {
                    _output += proc.StandardOutput.ReadLine() + Environment.NewLine;
                }
            }
            catch(Exception e) {
                Console.WriteLine($"Exception occured in scanRange {e.Message}");
            }                

            g_remaining_hosts_count -= (int)unscanned.Count;

            bool showOffline = cbShowOffline.Checked;
            string[] tokenizedScanResult = _output.Split(new[] { '\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach(var host in unscanned)
            {
                string scanResult = extractResult(tokenizedScanResult, host.IP);
                if (g_stopped)
                    break;

                g_ScanList.Host.FirstOrDefault(h => h.IP == host.IP).ScanResult = scanResult;

                if (scanResult == "Offline" && !showOffline)
                    continue;

                string[] row = { host.IP, scanResult };
                var listViewItem = new ListViewItem(row);
                AddLvItem(listViewItem);
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
            if (g_ScanList.Host == null)
                return;
            if(!cbShowOffline.Checked)
            {
                g_ScanList.Host = g_ScanList.Host.Where(h => (h.ScanResult != "Offline" && h.ScanResult != string.Empty)).ToArray();
            }
            SerializeObject(g_ScanList, "ScanList.xml");
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "notepad.exe";
            myProcess.StartInfo.Arguments = "ScanList.xml";
            myProcess.Start();
        }

        private static string get_nmap_ip_range(string startIP, string endIP)
        {
            string range = string.Empty;

            IEnumerable<string> ipRanges = new RangeFinder().GetIPRange(IPAddress.Parse(startIP), IPAddress.Parse(endIP));
            foreach (var ip in ipRanges)
                range = $"{range}{ip} ";

            return range;
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
