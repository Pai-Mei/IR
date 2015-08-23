using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace IRsoft
{
    public struct TempPoint
    {
        public int T {get; set;}
        public TimeSpan time {get; set;}

        public TempPoint(int T, TimeSpan time):this()
        {
            this.T = T;
            this.time = time;
        }
    }

    public partial class TProfile : Form
    {
        private SerialPort sp;
        private TimeSpan esTime = new TimeSpan(0);
        int termopairs = 0;
        double[] terms;

        public List<TempPoint> tempPoints = new List<TempPoint>();

        public TProfile()
        {
            InitializeComponent();
            string[] ports = SerialPort.GetPortNames();
            ListDevices.Items.Clear();
            foreach (var s in ports)
                ListDevices.Items.Add(s);
            ShowTime();
            System.Windows.Forms.DataVisualization.Charting.Series s1 =
                charts.Series.Where(i => i.Name == "UpperTermProfile").Select(i => i).FirstOrDefault();
            System.Windows.Forms.DataVisualization.Charting.Series s2 =
                charts.Series.Where(i => i.Name == "LowerTermProfile").Select(i => i).FirstOrDefault();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dp =
                new System.Windows.Forms.DataVisualization.Charting.DataPoint();
            dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            dp.XValue = 0;
            dp.YValues = new double[] { 0 };
            s1.Points.Add(dp);
            s2.Points.Add(dp);
        }

        public TProfile(double[] xData, double[] up, double[] low):this()
        {
            RemoveLastPoint();
            for (int i = 0; i < xData.Length; i++)
            {
                System.Windows.Forms.DataVisualization.Charting.Series series = null;
                series = (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault();
                System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                dp.YValues = new double[] { up[i] };
                dp.XValue = xData[i];
                series.Points.Add(dp);
                series = (from s in charts.Series where s.Name == "LowerTermProfile" select s).FirstOrDefault();
                dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                dp.YValues = new double[] { low[i] };
                dp.XValue = xData[i];
                series.Points.Add(dp);
            }
        }

        private bool InitSerialPort()
        {
            sp = new SerialPort(ListDevices.Text, 9600);
            sp.ReadTimeout = 500;
            sp.DataBits = 8;
            sp.NewLine = "\n";
            try
            {
                sp.Open();
                if (sp.IsOpen)
                {
                    sp.WriteLine("HelloDevice");
                }
                string sData = sp.ReadLine();
                if (sData == "HelloHost\r")
                {
                    sData = sp.ReadLine();
                    this.Text = "IR station " + sData.Trim('\r');
                    sData = sp.ReadLine();
                    if (!int.TryParse(sData, out termopairs))
                    {
                        MessageBox.Show("Maybe termopair not conected to device", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        for (int i = 0; i < termopairs; i++)
                        {
                            System.Windows.Forms.DataVisualization.Charting.Series s =
                                new System.Windows.Forms.DataVisualization.Charting.Series("term" + i.ToString());
                            s.Color = i == 1 ? Color.FromArgb(0xFF, 0x00, 0x00) :
                                        i == 2 ? Color.FromArgb(0x88, 0x88, 0x00) :
                                            i == 3 ? Color.FromArgb(0x00, 0x88, 0x88) :
                                                Color.FromArgb(0x55, 0x55, 0x55);
                            s.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                            charts.Series.Add(s);
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Wrong device connection!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ListDevices.Text = "Select device";
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Can't open port!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ListDevices.Text = "Select device";
                return false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            esTime = esTime.Add(new TimeSpan(0, 0, 1));
            ShowTime();
            GetDataFromDevice();
            SendCommands();
        }

        private void SendCommands()
        {
            if (sp.IsOpen)
            {
                double upValue = 0; double lowValue = 0;
                double[] x; double[] up; double[] low;
                GetTempProfile(out x, out up, out low);
                for (int i = 1; i < x.Length; i++ )
                {
                    if(x[i-1] < esTime.TotalSeconds && x[i] > esTime.TotalSeconds)
                    {
                        upValue = up[i - 1] + (up[i] - up[i - 1]) * (esTime.TotalSeconds - x[i - 1]) / (x[i] - x[i - 1]);
                        lowValue = low[i - 1] + (low[i] - low[i - 1]) / (x[i] - x[i - 1]) * (esTime.TotalSeconds - x[i - 1]);
                        break;
                    }
                }
                string str1 = "U" + upValue.ToString("0.000") + "\r\n\0";
                string str2 = "L" + lowValue.ToString("0.000") + "\r\n\0";
                sp.Write(str1);
                sp.Write(str2);
            }
        }

        private void GetDataFromDevice()
        {
            if(sp.IsOpen)
            {
                sp.WriteLine("GetTemp");
                double seconds = esTime.TotalSeconds;
                string rawData = string.Empty;
                try
                {
                    rawData = sp.ReadLine();
                }
                catch (TimeoutException e)
                {
                    return;
                }
                if (rawData == string.Empty)
                    return;
                string[] data = rawData.Replace('.', ',').Trim('\r').Split(';');
                for (int i = 0; i < data.Length; i++)
                {
                    double value = 0;
                    if (!double.TryParse(data[i], out value))
                        continue;
                    if (i < termopairs)
                    {
                        System.Windows.Forms.DataVisualization.Charting.Series series = null;
                        series = (from s in charts.Series where s.Name == "term" + i.ToString() select s).FirstOrDefault();
                        if (series != null)
                        {
                            System.Windows.Forms.DataVisualization.Charting.DataPoint dp 
                                = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                            dp.XValue = seconds;
                            dp.YValues = new double[] { value};
                            series.Points.Add(dp);
                        }
                    }
                }
            }
        }

        private void ShowTime()
        {
            string h = esTime.Hours < 10 ? "0" + esTime.Hours.ToString() : esTime.Hours.ToString();
            string m = esTime.Minutes < 10 ? "0" + esTime.Minutes.ToString() : esTime.Minutes.ToString();
            string s = esTime.Seconds < 10 ? "0" + esTime.Seconds.ToString() : esTime.Seconds.ToString();
            labelTime.Text = String.Format("{0}:{1}:{2}",h, m, s);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void ListDevices_TextChanged(object sender, EventArgs e)
        {
            if (ListDevices.Text != "Select device")
            {
               if(InitSerialPort()){
                   toolStripButton4.Enabled = true;
                   toolStripButton4.Enabled = true;
                   toolStripButton4.Enabled = true;
               } else {
                   toolStripButton4.Enabled = false;
                   toolStripButton4.Enabled = false;
                   toolStripButton4.Enabled = false;
               }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            RemoveLastPoint();
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddNewPoint();
        }

        private void RemoveLastPoint()
        {
            System.Windows.Forms.DataVisualization.Charting.Series series = null;
            series = (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault();
            if (series.Points.Any())
                series.Points.Remove(series.Points.Last());
            series = (from s in charts.Series where s.Name == "LowerTermProfile" select s).FirstOrDefault();
            if (series.Points.Any())
            series.Points.Remove(series.Points.Last());
        }

        private void AddNewPoint()
        {
            double upTemp = 0;
            double lowTemp = 0;
            double time = 0;
            if (!double.TryParse(textBoxTempUp.Text, out upTemp))
            {
                MessageBox.Show("Wrong value of temperature!");
                return;
            }
            if (!double.TryParse(textBoxTempLow.Text, out lowTemp))
            {
                MessageBox.Show("Wrong value of temperature!");
                return;
            }
            if (!double.TryParse(textBoxTime.Text, out time))
            {
                MessageBox.Show("Wrong value of time!");
                return;
            }

            System.Windows.Forms.DataVisualization.Charting.Series series = null;
            series = (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault();
            System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
            dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            dp.YValues = new double[] { upTemp };
            dp.XValue = time;
            series.Points.Add(dp);
            series = (from s in charts.Series where s.Name == "LowerTermProfile" select s).FirstOrDefault();
            dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
            dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            dp.YValues = new double[] { lowTemp };
            dp.XValue = time;
            series.Points.Add(dp);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddNewPoint();
        }

        public int GetNumberOfPoints()
        {
            return (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault().Points.Count;
        }

        public void GetTempProfile(out double[] x, out double[] up, out double[] low)
        {
            System.Windows.Forms.DataVisualization.Charting.Series s1 = null;
            System.Windows.Forms.DataVisualization.Charting.Series s2 = null;
            s1 = (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault();
            s2 = (from s in charts.Series where s.Name == "LowerTermProfile" select s).FirstOrDefault();
            int Count = s1.Points.Count;
            x = new double[Count];
            up = new double[Count];
            low = new double[Count];
            for(int i = 0; i < Count; i++)
            {
                x[i] = s1.Points[i].XValue;
                up[i] = s1.Points[i].YValues[0];
                low[i] = s2.Points[i].YValues[0];
            }

        }
    }
}
