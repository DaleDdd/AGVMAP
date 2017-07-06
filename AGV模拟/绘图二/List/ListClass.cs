using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 绘图二.List
{
   public static  class ListClass
    {
        /*AGV命令集（控制面板）*/
        private static List<byte[]>[] Orders = new List<byte[]>[Config.AGVNum + 1] 
                            { new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), 
                            new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), 
                            new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(),
                            new List<byte[]>(), new List<byte[]>() };
        /*AGV命令集（交通管制）*/
        private static List<byte[]>[] TrafficOrders = new List<byte[]>[Config.AGVNum + 1] 
                            { new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), 
                            new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), 
                            new List<byte[]>(), new List<byte[]>(), new List<byte[]>(), new List<byte[]>(),
                            new List<byte[]>(), new List<byte[]>() };
        /*路径更新命令集*/
        private static List<int[]>[] PathOrders = new List<int[]>[Config.AGVNum + 1] 
                            { new List<int[]>(), new List<int[]>(), new List<int[]>(), new List<int[]>(), 
                            new List<int[]>(), new List<int[]>(), new List<int[]>(), new List<int[]>(), 
                            new List<int[]>(), new List<int[]>(), new List<int[]>(), new List<int[]>(),
                            new List<int[]>(), new List<int[]>() };

        /// <summary>
        /// 读取AGV命令集（控制面板）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte[] ListCol(int index)
        {
            byte[] buff;

            if (Orders[index].Count != 0)
            {
                buff = Orders[index][0];
                Orders[index].RemoveAt(0);
            }
            else
            {
                buff = null;
            }
            return buff;
        }

        /// <summary>
        /// 添加AGV命令集（控制面板）
        /// </summary>
        /// <param name="index"></param>
        /// <param name="buff"></param>
        public static void ListCol(int index, byte[] buff)
        {
            if (buff.Length == 14)
            {
                Orders[index].Add(buff);
            }
            else
            {
                Console.WriteLine("添加AGV命令集（控制面板）：" + index + "，添加出错，命令长度不对！");
            }
        }

        /// <summary>
        /// 读取AGV命令集（交通管制）
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static byte[] ListTraCol(int index)
        {
            byte[] buff;

            if (TrafficOrders[index].Count != 0)
            {
                buff = TrafficOrders[index][0];
                TrafficOrders[index].RemoveAt(0);
            }
            else
            {
                buff = null;
            }
            return buff;
        }

        /// <summary>
        /// 添加AGV命令集（交通管制）
        /// </summary>
        /// <param name="index"></param>
        /// <param name="buff"></param>
        public static void ListTraCol(int index, byte[] buff)
        {
            if (buff.Length == 14)
            {
                TrafficOrders[index].Add(buff);
            }
            else
            {
                Console.WriteLine("添加AGV命令集（交通管制）：" + index + "，添加出错，命令长度不对！");
            }
        }

        /// <summary>
        /// 读取路径更新命令集
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int[] ListPathCol(int index)
        {
            int[] buff;
            if (PathOrders[index].Count != 0)
            {
                buff = PathOrders[index][0];
                PathOrders[index].RemoveAt(0);
                //Console.WriteLine("命令读出：{0},{1}", buff[0], buff[1]);
            }
            else
            {
                buff = null;
            }
            return buff;
        }

        /// <summary>
        /// 添加路径更新命令集
        /// </summary>
        /// <param name="index"></param>
        /// <param name="buff"></param>
        public static void ListPathCol(int index, int[] buff)
        {
            PathOrders[index].Add(buff);
        }
    }
}
