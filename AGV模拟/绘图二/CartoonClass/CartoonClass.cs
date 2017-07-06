using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using 绘图二.Comport;

namespace 绘图二
{
    public class CartoonClass
    {
        public static bool CartoonFlag = false;
        public static List<string> CartoonStr = new List<string>();

        public static TextBox PrevPoint, NextPoint, Interval;
        private static DateTime start, end;

        public static void CartoonTxt()
        {
            //Console.WriteLine(start);
            //Console.WriteLine(end);
            if (NextPoint.Text != Config.refCurrentPT[Config.CartoonChoose].Text)
            {
                string ListStr;

                PrevPoint.Text = NextPoint.Text;
                NextPoint.Text = Config.refCurrentPT[Config.CartoonChoose].Text;
                end = DateTime.Now;
                if (start.Year != 0001)
                {
                    Interval.Text = CalInterval(start, end).ToString();
                    ListStr = string.Format("起点：{0}-终点：{1}，用时：{2}。", PrevPoint.Text, NextPoint.Text, Interval.Text);
                    CartoonStr.Add(ListStr);
                }
                start = DateTime.Now;
            }
        }

        private static int CalInterval(DateTime Start, DateTime End)
        {
            int MSstart, MSend;
            MSstart = Start.Hour * 60 * 60 * 1000 + Start.Minute * 60 * 1000 + Start.Second * 1000 + Start.Millisecond;
            MSend = End.Hour * 60 * 60 * 1000 + End.Minute * 60 * 1000 + End.Second * 1000 + End.Millisecond;

            return (MSend - MSstart);
        }
    }
}
