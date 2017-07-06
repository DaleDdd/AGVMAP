using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 绘图二.Path;
using 绘图二.AGVLable;


namespace 绘图二
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

            #region////////打开画布
        public void AGVMap_Paint(object sender, PaintEventArgs e)
          {
              Graphics g = e.Graphics;                  
              foreach (int i in Config.PathList.Keys)               //画线
              {
                  if (Config.PathList[i] != null)
                  {
                      Config.PathList[i].pathDraw(g);
                  }
              }
                  SolidBrush b;

              foreach (int i in Config.Station.Keys)                //画地标
              {
               
                  b = new SolidBrush(Color.Blue);
                  if (Config.Station.Keys != null)
                  {
                      Config.Station[i].Draw(g, b);
                  }
                 
              }
          }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region ///////打开启动窗口，并初始化
            /*启动欢迎窗体*/
            StartForm  MyStartForm = new StartForm ();
            MyStartForm.ShowDialog();
            BringToFront();///移到前面显示
            /*调整锁定窗体大小*/
            this.Width = 1440;
            this.Height = 870;
            this.Top = 0;
            this.Left = 0;

            /*修改窗体标题*/
            this.Text = "AGV调度系统                                                                  佛山市顺德区嘉腾电子有限公司 http://www.sdpaten.com";

            /*调整画布位置大小*/
            AGVMap.Top = Config.MapInitY;
            AGVMap.Left = Config.MapInitX;
            AGVMap.Width = 1400;
            AGVMap.Height = 800;
            #endregion

            #region  /*下拉单数据添加*/  /*初始化并实例化AGV标签，AGV站点*/

            ComAGVNum.DropDownStyle = ComboBoxStyle.DropDownList;
            ComAGVPath.DropDownStyle = ComboBoxStyle.DropDownList;
            ComAGVTar.DropDownStyle = ComboBoxStyle.DropDownList;

            for (int i = 0; i <= Config.AGVNum; i++)
            {
                ComAGVNum.Items.Add(i);

            }
            ComAGVPath.Items.Add(11);
            ComAGVPath.Items.Add(21);
            ComAGVPath.Items.Add(31);
            ComAGVPath.Items.Add(41);

            for (int i = 1; i <= 4; i++)
            {
                ComAGVTar.Items.Add(i);
            }
           
            AGVClass.InitAGVPic();

            Config.StationPic[0] =  PicTarget01;
            Config.StationPic[1] = PicTarget02;
            Config.StationPic[2] = PicTarget03;
            Config.StationPic[3] = PicTarget04;
            Config.StationPic[4] = PicTarget05;
            Config.StationPic[5] = PicTarget06;
            Config.StationPic[6] = PicTarget07;
            Config.StationPic[7] = PicTarget08; 

            Config.AGVLabel[0] = AGVPic0;
            Config.AGVLabel[1] = AGVPic1;
            Config.AGVLabel[2] = AGVPic2;
            Config.AGVLabel[3] = AGVPic3;
            Config.AGVLabel[4] = AGVPic4;
            Config.AGVLabel[5] = AGVPic5;
            Config.AGVLabel[6] = AGVPic6;
            Config.AGVLabel[7] = AGVPic7;
            Config.AGVLabel[8] = AGVPic8;
            Config.AGVLabel[9] = AGVPic9;
            Config.AGVLabel[10] = AGVPic10;
            Config.AGVLabel[11] = AGVPic11;
            Config.AGVLabel[12] = AGVPic12;
            Config.AGVLabel[13] = AGVPic13;
            TimMoveAGV.Enabled = true;
            #endregion
    
    }
     
            #region ////移动窗口的代码
        Point InitPoint; 
        private void labelTestMove_MouseDown(object sender, MouseEventArgs e)
        {
           
            BoxCartoon.BringToFront();
            InitPoint = e.Location;
            labelTestMove.MouseMove += new MouseEventHandler(labelTestMove_MouseMove);
        }

        void labelTestMove_MouseMove(object sender, MouseEventArgs e)
        {
            BoxCartoon.Location = new Point(BoxCartoon.Location.X + e.Location.X - InitPoint.X, BoxCartoon.Location.Y + e.Location.Y - InitPoint.Y);
         
        }

        private void labelTestMove_MouseUp(object sender, MouseEventArgs e)
        {
            labelTestMove.MouseMove -= new MouseEventHandler(labelTestMove_MouseMove);
        }

        #endregion

        #region //////控制按钮代码
        private void BtnADD_Click(object sender, EventArgs e)
        {
            Config.i++; 
            Config.Equal = "false";
            try
            {              
                for (int j=0 ;j<Config.i;j++)
                {
                    if (Config.AGVNum1[j] == int.Parse(ComAGVNum.Text)) ///如果输入的AGV已经存在，那么就把当前的目的地值赋给它
                    {
                        Config.Equal = "true";
                        Config.AGVTar1[j] = (int.Parse(ComAGVTar.Text));
                        Config.i--;
                    }
                       
                }          
                if (Config.Equal == "false")
                {
                    Config.AGVNum1.Add(int.Parse(ComAGVNum.Text));
                    Config.AGVPath1.Add(int.Parse(ComAGVPath.Text));
                    Config.AGVTar1.Add(int.Parse(ComAGVTar.Text));
                }
            }
            catch
            {             
                for (int j = 0; j < Config.i; j++)
                {
                    if (Config.AGVNum1[j] == 0)
                    {
                        Config.Equal = "true";
                        Config.i--;
                    }
                }
                if (Config.Equal == "false")
                {
                    Config.AGVNum1.Add(0);
                    Config.AGVPath1.Add(11);
                    Config.AGVTar1.Add(1);
                }
            }
            Config.AGVLoct1.Add( 1) ;  ///////路径上的位置 
            Config.AGVStar1.Add(1);
            if (Config.Equal == "false")
            {
                AGVClass.AGVThread(Config.i, Config.AGVLabel[Config.AGVNum1[Config.i]]); //创建线程
            }
            AGVClass.AGVLabelStop(Config.AGVNum1[Config.i]);  //阻塞线程             
            btnAGVRun.Text = "开";


             if (Config.AGVLabel[Config.i] != null)   ///////使增加的AGV可视
             {
                 if (Config.AGVStar1[Config.i] == 1)
                 {
                     AGVClass.MoveAGV(Config.i, Config.AGVPath1[Config.i ]); 

                     Config.AGVLabel[Config.AGVNum1[Config.i]].Visible = true;
                 }
             }

            
        }

        private void btnAGVRun_Click_1(object sender, EventArgs e)
        {

            if (btnAGVRun.Text == "开")
            {
                btnAGVRun.Text = "停";
                for (int j = 0; j <= Config.i; j++)
                {
                    Config.AGVStar1[j] = 1;
                    AGVClass.AGVLabelRun(Config.AGVNum1[j])   ;
                }
                for (int j = 4000; j > 0;j-- )    //*使有足够时间去通知所有阻塞线程*/
                {
                    Config.autoEvent.Set(); //通知阻塞的线程继续执行  
                }
            }
            else
            {
                btnAGVRun.Text = "开";
                for (int j = 0; j <= Config.i; j++)
                {
                    AGVClass.AGVLabelStop(Config.AGVNum1[j ]);     /*j 表示Config.AGVNum1第j个的内容*/ //暂停所有AGV线程
                    Config.AGVStar1[j] =0;                         /////停止移动AGV
                }
            }
        }


        private void TimMoveAGV_Tick(object sender, EventArgs e)
        {

          

            for (int j = 0; j <= Config.i; j++)
            {

                if (Config.AGVLabel[j] != null)
                {
                     if (Config.AGVStar1[j] == 1)
                        AGVClass.MoveAGV(j, Config.AGVPath1[j]);   /*更新AGVLabel的位置*/
                }

            }
           
        }
        #endregion

    }
}
