using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    public class LinqClass
    {
        /// <summary>
        /// 建议在查询中使用lambda表达式
        /// </summary>
        public void NewMethod()
        {
            DataContext ctx = new DataContext("server=192.168.0.102;database=Temp;uid=sa;pwd=sa123");
            Table<Person> persons = ctx.GetTable<Person>();
            foreach (var item in persons.Where(p => p.Age > 20))
            {
                Console.WriteLine(string.Format("Name:{0}\tAge:{1}", item.Name, item.Age));
            }
        }
        /// <summary>
        /// IEnumerable<T>查询
        /// </summary>
        public void NewMethod3()
        {
            DataContext ctx = new DataContext("server=192.168.0.102;database=Temp;uid=sa;pwd=sa123");
            Table<Person> persons = ctx.GetTable<Person>();
            var temp1 = (from p in persons where p.Age > 20 select p).AsEnumerable<Person>();//转化为本地存储
            var temp2 = from p in temp1 where p.Name.IndexOf('e') > 0 select p;
            foreach (var item in temp2)
            {
                Console.WriteLine(string.Format("Name:{0}\tAge:{1}", item.Name, item.Age));
            }
        }
        /// <summary>
        /// IQueryable<T>查询
        /// </summary>
        public void NewMethod2()
        {
            DataContext ctx = new DataContext("server=192.168.0.102;database=Temp;uid=sa;pwd=sa123");
            Table<Person> persons = ctx.GetTable<Person>();
            var temp1 = from p in persons where p.Age > 20 select p;
            var temp2 = from p in temp1 where p.Name.IndexOf('e') > 0 select p;
            foreach (var item in temp2)
            {
                Console.WriteLine(string.Format("Name:{0}\tAge:{1}", item.Name, item.Age));
            }
        }
        /// <summary>
        /// IEnumerable<T>查询逻辑可以直接用我们自定义的方法;
        /// 而IQueryable<T>则不能使用自定义的方法，必须先生成表达式树；
        /// </summary>
        public void NewMethod1()
        {
            DataContext ctx = new DataContext("server=192.168.0.102;database=Temp;uid=sa;pwd=sa123");
            Table<Person> persons = ctx.GetTable<Person>();
            var temp1 = from p in persons where OlderThan20(p.Age) select p;//不能使用自定义的方法，会报异常
            List<int> testEnumerableList = new List<int> { 20, 12, 34, 44, 55 };
            var temp2 = from p in testEnumerableList where OlderThan20(p) select p;
            foreach (var item in temp1)
            {
                Console.WriteLine(string.Format("Name:{0}\tAge:{1}", item.Name, item.Age));
            }
        }
        private bool OlderThan20(int age)
        {
            if (age > 20)
                return true;
            else
                return false;
        }

        [Table(Name = "Person")]
        class Person
        {
            [Column]
            public string Name { get; set; }
            [Column]
            public int Age { get; set; }
        }
        /// <summary>
        /// Linq简化了排序编码（Tip10）
        /// </summary>
        public void UseLinqToSort()
        {
            List<Salary> companySalary = new List<Salary>()
                {
                    new Salary() { Name = "Mike", BaseSalary = 3000, Bonus = 1000 },
                    new Salary() { Name = "Rose", BaseSalary = 2000, Bonus = 4000 },
                    new Salary() { Name = "Jeffry", BaseSalary = 1000, Bonus = 6000 },
                    new Salary() { Name = "Steve", BaseSalary = 4000, Bonus = 3000 }
                };
            Console.WriteLine("默认排序：");
            foreach (Salary item in companySalary)
            {
                Console.WriteLine(string.Format("Name:{0} \tBaseSalary:{1} \tBonus:{2}", item.Name, item.BaseSalary, item.Bonus));
            }
            Console.WriteLine("BaseSalary排序：");
            var orderByBaseSalary = from s in companySalary orderby s.BaseSalary select s;
            foreach (Salary item in orderByBaseSalary)
            {
                Console.WriteLine(string.Format("Name:{0} \tBaseSalary:{1} \tBonus:{2}", item.Name, item.BaseSalary, item.Bonus));
            }
            Console.WriteLine("Bonus排序：");
            var orderByBonus = from s in companySalary orderby s.Bonus select s;
            foreach (Salary item in orderByBonus)
            {
                Console.WriteLine(string.Format("Name:{0} \tBaseSalary:{1} \tBonus:{2}", item.Name, item.BaseSalary, item.Bonus));
            }
        }

        class Salary
        {
            public string Name { get; set; }
            public int BaseSalary { get; set; }
            public int Bonus { get; set; }
        }
    }
}
