using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 绘图二.Path;
using System.Windows.Forms;
using System.Threading;
using 绘图二.List;

 

namespace 绘图二.AGVLable
{

    #region ///枚举方向，颜色
    public enum AGVColor            
    {
        AGVRed = 1,
        AGVGreen
    }




    public enum AGVMoveMethod   /*AGV运动方向向量*/
    {
        None = 0,
        LinUp,
        LinDown,
        LinLeft,
        LinRight,

        TurnUtoL,
        TurnUtoR,
        TurnDtoL,
        TurnDtoR,
        TurnLtoU,
        TurnLtoD,
        TurnRtoU,
        TurnRtoD
    }
    #endregion

    public class AGVClass
    {
       
        #region  /// 初始化AGV标签所用的图片引用以及动画标志位 
        /// <summary>
        /// 初始化AGV标签所用的图片引用以及动画标志位
        /// </summary>
        public static void InitAGVPic()
        {
            Config.AGVBitmapG[0] = global::绘图二.Properties.Resources.Num0;
            Config.AGVBitmapG[1] = global::绘图二.Properties.Resources.Num1g;
            Config.AGVBitmapG[2] = global::绘图二.Properties.Resources.Num2g;
            Config.AGVBitmapG[3] = global::绘图二.Properties.Resources.Num3g;
            Config.AGVBitmapG[4] = global::绘图二.Properties.Resources.Num4g;
            Config.AGVBitmapG[5] = global::绘图二.Properties.Resources.Num5g;
            Config.AGVBitmapG[6] = global::绘图二.Properties.Resources.Num6g;
            Config.AGVBitmapG[7] = global::绘图二.Properties.Resources.Num7g;
            Config.AGVBitmapG[8] = global::绘图二.Properties.Resources.Num8g;
            Config.AGVBitmapG[9] = global::绘图二.Properties.Resources.Num9g;
            Config.AGVBitmapG[10] = global::绘图二.Properties.Resources.Num10g;
            Config.AGVBitmapG[11] = global::绘图二.Properties.Resources.Num11g;
            Config.AGVBitmapG[12] = global::绘图二.Properties.Resources.Num12g;
            Config.AGVBitmapG[13] = global::绘图二.Properties.Resources.Num13g;

            Config.AGVBitmapR[0] = global::绘图二.Properties.Resources.Num0;
            Config.AGVBitmapR[1] = global::绘图二.Properties.Resources.Num1r;
            Config.AGVBitmapR[2] = global::绘图二.Properties.Resources.Num2r;
            Config.AGVBitmapR[3] = global::绘图二.Properties.Resources.Num3r;
            Config.AGVBitmapR[4] = global::绘图二.Properties.Resources.Num4r;
            Config.AGVBitmapR[5] = global::绘图二.Properties.Resources.Num5r;
            Config.AGVBitmapR[6] = global::绘图二.Properties.Resources.Num6r;
            Config.AGVBitmapR[7] = global::绘图二.Properties.Resources.Num7r;
            Config.AGVBitmapR[8] = global::绘图二.Properties.Resources.Num8r;
            Config.AGVBitmapR[9] = global::绘图二.Properties.Resources.Num9r;
            Config.AGVBitmapR[10] = global::绘图二.Properties.Resources.Num10r;
            Config.AGVBitmapR[11] = global::绘图二.Properties.Resources.Num11r;
            Config.AGVBitmapR[12] = global::绘图二.Properties.Resources.Num12r;
            Config.AGVBitmapR[13] = global::绘图二.Properties.Resources.Num13r;

            for (int i = 0; i <= Config.AGVNum; i++)
            {
                Config.AGVMoveFlag[i] = false;
                Config.PathTurnRight[i] = false;
            }
        }

        #endregion

        #region //////线程的创建  (创建线程，计算AGV下一路径)
        /// <summary>
        /// 创建AGV动画进程
        /// </summary>
        /// <param name="AGVIndex">AGV编号</param>
        public static  void AGVThread(int AGVIndex, PictureBox AGVPicBox)
        {
            Config.ThreadAGVMove[AGVIndex] = new Thread(AGVClass.threadUpdataPath);
            Config.ThreadAGVMove[AGVIndex].IsBackground = true;
            Config.ThreadAGVMove[AGVIndex].Name = string.Format("AGV{0}动画", Config.AGVNum1[AGVIndex]);
            //AGVConfig.ThreadAGVMove[AGVIndex].Priority = ThreadPriority.BelowNormal;
            Console.WriteLine("线程：‘{0}’开始", Config.ThreadAGVMove[AGVIndex].Name);
            Config.DebugList.Add(string.Format("线程：‘{0}’开始 " + DateTime.Now.ToString(), Config.ThreadAGVMove[AGVIndex].Name));
            Config.ThreadAGVMove[AGVIndex].Start((object)AGVIndex);
           
        }
        /// <summary>
        /// 自动更新AGV位置线程
        /// </summary>
        /// <param name="Number">AGV编号</param>
        public static void threadUpdataPath(object Number)
        {
            int AGVnum;
            int[] order;
            AGVnum = int.Parse(Number.ToString());
            while (true)
            {
                order = ListClass.ListPathCol(AGVnum);
                if (order != null)
                {
                    UpdataAGV(AGVnum, order[0], order[1]);
                }
                else if (Config.AGVMoveFlag[Config.AGVNum1[AGVnum]] == true)
                {

                    if (Config.PathList[Config.AGVPath1[AGVnum]] != null)
                    {
                        Thread.Sleep(Config.PathList[Config.AGVPath1[AGVnum]].Interval);
                        UpdataPath(AGVnum);
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }

                if (Config.AGVMoveFlag[Config.AGVNum1[AGVnum]] == false)
                {
                    Config.autoEvent.WaitOne(); //阻塞当前线程，等待通知以继续执行  
                }
            }
        }
        /// <summary>
        /// 计算AGV下一个路径位置（路径链表）
        /// </summary>
        /// <param name="AGVIndex">AGV编号</param>
        public static void UpdataPath(int AGVNum )
        {

            if (Config.PathList[Config.AGVPath1[AGVNum]] != null) ///开始路径不为空
            {
                if (Config.AGVLoct1[AGVNum] >= Config.PathList[Config.AGVPath1[AGVNum]].PathLength)////走完了一条路径
                {
                   Config.AGVLoct1[AGVNum] = 1; ///
                    switch (Config.AGVPath1[AGVNum])
                    {
                        case 11:
                            if (Config.AGVTar1[AGVNum] ==1 )
                            {
                                Config.AGVPath1[AGVNum] = 12;
                            }
                            else  
                            {
                                Config.AGVPath1[AGVNum] = 21;
                            }
                          
                            break;

                        case 21:

                            if (Config.AGVTar1[AGVNum] == 2)
                            {
                                Config.AGVPath1[AGVNum] = 22;
                            }
                            else 
                            {
                                Config.AGVPath1[AGVNum] = 31;
                            }
                            break;
                        case 31:

                            if (Config.AGVTar1[AGVNum] == 3)
                            {
                                Config.AGVPath1[AGVNum] = 32;
                            }
                            else
                            {
                                Config.AGVPath1[AGVNum] = 41;
                            }
                            break;
                        default:
                            Path1 pPrev = Config.PathList[Config.AGVPath1[AGVNum]];
                            foreach (int i in Config.PathList.Keys)             /**/
                            {
                                Path1 pNext = Config.PathList[i];
                                if (pNext != null && pNext != pPrev)///下一路径不为空且不是本身
                                {
                                    if (EqPoint(pPrev, pNext))
                                    {
                                        Config.AGVPath1[AGVNum] = i;  ///确定下一路径
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Config.AGVLoct1[AGVNum]++; 
                }
            }
        }


        private static bool EqPoint(Path1 p1, Path1 p2)
        {
            bool flag = false;

            if (p1.endX == p2.startX && p1.endY == p2.startY) flag = true;

            return flag;
        }
        /// <summary>
        /// 改变AGV所在路劲位置
        /// </summary>
        /// <param name="AGVIndex">AGV编号</param>
        /// <param name="path">AGV路劲</param>
        /// <param name="loct">AGV位置</param>
        public static void UpdataAGV(int AGVIndex, int path, int loct)
        {
            Config.AGVPath1[AGVIndex] = path;
           Config.AGVLoct1[AGVIndex] = loct;
        }

        #endregion   

        #region //////AGV移动位置的计算与实现
        /// <summary>
        /// 移动AGV到目标点
        /// </summary>
        /// <param name="AGVNum"></param>
        /// <param name="AGVPath"></param>
        public static void MoveAGV(int AGVNum, int AGVPath)
        {
            int CurrentPx, CurrentPy;
            PictureBox PAGVLabel = Config.AGVLabel[Config.AGVNum1[AGVNum]];
            CalPoint(AGVPath, Config.AGVLoct1[AGVNum], out CurrentPx, out CurrentPy);
            PAGVLabel.Top = CurrentPy - 6;
            PAGVLabel.Left = CurrentPx - 6;

        }

        /// <summary>
        /// 计算路劲位置所在的坐标
        /// </summary>
        /// <param name="path">路劲</param>
        /// <param name="loct">路劲上的位置</param>
        /// <param name="CurrentPx">坐标X</param>
        /// <param name="CurrentPy">坐标Y</param>
        public static void CalPoint(int path, int loct, out int CurrentPx, out int CurrentPy)
        {
            int Px = 0, Py = 0;
            double t;
            Path1 p = null;

            //Console.WriteLine(loct);

            if (Config.PathList.Keys.Contains(path))
            {
                p = Config.PathList[path];
            }

            if (p != null)
            {
                if (loct >= p.PathLength)
                {
                    Px = p.endX;
                    Py = p.endY;
                }
                else
                {
                    switch (p.Dire)
                    {
                        case Direction.DirUp:
                            Px = p.startX;
                            Py = p.startY - loct;
                            break;
                        case Direction.DirDown:
                            Px = p.startX;
                            Py = p.startY + loct;
                            break;
                        case Direction.DirLeft:
                            Px = p.startX - loct;
                            Py = p.startY;
                            break;
                        case Direction.DirRight:
                            Px = p.startX + loct;
                            Py = p.startY;
                            break;
                        case Direction.DirTnUtL:
                            t = Math.PI / 2 * (0) + Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnUtR:
                            t = Math.PI / 2 + Math.PI / 2 * (1) - Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnDtL:
                            t = Math.PI / 2 + Math.PI / 2 * (3) - Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnDtR:
                            t = Math.PI / 2 * (2) + Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnLtU:
                            t = Math.PI / 2 + Math.PI / 2 * (2) - Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnLtD:
                            t = Math.PI / 2 * (1) + Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnRtU:
                            t = Math.PI / 2 * (3) + Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        case Direction.DirTnRtD:
                            t = Math.PI / 2 + Math.PI / 2 * (4) - Math.PI / 30 * (loct);
                            Px = (int)(10 * Math.Cos(t) + p.CenterX + 0.5);
                            Py = (int)(-10 * Math.Sin(t) + p.CenterY + 0.5);
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                Px = 0;
                Py = 0;
            }

            CurrentPx = Px;
            CurrentPy = Py;
        }

        #endregion

        #region  ///////AGV是否要移动标签
        /// <summary>
        /// 启动AGV标签
        /// </summary>
        /// <param name="AGVIndex">AGV编号</param>
        public static void AGVLabelRun(int AGVNum)
        {
            Config.AGVLabel[AGVNum].Image = Config.AGVBitmapG[AGVNum];
            Config.AGVMoveFlag[AGVNum] = true;
        }

        /// <summary>
        /// 停止AGV标签
        /// </summary>
        /// <param name="AGVIndex">AGV编号</param>
        public static void AGVLabelStop(int AGVNum)
        {
            Config.AGVLabel[AGVNum].Image = Config.AGVBitmapR[AGVNum];
            Config.AGVMoveFlag[AGVNum] = false;
        }
        #endregion
    }

    }
