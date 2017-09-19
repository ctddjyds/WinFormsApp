using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace WinFormsAppFor157Recommend
{
    public class Tip16Collections
    {
        public static void ArrayTest()
        {
            //NewMethod();
            //int[] iArr = { 0, 1, 2, 3, 4, 5, 6 };
            //iArr = (int[])iArr.ReSize(10);
            ResizeArray();
            ResizeList();

        }

        private static void NewMethod()
        {
            int[] iArr = { 0, 1, 2, 3, 4, 5, 6 };
            ArrayList arrayListInt = ArrayList.Adapter(iArr);   //将数组转变为ArrayList
            arrayListInt.Add(7);
            List<int> listInt = iArr.ToList<int>();             //将数组转变为List<T>
            listInt.Add(7);
        }

        private static void ResizeArray()
        {
            int[] iArr = { 0, 1, 2, 3, 4, 5, 6 };
            Stopwatch watch = new Stopwatch();
            watch.Start();
            iArr = (int[])iArr.ReSize(10);
            watch.Stop();
            Console.WriteLine("ResizeArray: " + watch.Elapsed);
        }

        private static void ResizeList()
        {
            List<int> iArr = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6 });
            Stopwatch watch = new Stopwatch();
            watch.Start();
            iArr.Add(0);
            iArr.Add(0);
            iArr.Add(0);
            watch.Stop();
            Console.WriteLine("ResizeList: " + watch.Elapsed);
        }
        public static void  TestList()
        {
            Employees1 employees1 = new Employees1()
            {
                new Employee(){ Name = "Mike" },
                new Employee(){ Name = "Rose" }
            };
            IList<Employee> employees = employees1;
            employees.Add(new Employee() { Name = "Steve" });
            foreach (var item in employees1)
            {
                Console.WriteLine(item.Name);
            }
        }
        public static void TestCollection()
        {
            Employees2 employees2 = new Employees2()
            {
                new Employee(){ Name = "Mike" },
                new Employee(){ Name = "Rose" }
            };
            ICollection<Employee> employees = employees2;
            employees.Add(new Employee() { Name = "Steve" });
            foreach (var item in employees2)
            {
                Console.WriteLine(item.Name);
            }
        }
        class Employee
        {
            public string Name { get; set; }
        }

        class Employees1 : List<Employee>
        {
            public new void Add(Employee item)
            {
                item.Name += " Changed!";
                base.Add(item);
            }
        }
        class Employees2 : IEnumerable<Employee>, ICollection<Employee>
        {
            List<Employee> items = new List<Employee>();

            public int Count
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            #region IEnumerable<Employee> 成员

            public IEnumerator<Employee> GetEnumerator()
            {
                return items.GetEnumerator();
            }

            #endregion

            #region ICollection<Employee> 成员

            public void Add(Employee item)
            {
                item.Name += " Changed!";
                items.Add(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(Employee item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Employee[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(Employee item)
            {
                throw new NotImplementedException();
            }

            //省略

            #endregion
        }
        class Student
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        class StudentTeamA
        {
            public List<Student> Students { get; set; }
        }
        static List<Student> listStudent = new List<Student>()
            {
                new Student(){ Name = "Mike", Age = 1},
                new Student(){ Name = "Rose", Age = 2}
            };
        public static void TestThread()
        {
            StudentTeamA teamA = new StudentTeamA();
            Thread t1 = new Thread(() =>
            {
                teamA.Students = listStudent;
                Thread.Sleep(3000);
                Console.WriteLine(listStudent.Count); //模拟对
                //集合属性进行一些运算
            });
            t1.Start();
            Thread t2 = new Thread(() =>
            {
                listStudent = null;   //模拟在别的地方对list1而
                //不是属性本身赋值为null
            });
            t2.Start();
        }
    }
    
    public static class ClassForExtensions
    {
        public static Array ReSize(this Array array, int newSize)
        {
            Type t = array.GetType().GetElementType();
            Array newArray = Array.CreateInstance(t, newSize);
            Array.Copy(array, 0, newArray, 0, Math.Min(array.Length, newSize));
            return newArray;
        }
    }
}
