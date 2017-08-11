using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// 静态类添加静态构造函数：可以初始化静态成员并捕获在这过程中发生的异常
    /// </summary>
    public static class SampleClass
    {
        static FileStream fileStream;
        /// <summary>
        /// 只被执行一次，且在第一次调用类成员之前被运行时执行
        /// 代码无法调用该构造函数（new）
        /// 没有访问标识符
        /// 不能带任何参数
        /// </summary>
        static SampleClass()
        {
            try
            {
                fileStream = File.Open(@"c:\temp.txt", FileMode.Open);
            }
            catch (FileNotFoundException err)
            {
                Console.WriteLine(err.Message);
                //处理异常
            }
        }

        public static void SampleMethod()
        { }
    }
}
