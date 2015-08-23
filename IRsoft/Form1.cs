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
namespace IRsoft
{
    public partial class Form1 : Form
    {
        private void NewProfile()
        {
            TProfile tp = new TProfile();
            tp.MdiParent = this;
            tp.Show();
        }

        public Form1()
        {
            InitializeComponent();
            ShowTime();
            timer1.Interval = 1000;
            timer1.Start();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NewProfile();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowTime();
        }

        private void ShowTime()
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            labelTime.Text = time.Hours.ToString("00") + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenProfile();
        }

        private void OpenProfile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                double[] x;
                double[] up;
                double[] low;
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    int number = 0;
                    string sData = sr.ReadLine();
                    if (int.TryParse(sData, out number))
                    {
                        x = new double[number];
                        up = new double[number];
                        low = new double[number];
                        for (int i = 0; i < number; i++)
                        {
                            sData = sr.ReadLine();
                            string[] d = sData.Split(';');
                            if(!double.TryParse(d[0], out x[i]))
                            {
                                return;
                            }
                            if(!double.TryParse(d[1], out up[i]))
                            {
                                return;
                            }
                            if(!double.TryParse(d[2], out low[i]))
                            {
                                return;
                            }
                        }
                    }
                    else { return; }
                }
                TProfile tp = new TProfile(x, up, low);
                tp.MdiParent = this;
                tp.Show();
            }
            
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveTermProfile();
        }

        private void SaveTermProfile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using(StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    TProfile tp = (this.ActiveMdiChild as TProfile);
                    int N = tp.GetNumberOfPoints();
                    sw.WriteLine(N.ToString());
                    double[] x = null;
                    double[] up = null;
                    double[] low = null;
                    tp.GetTempProfile(out x, out up, out low);
                    for (int i = 0; i < N; i++)
                    {
                        sw.WriteLine(String.Format("{0};{1};{2}", x[i], up[i], low[i]));
                    }
                }
            }
        }
    }
}
