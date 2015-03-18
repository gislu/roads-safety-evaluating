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
    public partial class Table : Form
    {
        public Table()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\DATA.mdb");
            OleDbDataAdapter oda = new OleDbDataAdapter("select distinct LH as 路号,LM as 路名 from DATA order by LH", conn);
            DataSet ds = new DataSet();
            oda.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            edit edit = new edit();
            edit.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string LH = res.reg;
            string query = "";
            string query2 = "";
            if (radioall.Checked == true)
            { query2 = "order by LH,QD"; }
            else if (radiop.Checked == true)
            { query2 = "and DBDJ='省级督办' order by LH,QD"; }
            else if (radiocity.Checked == true)
            { query2 = "and DBDJ='市级督办' order by LH,QD"; }
            else if (radiotown.Checked == true)
            { query2 = "and DBDJ='县级督办' order by LH,QD"; }
            try
            {
                switch (res.sw)
                {
                    case 1:
                        {
                            if (radio500.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 500M where 1=1 " + query2;
                            }
                            else if (radio2000.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 2000M where 1=1 " + query2;
                            }
                            else if (radio5200.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 2500M where 1=1 " + query2;
                            }

                            break;
                        }
                    case 0:
                        {
                            if (radio500.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 500M where LH = '" + res.reg + "'" + query2;
                            }
                            else if (radio2000.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 2000M where LH = '" + res.reg + "'" + query2;
                            }
                            else if (radio5200.Checked == true)
                            {
                                query = "select LH as 路号,LM as 路名,QD as 路段起点,ZD as 路段终点,DBDJ as 督办等级 from 2500M where LH = '" + res.reg + "'" + query2;
                            }

                            break;
                        }
                }

                OleDbConnection conn1 = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\DATA.mdb");
                OleDbDataAdapter oda1 = new OleDbDataAdapter(query, conn1);
                DataSet ds1 = new DataSet();
                oda1.Fill(ds1);
                dataGridView2.DataSource = ds1.Tables[0].DefaultView;
                DataTable dt = ds1.Tables[0];
                int r = dt.Rows.Count;
                res.info1 = new string[r];
                res.info2 = new string[r];
                res.info3 = new string[r];
                res.info4 = new string[r];
                res.info5 = new string[r];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    res.info1[i] = dt.Rows[i]["路号"].ToString();//行集合.行[号]列[名]
                    res.info2[i] = dt.Rows[i]["路名"].ToString();
                    res.info3[i] = dt.Rows[i]["路段起点"].ToString();
                    res.info4[i] = dt.Rows[i]["路段终点"].ToString();
                    res.info5[i] = dt.Rows[i]["督办等级"].ToString();
                }
                conn1.Close();
            }
            catch { MessageBox.Show("请选择完整的查询条件！"); }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int index = this.dataGridView1.SelectedRows[0].Index;
            if (index == -1)
                return;
            this.txtRegionName.Text = (string)this.dataGridView1.SelectedRows[0].Cells[1].Value;
            res.reg = (string)this.dataGridView1.SelectedRows[0].Cells[0].Value;
            res.reg1 = (string)this.dataGridView1.SelectedRows[0].Cells[1].Value;
            res.sw = 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Save Form2 = new Save();
            Form2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.txtRegionName.Text = "全部公路";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].Selected = true;
            }
            res.sw = 1;
        }
    }
}
