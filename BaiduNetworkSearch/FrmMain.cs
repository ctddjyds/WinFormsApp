using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace BaiduNetworkSearch
{
    public partial class FrmMain : Form
    {
        bool isSearch = true;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string key = this.txtKey.Text;
            if (!string.IsNullOrEmpty(key))
            {
                this.dataGridView1.Rows.Clear();
                this.lblResult.Text = "0";
                this.pgsBar.Value = 0;
                this.btnSearch.Text = "正在搜索";
                this.btnSearch.Enabled = false;
                this.btnStop.Enabled = true;
                Thread thread = new Thread(() =>
                {
                    for (int i = 0; i < 100; i += 10)
                    {
                        if (isSearch)
                        {
                            SearchResult sr = HttpHelper.Requset(key, i.ToString());
                            if (sr != null)
                            {
                                foreach (BDWPResource resource in sr.resources)
                                {
                                    BindResource(resource);
                                }
                            }
                        }
                        else break;
                    }
                    //搜索完成
                    SearchOver();
                });
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void BindResource(BDWPResource resource)
        {
            string title = resource.title.Replace("</b>", "").Replace("<b>", "");
            string content = resource.content.Replace("</b>", "").Replace("<b>", "");

            this.Invoke(new Action<string, string, string>((tle, ctt, url) =>
            {
                this.dataGridView1.Rows.Add(tle, ctt, url);
                this.lblResult.Text = (Int32.Parse(this.lblResult.Text) + 1).ToString();
                if (this.pgsBar.Value < this.pgsBar.Maximum)
                {
                    this.pgsBar.Value++;
                }
            }), title, content, resource.unescapedUrl);
        }

        private void SearchOver()
        {
            this.Invoke(new Action(() =>
            {
                this.btnSearch.Text = "开始搜索";
                this.btnSearch.Enabled = true;
                this.btnStop.Enabled = false;
                this.isSearch = true;
            }));
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), this.dataGridView1.DefaultCellStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 6);
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(new Point(e.RowBounds.Location.X + 2, e.RowBounds.Location.Y + 2), new Size(20, 20)));//隐藏每行前面的图标
        }

        //打开网页链接
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                string url = this.dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                Process.Start(url);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            isSearch = false;
            this.btnSearch.Enabled = true;
        }
    }
}
