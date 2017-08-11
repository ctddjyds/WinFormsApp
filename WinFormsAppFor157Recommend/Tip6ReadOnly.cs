using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    /// <summary>
    /// const是一个编译期常量（天然就是static的，不能手动添加static修饰符），readOnly是运行时常量
    /// const只能修饰基元类型、枚举类型或字符串类型，readOnly则没有限制
    /// const效率高，经过编译后，代码中引用const常量的地方会用const变量所对应的实际值来代替
    /// </summary>
    public class Tip6ReadOnly
    {
        public Tip6ReadOnly()
        {
            Sample sample = new Sample(200);
            //sample.ReadOnlyValue = 300;         //无法对只读的字段赋值(构造函数或变量初始值指定项中除外)
            Sample2 sample2 = new Sample2(new Student() { Age = 10 });
            //sample2.ReadOnlyValue = new Student() { Age = 20 };     //无法对只读的字段赋值(构造函数或变量初始值指定项中除外)
            sample2.ReadOnlyValue.Age = 20;//引用所指的实例的值可以改变；
        }

        class Sample
        {
            public readonly int ReadOnlyValue;

            public Sample(int value)
            {
                ReadOnlyValue = value;
            }
        }

        class Sample2
        {
            public readonly Student ReadOnlyValue;

            public Sample2(Student value)
            {
                ReadOnlyValue = value;
            }
        }
        class Student
        {
            public int Age { get; set; }
        }

    }
}
