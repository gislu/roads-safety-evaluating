namespace Project
{
    partial class Analy
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.light = new System.Windows.Forms.CheckBox();
            this.weather = new System.Windows.Forms.CheckBox();
            this.speci = new System.Windows.Forms.CheckBox();
            this.road = new System.Windows.Forms.CheckBox();
            this.car = new System.Windows.Forms.CheckBox();
            this.driver = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, -2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(645, 14);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.light);
            this.panel1.Controls.Add(this.weather);
            this.panel1.Controls.Add(this.speci);
            this.panel1.Controls.Add(this.road);
            this.panel1.Controls.Add(this.car);
            this.panel1.Controls.Add(this.driver);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(43, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(559, 132);
            this.panel1.TabIndex = 1;
            // 
            // light
            // 
            this.light.AutoSize = true;
            this.light.Location = new System.Drawing.Point(394, 92);
            this.light.Name = "light";
            this.light.Size = new System.Drawing.Size(72, 16);
            this.light.TabIndex = 6;
            this.light.Text = "照明条件";
            this.light.UseVisualStyleBackColor = true;
            // 
            // weather
            // 
            this.weather.AutoSize = true;
            this.weather.Location = new System.Drawing.Point(241, 92);
            this.weather.Name = "weather";
            this.weather.Size = new System.Drawing.Size(72, 16);
            this.weather.TabIndex = 5;
            this.weather.Text = "天气条件";
            this.weather.UseVisualStyleBackColor = true;
            // 
            // speci
            // 
            this.speci.AutoSize = true;
            this.speci.Location = new System.Drawing.Point(93, 92);
            this.speci.Name = "speci";
            this.speci.Size = new System.Drawing.Size(72, 16);
            this.speci.TabIndex = 4;
            this.speci.Text = "路段类型";
            this.speci.UseVisualStyleBackColor = true;
            // 
            // road
            // 
            this.road.AutoSize = true;
            this.road.Location = new System.Drawing.Point(394, 28);
            this.road.Name = "road";
            this.road.Size = new System.Drawing.Size(72, 16);
            this.road.TabIndex = 3;
            this.road.Text = "路面状况";
            this.road.UseVisualStyleBackColor = true;
            // 
            // car
            // 
            this.car.AutoSize = true;
            this.car.Location = new System.Drawing.Point(241, 27);
            this.car.Name = "car";
            this.car.Size = new System.Drawing.Size(72, 16);
            this.car.TabIndex = 2;
            this.car.Text = "车辆状况";
            this.car.UseVisualStyleBackColor = true;
            // 
            // driver
            // 
            this.driver.AutoSize = true;
            this.driver.Location = new System.Drawing.Point(93, 27);
            this.driver.Name = "driver";
            this.driver.Size = new System.Drawing.Size(84, 16);
            this.driver.TabIndex = 1;
            this.driver.Text = "驾驶员因素";
            this.driver.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择纬度：";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(268, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "显示关联规则";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(194, 182);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(70, 21);
            this.textBox2.TabIndex = 4;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(477, 182);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(72, 21);
            this.textBox3.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "设置支持度阈值：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(329, 185);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "设置置信度阈值：";
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.GridColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView2.Location = new System.Drawing.Point(12, 269);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(661, 141);
            this.dataGridView2.TabIndex = 8;
            this.dataGridView2.Visible = false;
            // 
            // Analy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 426);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Analy";
            this.Text = "Analy";
            this.Load += new System.EventHandler(this.Analy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox light;
        private System.Windows.Forms.CheckBox weather;
        private System.Windows.Forms.CheckBox speci;
        private System.Windows.Forms.CheckBox road;
        private System.Windows.Forms.CheckBox car;
        private System.Windows.Forms.CheckBox driver;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}