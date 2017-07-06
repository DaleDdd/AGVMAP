using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using 绘图二.AGVLable;
using System.Windows.Forms;

namespace 绘图二.Path
{
    #region ///枚举方向
    public enum Direction
    {
        None = 0,
        DirUp,
        DirDown,
        DirLeft,
        DirRight,

        DirTnUtL,
        DirTnUtR,
        DirTnDtL,
        DirTnDtR,
        DirTnLtU,
        DirTnLtD,
        DirTnRtU,
        DirTnRtD
    }
    #endregion

   
    public class PathClass
    { 
     
        #region  ///路线绘制初始化

        public static void InitMap()////初始化地图
        {
            const int 间隔 = 200;
            Dictionary<int, Path1> list = Config.PathList;
            Point 原点 = new Point(Config.InitX, Config.InitY);
            Point p1 = new Point(350, 100);
            Point p2 = new Point(520, 100);


            list[11] = new Path1(p1, Direction.DirRight, 150);
            list[12] = new Path1(list[11], Direction.DirDown);
            list[13] = new Path1(list[12], 500);
            list[14] = new Path1(list[13], Direction.DirLeft);
            list[15] = new Path1(list[14], 150);
            list[16] = new Path1(list[15], Direction.DirUp);
            list[17] = new Path1(list[16], 500);
            list[18] = new Path1(list[17], Direction.DirRight);

            list[21] = new Path1(list[11],  170);
            list[22] = new Path1(list[21], Direction.DirDown);
            list[23] = new Path1(list[22], 500);
            list[24] = new Path1(list[23], Direction.DirLeft);
            list[25] = new Path1(list[24], 170);


            list[31] = new Path1(list[21], 170);
            list[32] = new Path1(list[31], Direction.DirDown);
            list[33] = new Path1(list[32], 500);
            list[34] = new Path1(list[33], Direction.DirLeft);
            list[35] = new Path1(list[34], 170);


            list[41] = new Path1(list[31], 170);
            list[42] = new Path1(list[41], Direction.DirDown);
            list[43] = new Path1(list[42], 500);
            list[44] = new Path1(list[43], Direction.DirLeft);
            list[45] = new Path1(list[44], 170);

        }
        /// <summary>
        /// 地标参数
        /// </summary>
        public static void InitStation()
        {
            Dictionary<int, Station> s =Config.Station;
            s[Config.j] = new Station(Config.j, 13, 0.5);
            Config.j++;
            s[Config.j] = new Station(Config.j, 23, 0.5);
            Config.j++;
            s[Config.j] = new Station(Config.j, 33, 0.5);
            Config.j++;
            s[Config.j] = new Station(Config.j, 43, 0.5);
        }

      
        /// <summary>
        /// 初始化小车移动速度
        /// </summary>
        public static void InitTime()
        {
            Dictionary<int, Path1> p = Config.PathList;


        }
    }

    #endregion

        #region ///路线绘制代码
    public class Path1
    {


        private int _PathLength;
        private int _startX;
        private int _startY;
        private int _endX;
        private int _endY;
        private int _CenterX;
        private int _CenterY;
        private Direction _Dire;
        private int _Interval;
        private Rectangle _rect;


        public int PathLength { get { return _PathLength; } }     /// 返回该路劲长度
        public int startX { get { return _startX; } }             /// 返回该路劲起始点X坐标
        public int startY { get { return _startY; } }             /// 返回该路劲起始点Y坐标
        public int endX { get { return _endX; } }                 /// 返回该路劲结束点X坐标
        public int endY { get { return _endY; } }                 /// 返回该路劲结束点Y坐标
        public int CenterX { get { return _CenterX; } }           /// 返回该转弯路劲圆心X坐标，直线为0
        public int CenterY { get { return _CenterY; } }           /// 返回该转弯路劲圆心Y坐标，直线为0
        public Direction Dire { get { return _Dire; } }           /// 返回该路劲的方向
        public int Interval { get { return _Interval; } }         /// 返回改路劲的小车前进时间间隔
        public Rectangle rect { get { return _rect; } }           /// 返回该转弯路劲弧线所在的矩形框，直线为null

        private int DRAWstartX { get { return _startX - Config.MapInitX; } }
        private int DRAWstartY { get { return _startY - Config.MapInitY; } }
        private int DRAWendX { get { return _endX - Config.MapInitX; } }
        private int DRAWendY { get { return _endY - Config.MapInitY; } }
        #region  构造函数
        /// <summary>
        /// 直线路径
        /// </summary>
        /// <param name="InitP">直线的起始点</param>
        /// <param name="Dir">直线的方向</param>
        /// <param name="length">直线的长度</param>
        public Path1(Point InitP, Direction Dir, int length)
        {
            Point FinP = new Point(InitP.X, InitP.Y);               //终点坐标
            switch (Dir)
            {
                case Direction.DirUp:
                    FinP.X = InitP.X;
                    FinP.Y = InitP.Y - length;
                    break;
                case Direction.DirDown:
                    FinP.X = InitP.X;
                    FinP.Y = InitP.Y + length;
                    break;
                case Direction.DirLeft:
                    FinP.X = InitP.X - length;
                    FinP.Y = InitP.Y;
                    break;
                case Direction.DirRight:
                    FinP.X = InitP.X + length;
                    FinP.Y = InitP.Y;
                    break;
                default:
                    break;
            }
            _startX = InitP.X;
            _startY = InitP.Y;
            _endX = FinP.X;
            _endY = FinP.Y;
            _Dire = Dir;
            _PathLength = length;
            _Interval = Config.AGVtime;
        }

        /// <summary>
        /// </summary>
        /// <param name="InitP">圆弧的起始点</param>
        /// <param name="DirInit">圆弧的起始方向</param>
        /// <param name="DirFin">圆弧的的最后方向</param>
        public Path1(Point InitP, Direction DirInit, Direction DirFin)
        {
            _rect = new Rectangle(InitP.X, InitP.Y, 20, 20);
            _startX = InitP.X;
            _startY = InitP.Y;
            switch (DirInit)
            {
                case Direction.DirUp:
                    if (DirFin == Direction.DirLeft)
                    {
                        _rect.Location = new Point(InitP.X - 20, InitP.Y - 10);
                        _endX = _startX - 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnUtL;
                    }
                    else if (DirFin == Direction.DirRight)
                    {
                        _rect.Location = new Point(InitP.X, InitP.Y - 10);
                        _endX = _startX + 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnUtR;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _endX;
                    _CenterY = _startY;
                    break;
                case Direction.DirDown:
                    if (DirFin == Direction.DirLeft)
                    {
                        _rect.Location = new Point(InitP.X - 20, InitP.Y - 10);
                        _endX = _startX - 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnDtL;
                    }
                    else if (DirFin == Direction.DirRight)
                    {
                        _rect.Location = new Point(InitP.X, InitP.Y - 10);
                        _endX = _startX + 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnDtR;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _endX;
                    _CenterY = _startY;
                    break;
                case Direction.DirLeft:
                    if (DirFin == Direction.DirUp)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y - 20);
                        _endX = _startX - 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnLtU;
                    }
                    else if (DirFin == Direction.DirDown)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y);
                        _endX = _startX - 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnLtD;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
                case Direction.DirRight:
                    if (DirFin == Direction.DirUp)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y - 20);
                        _endX = _startX + 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnRtU;
                    }
                    else if (DirFin == Direction.DirDown)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y);
                        _endX = _startX + 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnRtD;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
                default:
                    _endX = _startX;
                    _endY = _startY;
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
            }
            _rect.X -= Config.MapInitX;
            _rect.Y -= Config.MapInitY;
            _PathLength = 15;
            _Interval = Config.AGVtime;
        }
     

      
        /// <summary>
        /// </summary>
        /// <param name="Lastpath">上一条轨迹的实例</param>
        /// <param name="length">直线的长度</param>
        public Path1(Path1 Lastpath, int length)
        {
            Point FinP = new Point(Lastpath.endX, Lastpath.endY);
            switch (Lastpath.Dire)
            {
                case Direction.DirUp:
                case Direction.DirTnLtU:
                case Direction.DirTnRtU:
                    FinP.X = Lastpath.endX;
                    FinP.Y = Lastpath.endY - length;
                    _Dire = Direction.DirUp;
                    break;
                case Direction.DirDown:
                case Direction.DirTnLtD:
                case Direction.DirTnRtD:
                    FinP.X = Lastpath.endX;
                    FinP.Y = Lastpath.endY + length;
                    _Dire = Direction.DirDown;
                    break;
                case Direction.DirLeft:
                case Direction.DirTnUtL:
                case Direction.DirTnDtL:
                    FinP.X = Lastpath.endX - length;
                    FinP.Y = Lastpath.endY;
                    _Dire = Direction.DirLeft;
                    break;
                case Direction.DirRight:
                case Direction.DirTnUtR:
                case Direction.DirTnDtR:
                    FinP.X = Lastpath.endX + length;
                    FinP.Y = Lastpath.endY;
                    _Dire = Direction.DirRight;
                    break;
                default:
                    break;
            }
            _startX = Lastpath.endX;
            _startY = Lastpath.endY;
            _endX = FinP.X;
            _endY = FinP.Y;
            _CenterX = FinP.X;
            _CenterY = FinP.Y;
            _PathLength = length;
            _Interval = Config.AGVtime;

        }
     

       
        /// <summary>
       /// 画轨道：转弯圆弧（简化版）
        /// </summary>
        /// <param name="Lastpath">上一条轨迹的实例</param>
        /// <param name="DirFin">圆弧的最后方向</param>
        public Path1(Path1 Lastpath, Direction DirFin)
        {
            Point InitP = new Point(Lastpath.endX, Lastpath.endY);
            _rect = new Rectangle(InitP.X, InitP.Y, 20, 20);

            _startX = InitP.X;
            _startY = InitP.Y;
            switch (Lastpath.Dire)
            {
                case Direction.DirUp:
                case Direction.DirTnLtU:
                case Direction.DirTnRtU:
                    if (DirFin == Direction.DirLeft)
                    {
                        _rect.Location = new Point(InitP.X - 20, InitP.Y - 10);
                        _endX = _startX - 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnUtL;
                    }
                    else if (DirFin == Direction.DirRight)
                    {
                        _rect.Location = new Point(InitP.X, InitP.Y - 10);
                        _endX = _startX + 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnUtR;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _endX;
                    _CenterY = _startY;
                    break;
                case Direction.DirDown:
                case Direction.DirTnLtD:
                case Direction.DirTnRtD:
                    if (DirFin == Direction.DirLeft)
                    {
                        _rect.Location = new Point(InitP.X - 20, InitP.Y - 10);
                        _endX = _startX - 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnDtL;
                    }
                    else if (DirFin == Direction.DirRight)
                    {
                        _rect.Location = new Point(InitP.X, InitP.Y - 10);
                        _endX = _startX + 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnDtR;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _endX;
                    _CenterY = _startY;
                    break;
                case Direction.DirLeft:
                case Direction.DirTnUtL:
                case Direction.DirTnDtL:
                    if (DirFin == Direction.DirUp)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y - 20);
                        _endX = _startX - 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnLtU;
                    }
                    else if (DirFin == Direction.DirDown)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y);
                        _endX = _startX - 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnLtD;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
                case Direction.DirRight:
                case Direction.DirTnUtR:
                case Direction.DirTnDtR:
                    if (DirFin == Direction.DirUp)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y - 20);
                        _endX = _startX + 10;
                        _endY = _startY - 10;
                        _Dire = Direction.DirTnRtU;
                    }
                    else if (DirFin == Direction.DirDown)
                    {
                        _rect.Location = new Point(InitP.X - 10, InitP.Y);
                        _endX = _startX + 10;
                        _endY = _startY + 10;
                        _Dire = Direction.DirTnRtD;
                    }
                    else
                    {
                        _endX = _startX;
                        _endY = _startY;
                    }
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
                default:
                    _endX = _startX;
                    _endY = _startY;
                    _CenterX = _startX;
                    _CenterY = _endY;
                    break;
            }
            _rect.X -= Config.MapInitX;
            _rect.Y -= Config.MapInitY;
            _PathLength = 15;
            _Interval = Config.AGVtime;
        }
        #endregion

        #endregion

        #region//绘制路线

        public void pathDraw(Graphics g)
        {
            Pen pen = new Pen(Color.Green, 1);
            switch (Dire)
            {
                case Direction.DirUp:
                case Direction.DirDown:
                case Direction.DirLeft:
                case Direction.DirRight:
                    g.DrawLine(pen, DRAWstartX, DRAWstartY, DRAWendX, DRAWendY);
                    break;
                case Direction.DirTnRtD:
                case Direction.DirTnUtL:
                    g.DrawArc(pen, rect, 270, 90);
                    break;
                case Direction.DirTnRtU:
                case Direction.DirTnDtL:
                    g.DrawArc(pen, rect, 0, 90);
                    break;
                case Direction.DirTnLtD:
                case Direction.DirTnUtR:
                    g.DrawArc(pen, rect, 180, 90);
                    break;
                case Direction.DirTnLtU:
                case Direction.DirTnDtR:
                    g.DrawArc(pen, rect, 90, 90);
                    break;
            }
        }
        #endregion
    }

    #region  ///画出地标
    public class Station
    {
        private int _path;
        private int _loct;
        private int _CenX;
        private int _CenY;
        private int _Num;

        public int path { get { return _path; } }
        public int loct { get { return _loct; } }
        public int CenX { get { return _CenX; } }
        public int CenY { get { return _CenY; } }
        public int Num { get { return _Num; } }

        /// <summary>
        /// 构造地标（地标数组）
        /// </summary>
        /// <param name="Path">地标所在路径</param>
        /// <param name="Percent">地标所在位置相对于路劲长度的百分比</param>
        public Station(int StationNum,int Path, double Percent)
        {
            _Num = StationNum;
            _path = Path;
           
          
            if (Config.PathList[path] != null)
            {
                _loct = (int)(Config.PathList[path].PathLength * Percent);
                if (_loct < 1)
                {
                    _loct = 1;
                }
                AGVClass.CalPoint(_path, _loct, out _CenX, out _CenY);
            }
            else
            {
                _loct = 0;
                _CenX = 0;
                _CenY = 0;
            }
        }

        /// <summary>
        /// 画出地标
        /// </summary>
        /// <param name="g">绘图画板</param>
        public void Draw(Graphics g, SolidBrush b)
        {
            g.FillEllipse(b, new Rectangle(CenX - 3 - Config.MapInitX, CenY - 3 - Config.MapInitY, 7, 7));
            Config.StationPic[Num].Top = CenY;
            Config.StationPic[Num].Left = CenX + 6;
            Config.StationPic[Num].Visible = true;

        }
    #endregion

    }






}
