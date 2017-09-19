using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace WinFormsAppFor157Recommend
{
    public class Tip2TypeConversion
    {
        public static void COnversionTest()
        {
            Ip ip = "192.168.0.96";
            Console.WriteLine(ip.ToString());

            Animal animal;
            Dog dog = new Dog();
            animal = dog;       //隐式转化，因为Dog就是Animal。
            //dog = animal;     //编译不通过
            dog = (Dog)animal;  //必须存在一个显式转换
            FirstType firstType = new FirstType() { Name = "First Type" };
            SecondType secondType = (SecondType)firstType;         //转型成功
            //secondType = firstType as SecondType;     //编译期转型失败，编译不通过
        }
    }

    class Ip
    {
        IPAddress value;

        public Ip(string ip)
        {
            value = IPAddress.Parse(ip);
        }
        public static implicit operator Ip(string ip)
        {
            Ip iptemp = new Ip(ip);
            return iptemp;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }

    class Animal
    {

    }

    class Dog : Animal
    {

    }

    class Cat : Animal
    {

    }
    public class FirstType
    {
        public string Name { get; set; }
    }
    /// <summary>
    /// implicit关键字用于声明隐式的用户定义类型转换运算符(explicit反之)
    /// static implicit operator target_type(source_type identifier){}
    /// 隐式转换可以通过消除不必要的类型转换来提高源代码的可读性。但是，因为可以在未指定的情况下发生隐式转换，因此必须注意防止令人不愉快的后果。
    /// 一般情况下，隐式转换运算符应当从不引发异常并且从不丢失信息，以便可以在不知晓的情况下安全使用它们。如果转换运算符不能满足那些条件，则应将其标记为 explicit。 
    /// </summary>
    public class SecondType
    {
        public string Name { get; set; }
        /// <summary>
        ///  //由一个FirstType显式返回一个SecondType
        /// </summary>
        /// <param name="firstType"></param>
        public static explicit operator SecondType(FirstType firstType)
        {
            SecondType secondType = new SecondType() { Name = "转型自：" + firstType.Name };
            return secondType;
        }
    }
}
