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
using System.Windows.Forms.DataVisualization.Charting;

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
        private DataPoint m_selectedPoint;
        private Int32 m_SelectedPointIndex;
        private Series m_SelectedSeries;
        private TimeSpan esTime = new TimeSpan(0);
        int termopairs = 0;
        double[] terms;
        SerialPort sp;

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

        public TProfile(Profile Profile) : this()
        {
            RemoveLastPoint();
            for (int i = 0; i < Profile.Count; i++)
            {
                System.Windows.Forms.DataVisualization.Charting.Series series = null;
                series = (from s in charts.Series where s.Name == "UpperTermProfile" select s).FirstOrDefault();
                System.Windows.Forms.DataVisualization.Charting.DataPoint dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                dp.YValues = new double[] { Profile[i].Values[0] };
                dp.XValue = Profile[i].X;
                series.Points.Add(dp);
                series = (from s in charts.Series where s.Name == "LowerTermProfile" select s).FirstOrDefault();
                dp = new System.Windows.Forms.DataVisualization.Charting.DataPoint();
                dp.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                dp.YValues = new double[] { Profile[i].Values[0] };
                dp.XValue = Profile[i].X;
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

        private void TProfile_Load(object sender, EventArgs e)
        {
            
        }

        private void charts_MouseDown(object sender, MouseEventArgs e)
        {
            var results = charts.HitTest(e.X, e.Y);
            if (results.ChartElementType == ChartElementType.DataPoint)
            {
                m_SelectedPointIndex = results.PointIndex;
                m_SelectedSeries = results.Series;
                m_selectedPoint = results.Series.Points[m_SelectedPointIndex];
            }
        }

        private void charts_MouseUp(object sender, MouseEventArgs e)
        {
            m_selectedPoint = null;
        }

        private void charts_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_selectedPoint != null)
            {
                var total = 0.0;
                //foreach (var s in charts.Series)
                //{
                //    if (s.Equals(m_SelectedSeries)) break;
                //    total += s.Points[m_SelectedPointIndex].YValues[0];
                //}
                var prevValue = m_selectedPoint.YValues[0];
                var yValue = charts.ChartAreas[0].AxisY.PixelPositionToValue(Math.Max(Math.Min(e.Y, charts.Size.Height - 1), 0));
                yValue -= total;
                yValue = Math.Min(yValue, charts.ChartAreas[0].AxisY.Maximum);
                yValue = Math.Max(yValue, charts.ChartAreas[0].AxisY.Minimum);
                m_selectedPoint.YValues[0] = yValue;
                charts.Invalidate();
            }
            else
            {
                var hitTest = charts.HitTest(e.X, e.Y);
                if (hitTest.ChartElementType == ChartElementType.DataPoint)
                    charts.Cursor = Cursors.Hand;
                else
                    charts.Cursor = Cursors.Default;
            }
        }

        private Color GetColorByValue(Double yValue)
        {
            yValue = Math.Max(yValue, charts.ChartAreas[0].AxisY.Minimum);
            yValue = Math.Min(yValue, charts.ChartAreas[0].AxisY.Maximum);
            return Color.FromArgb(
                    /*r*/(Int32)(255 * (yValue - charts.ChartAreas[0].AxisY.Minimum) / (charts.ChartAreas[0].AxisY.Maximum - charts.ChartAreas[0].AxisY.Minimum)),
                    /*g*/0,
                    /*b*/(Int32)(255 - 255 * (yValue - charts.ChartAreas[0].AxisY.Minimum) / (charts.ChartAreas[0].AxisY.Maximum - charts.ChartAreas[0].AxisY.Minimum))
                    );
        }

        private Int32 IntToRange(Int32 value, Int32 MaxValue)
        {
            value = value % MaxValue;
            if (value < 0)
                value = MaxValue + value;
            return value;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(InitSerialPort())
            {
                toolStripButton4.Enabled = true;
                toolStripButton5.Enabled = true;
                toolStripButton6.Enabled = true;
            }
        }

        private void TProfile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sp != null)
            {
                sp.Close();
                sp.Dispose();
            }
        }
    }

    public class COM : IDisposable
    {
        private SerialPort m_SP;
        private static List<String> m_Ports;

        static COM()
        {
            m_Ports = new List<String>();
            m_Ports.AddRange(SerialPort.GetPortNames());
        }

        public static List<string> Ports { get { return m_Ports; } }

        public static COM Connect(string PortName)
        {
            if (m_Ports.Contains(PortName))
                return new COM(PortName);
            else
                return null;
        }

        public void Dispose()
        {
            m_SP.Close();
            m_SP.Dispose();
        }

        private COM(string PortName)
        {
            m_SP = new SerialPort(PortName, 9600, Parity.None);
            m_SP.NewLine = "\n";
            m_SP.Open();
        }

        public void Write(Double[] values)
        {
            try
            {
                if (values.Length != 24) throw new ArgumentOutOfRangeException();
                m_SP.WriteLine("BeginProfile");
                foreach (var value in values)
                {
                    var data = BitConverter.GetBytes(value);
                    m_SP.Write(data, 0, data.Length);
                }
                m_SP.WriteLine("EndProfile");
                var str = m_SP.ReadLine();
            }
            catch (Exception e)
            {
                String msg = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    msg += "\n" + e.Message;
                }
                Error(msg);
            }
        }

        public Double[] Read()
        {
            try
            {
                Double[] values = new Double[24];
                m_SP.WriteLine("GiveProfile");
                for (int i = 0; i < 24; i++)
                {
                    byte[] data = new byte[8];
                    if (m_SP.Read(data, 0, 8) == 8)
                        values[i] = BitConverter.ToDouble(data, 0);
                }
                return values;
            }
            catch (Exception e)
            {
                String msg = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    msg += "\n" + e.Message;
                }
                Error(msg);
                return null;
            }
        }

        private void Error(string msg)
        {
            MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
