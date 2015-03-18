using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace Project
{
    public partial class Analy : Form
    {
        public Analy()
        {
            InitializeComponent();
        }

        private void Analy_Load(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\DATA.mdb");
            OleDbDataAdapter oda = new OleDbDataAdapter("select 驾驶员因素,路段类型,照明条件,支持度,置信度 from Analy ", conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
            DataTable dt = ds.Tables[0];
            int r = dt.Rows.Count;
            res.FID = new string[r];
            res.accident = new string[r];
            res.driver = new string[r];
            res.car= new string[r];
            res.road = new string[r];
            res.speci = new string[r];
            res.weather = new string[r];
            res.light = new string[r];

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    res.FID[i] = dt.Rows[i]["FID"].ToString();//行集合.行[号]列[名]
            //    res.accident[i] = dt.Rows[i]["事故类型"].ToString();
            //    res.driver[i] = dt.Rows[i]["驾驶员因素"].ToString();
            //    res.car[i] = dt.Rows[i]["车辆状况"].ToString();
            //    res.road[i] = dt.Rows[i]["路面状况"].ToString();
            //    res.speci[i] = dt.Rows[i]["路段类型"].ToString();
            //    res.weather[i] = dt.Rows[i]["天气条件"].ToString();
            //    res.light[i] = dt.Rows[i]["照明条件"].ToString();
            //  //  textBox1.Text = textBox1.Text + "\r\n" + res.FID[i] + "\r\n" + res.accident[i] + "\r\n" + res.driver[i] + "\r\n" + res.car[i] + "\r\n" + res.road[i] + "\r\n" + res.speci[i] + "\r\n" + res.weather[i] + "\r\n" + res.light[i];
                
            //}

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.Visible = true;

        }

 


    }
}
