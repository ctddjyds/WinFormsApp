using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    public class Tip12HashCode
    {
        private Dictionary<Person, PersonMoreInfo> m_PersonValuesDic = new Dictionary<Person, PersonMoreInfo>();
        public void UseEqual()
        {
            AddAPerson();
            Person mike = new Person("NB123");
            //Console.WriteLine(mike.GetHashCode());
            Console.WriteLine(m_PersonValuesDic.ContainsKey(mike));
        }

        private void AddAPerson()
        {
            Person mike = new Person("NB123");
            PersonMoreInfo mikeValue = new PersonMoreInfo() { SomeInfo = "Mike's info" };
            m_PersonValuesDic.Add(mike, mikeValue);
            //Console.WriteLine(mike.GetHashCode());
            Console.WriteLine(m_PersonValuesDic.ContainsKey(mike));
        }
        /// <summary>
        /// 重写Equals方法的同时，也应该实现一个类型安全的的接口IEquatable<T>
        /// 不建议使用值类型对象的GetHashCode函数返回值来作为HashTable对象的Key；
        /// 不管是值类型还是引用类型，要保证产生HashCode的成员不能被修改；
        /// </summary>
        class Person : IEquatable<Person>
        {
            public string IDCode { get; private set; }

            public Person(string idCode)
            {
                this.IDCode = idCode;
            }

            public override bool Equals(object obj)
            {
                return IDCode == (obj as Person).IDCode;
            }

            public override int GetHashCode()
            {
                //减少不同字符串产生相同的HashCode的几率
                return (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "#" + this.IDCode).GetHashCode();
            }

            public bool Equals(Person other)
            {
                return IDCode == other.IDCode;
            }
        }

        class PersonMoreInfo
        {
            public string SomeInfo { get; set; }
        }

    }
}
