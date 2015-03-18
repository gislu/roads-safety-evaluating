using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Project
{
    public partial class Save : Form
    {
        public Save()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //实例化StreamWriter对象
            string name = res.reg1;
            string lh = res.reg;
            string strpath;
            int i = 0;
            string title = "路号,路名,路段起点,路段终点,督办等级\r\n";
            string tit = System.Text.Encoding.GetEncoding("gb2312").GetString(System.Text.Encoding.Default.GetBytes(title));
            if (res.sw == 0)
            { strpath = @"D:\" + name + "(" + lh + ")" + "查询结果.csv"; }
            else
            { strpath = @"D:\" + "全部数据查询结果.csv"; }
            StreamWriter sw1 = new StreamWriter(strpath, true);

            sw1.Write(tit);
            foreach (string a in res.info1)
            {
                sw1.Write(a + ",");
                sw1.Write(res.info2[i] + ",");
                sw1.Write(res.info3[i] + ",");
                sw1.Write(res.info4[i] + ",");
                sw1.Write(res.info5[i] + "\r\n ");
                i = i + 1;
            }
            sw1.Close();
            MessageBox.Show("成功保存在D盘！");
            this.Close();
        }
    }
}
