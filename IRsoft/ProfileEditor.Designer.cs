namespace IRsoft
{
    partial class ProfileEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numTime4 = new System.Windows.Forms.NumericUpDown();
            this.numTerm4 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numTime3 = new System.Windows.Forms.NumericUpDown();
            this.numTime2 = new System.Windows.Forms.NumericUpDown();
            this.numTime1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numTerm3 = new System.Windows.Forms.NumericUpDown();
            this.numTerm2 = new System.Windows.Forms.NumericUpDown();
            this.numTerm1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numTime4);
            this.groupBox1.Controls.Add(this.numTerm4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.numTime3);
            this.groupBox1.Controls.Add(this.numTime2);
            this.groupBox1.Controls.Add(this.numTime1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numTerm3);
            this.groupBox1.Controls.Add(this.numTerm2);
            this.groupBox1.Controls.Add(this.numTerm1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 336);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(473, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры";
            // 
            // numTime4
            // 
            this.numTime4.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTime4.Location = new System.Drawing.Point(253, 108);
            this.numTime4.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numTime4.Name = "numTime4";
            this.numTime4.Size = new System.Drawing.Size(120, 20);
            this.numTime4.TabIndex = 14;
            this.numTime4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numTime4.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // numTerm4
            // 
            this.numTerm4.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTerm4.Location = new System.Drawing.Point(116, 108);
            this.numTerm4.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numTerm4.Name = "numTerm4";
            this.numTerm4.Size = new System.Drawing.Size(120, 20);
            this.numTerm4.TabIndex = 13;
            this.numTerm4.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numTerm4.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Остываение:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(386, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // numTime3
            // 
            this.numTime3.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTime3.Location = new System.Drawing.Point(253, 82);
            this.numTime3.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numTime3.Name = "numTime3";
            this.numTime3.Size = new System.Drawing.Size(120, 20);
            this.numTime3.TabIndex = 10;
            this.numTime3.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
            this.numTime3.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // numTime2
            // 
            this.numTime2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTime2.Location = new System.Drawing.Point(253, 57);
            this.numTime2.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numTime2.Name = "numTime2";
            this.numTime2.Size = new System.Drawing.Size(120, 20);
            this.numTime2.TabIndex = 9;
            this.numTime2.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            this.numTime2.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // numTime1
            // 
            this.numTime1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTime1.Location = new System.Drawing.Point(253, 33);
            this.numTime1.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numTime1.Name = "numTime1";
            this.numTime1.Size = new System.Drawing.Size(120, 20);
            this.numTime1.TabIndex = 8;
            this.numTime1.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numTime1.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(290, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Время(сек)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(136, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Температура(C°)";
            // 
            // numTerm3
            // 
            this.numTerm3.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTerm3.Location = new System.Drawing.Point(116, 82);
            this.numTerm3.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numTerm3.Name = "numTerm3";
            this.numTerm3.Size = new System.Drawing.Size(120, 20);
            this.numTerm3.TabIndex = 5;
            this.numTerm3.Value = new decimal(new int[] {
            220,
            0,
            0,
            0});
            this.numTerm3.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // numTerm2
            // 
            this.numTerm2.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTerm2.Location = new System.Drawing.Point(116, 57);
            this.numTerm2.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.numTerm2.Name = "numTerm2";
            this.numTerm2.Size = new System.Drawing.Size(120, 20);
            this.numTerm2.TabIndex = 4;
            this.numTerm2.Value = new decimal(new int[] {
            170,
            0,
            0,
            0});
            this.numTerm2.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // numTerm1
            // 
            this.numTerm1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numTerm1.Location = new System.Drawing.Point(116, 33);
            this.numTerm1.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTerm1.Name = "numTerm1";
            this.numTerm1.Size = new System.Drawing.Size(120, 20);
            this.numTerm1.TabIndex = 3;
            this.numTerm1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numTerm1.ValueChanged += new System.EventHandler(this.AllNumerics_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Оплавление:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Вымачивание:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Преднагрев:";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(473, 336);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // ProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 471);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(489, 509);
            this.Name = "ProfileEditor";
            this.Text = "ProfileEditor";
            this.Load += new System.EventHandler(this.ProfileEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTime4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTime1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTerm1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.NumericUpDown numTime3;
        private System.Windows.Forms.NumericUpDown numTime2;
        private System.Windows.Forms.NumericUpDown numTime1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numTerm3;
        private System.Windows.Forms.NumericUpDown numTerm2;
        private System.Windows.Forms.NumericUpDown numTerm1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numTime4;
        private System.Windows.Forms.NumericUpDown numTerm4;
        private System.Windows.Forms.Label label6;
    }
}