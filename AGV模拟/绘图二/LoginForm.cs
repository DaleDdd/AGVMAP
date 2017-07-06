using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 绘图二
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*启动欢迎窗体*/
          MainForm form1 = new MainForm();
            form1.ShowDialog();
            //Rectangle ScreenArea = Screen.GetWorkingArea(this);
            //this.Width = ScreenArea.Width;
            //this.Height = ScreenArea.Height;
        }



        //private void btnEnter_Click(object sender, EventArgs e)
        //{
        //    if (txtUser.Text == "JT_AGV" && txtPassword.Text == Config.password)
        //    {
        //        From1.UserFlag = true;
        //        this.Close();
        //    }
        //    else
        //    {
        //        MessageBox.Show("用户名或密码错误！");
        //    }
        //}

        //private void btnCancle_Click(object sender, EventArgs e)
        //{
        //    From1.UserFlag = false;
        //    this.Close();
        //}



    }
}
