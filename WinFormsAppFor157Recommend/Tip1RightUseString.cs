using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// 确保尽量少的装箱
    /// 避免分配额外的内存空间
    /// 建议使用stringbuilder和format对字符串进行拼接
    /// </summary>
    public class RightUseString
    {
        public static void NewMethod1()
        {
            string s1 = "abc";
            s1 = "123" + s1 + "456"; 
            //以上两行代码创建了3个字符串对象，并执行了一次string.Contact方法
        }

        public static void NewMethod6()
        {
            string re6 = 9 + "456";     //该代码发生一次装箱，并调用一次string.Contact方法
        }

        public static void NewMethod2()
        {
            string re2 = "123" + "abc" + "456"; // 不会在运行时拼接，在编译时直接生成一个字符串
            //该代码等效于string re2 = "123abc456";
        }

        public static void NewMethod9()
        {
            const string a = "t";
            string re1 = "abc" + a;
            //因为a是一个常量，所以该代码等效于 string re1 = "abc" + "t"; 
            //最终等效于string re1 = "abct";
        }

        public static void NewMethod8()
        {
            //效率不高，三次Contact
            string a = "t";
            a += "e";
            a += "s";
            a += "t";
        }

        public static void NewMethod7()
        {
            string a = "t";
            string b = "e";
            string c = "s";
            string d = "t";
            string result = a + b + c + d;
        }

        public static void NewMethod10()
        {
            //为了演示必要，定义了4个变量
            string a = "t";
            string b = "e";
            string c = "s";
            string d = "t";
            StringBuilder sb = new StringBuilder(a);
            sb.Append(b);
            sb.Append(c);
            sb.Append(d);
            //再次提示，是运行时，所以没有使用下面的代码
            //StringBuilder sb = new StringBuilder("t");
            //sb.Append("e");
            //sb.Append("s");
            //sb.Append("t");
            string result = sb.ToString();
        }

        public static void NewMethod11()
        {
            //为了演示必要，定义了4个变量
            string a = "t";
            string b = "e";
            string c = "s";
            string d = "t";
            string.Format("{0}{1}{2}{3}", a, b, c, d);
        }


    }
}
