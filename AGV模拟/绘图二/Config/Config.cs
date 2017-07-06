using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using 绘图二.AGVLable;
using System.Threading;
using System.Windows.Forms;
using 绘图二.Path;
using System.Drawing;

namespace 绘图二
{
    public static class Config
    {
        public static AutoResetEvent autoEvent = new AutoResetEvent(false);

        public static bool ReadFile = false;
        public static bool TrafficFlag = true; 

        public static bool[] AGVMoveFlag = new bool[Config.AGVNum + 1];
        public static bool[] ReadOnline = new bool[AGVNum + 1];
        public static bool[] PathTurnRight = new bool[Config.AGVNum + 1];

        public static Bitmap[] AGVBitmapR = new Bitmap[Config.AGVNum + 1];
        public static Bitmap[] AGVBitmapG = new Bitmap[Config.AGVNum + 1];

        public static Dictionary<int, Station> Station = new Dictionary<int, Station>();
        public static Dictionary<int, Path1> PathList = new Dictionary<int, Path1>();

        public const int AGVtime = 10;//小车移动默认间隔时间（通过pathTime()方法进行微调）
        public const int AGVNum = 13;//最大的小车数量
        public const int InitX = 350;//路劲起始点X
        public const int InitY = 100;//路劲起始点Y
        public const int MapInitX = 10;//画布左上角坐标X
        public const int MapInitY = 28;//画布左上角坐标Y
        public static int TestChoose;
        public static int CartoonChoose;
        public static int ConsoleChoose;
        public static int AGVIndex;
        public static int i = -1;             ///记录增加点击按钮的次数  
        public static int j = 0;              ///记录增加点击按钮的次数                                    
        public static int AGVShow1;           ///
        public static int AGVMov1;
        public static int[] ReadComNum = new int[AGVNum + 1];

        public static Label[] refCurrentPT = new Label[Config.AGVNum + 1];

        public static List<int> AGVStar1 = new List<int>();
        public static List<int> AGVNum1 = new List<int>();
        public static List<int> AGVPath1 = new List<int>();
        public static List<int> AGVTar1 = new List<int>();
        public static List<int> AGVLoct1 = new List<int>();
        public static List<string> DebugList = new List<string>();

        public static PictureBox[] StationPic = new PictureBox[100];
        public static PictureBox[] AGVLabel = new PictureBox[Config.AGVNum + 1];

        public const string password = "68686868";
        public const string Version = "1.0.1";//版本号
        public static String Equal="false";

        public static Thread[] ThreadAGVMove = new Thread[Config.AGVNum + 1];

       
    }
}
