using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlDependencyApp
{
    public partial class Form1 : Form
    {
        //监听数据库连接
        private string connectionString = @"Data Source=192.168.16.190\HTFT;Initial Catalog=WCSFORJH;User ID=JH;Password=wcsforjh008;Connect Timeout=30;Pooling=true;Max Pool Size=400;Min Pool Size=0";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlDependency.Start(connectionString);//传入连接字符串,启动基于数据库的监听
            UpdateGrid();
        }
        private void UpdateGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //依赖是基于某一张表的,而且查询语句只能是简单查询语句,不能带top或*,同时必须指定所有者,即类似[dbo].[]  
                using (SqlCommand command = new SqlCommand("select PalletNO,SHIP_BATCH From dbo.FT_PalletInfo", connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    SqlDataReader sdr = command.ExecuteReader();
                    this.richTextBox1.AppendText("\r\n");
                    while (sdr.Read())
                    {
                        this.richTextBox1.AppendText(string.Format("USERS:{0}\tPASSWORD:{1}\t", sdr["Username"].ToString(), sdr["PasswordHash"].ToString()));
                    }
                    sdr.Close();
                }
            }
        }

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change && e.Info==SqlNotificationInfo.Insert) //只有数据发生变化时,才重新获取
            {
                if(this.InvokeRequired)
                {
                    this.Invoke(new Action(()=>{ this.richTextBox1.AppendText("data is change"); }));
                }
                else
                {
                    this.richTextBox1.AppendText("data is change");
                }
            }
        }  

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDependency.Stop(connectionString);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SqlDependency.Stop(connectionString);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                this.richTextBox1.AppendText("连接成功\r\n");
                connection.Close();
            }
        }
    }
}
