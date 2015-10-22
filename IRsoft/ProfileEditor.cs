using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace IRsoft
{
    public partial class ProfileEditor : Form
    {
        public Profile Profile { get; private set; }

        public ProfileEditor()
        {
            InitializeComponent();
        }

        private void ProfileEditor_Load(object sender, EventArgs e)
        {
            Build();
        }

        private Profile GetCurrentPoints()
        {
            Profile p = new Profile();
            p.Add(0, (double)numTerm1.Value);
            p.Add((double)numTime1.Value, (double)(numTerm2.Value));
            p.Add((double)(numTime1.Value + numTime2.Value), (double)(numTerm2.Value));
            p.Add((double)(numTime1.Value + numTime2.Value + numTime3.Value * 0.25m), (double)(numTerm3.Value));
            p.Add((double)(numTime1.Value + numTime2.Value + numTime3.Value), (double)numTerm3.Value);
            p.Add((double)(numTime1.Value + numTime2.Value + numTime3.Value + numTime4.Value), (double)numTerm4.Value);
            return p;
        }

        private void Build()
        {
            Profile = GetCurrentPoints();
            chart1.Series.Clear();
            chart1.Series.Add("1");
            chart1.Series[0].ChartType = SeriesChartType.Line;
            foreach (var point in Profile)
                chart1.Series[0].Points.Add(new DataPoint(point.X, point.Values));                    
        }

        private void AllNumerics_ValueChanged(object sender, EventArgs e)
        {
            Build();
        }
    }
}
