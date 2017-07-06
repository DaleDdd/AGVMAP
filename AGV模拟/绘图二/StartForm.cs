using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using 绘图二.Path;

namespace 绘图二
{
    public partial class StartForm : Form
    {
        string str;
        ArrayList LineList = new ArrayList();

        public StartForm()
        {
            InitializeComponent();
        }
        private void StartForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            labVersion.Text = labVersion.Text + Config.Version;

            //this.Show();
            this.timer1.Interval = 3000;
            this.timer1.Enabled = true;
         
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //关闭定时器
            this.timer1.Stop();

            //初始化Debug报告列表
            Config.DebugList.Clear();
            Config.DebugList.Add(string.Format("华为AGV中控软件 " + DateTime.Now.ToString()));

            PathClass.InitMap();
            PathClass.InitStation();
         
            this.Close();
        }


    }
}
