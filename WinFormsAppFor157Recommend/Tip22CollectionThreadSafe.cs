using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WinFormsAppFor157Recommend
{
    public class Tip22CollectionThreadSafe
    {
        public ArrayList ArrayListSyncRoot = new ArrayList()
        {
            new Person() {Name="Rose",Age=19 },
            new Person() {Name="Mike",Age=40 },
            new Person() {Name="steve",Age=25 }
        };
        public List<Person> GenericListLock = new List<Person>()
        {
            new Person() {Name="Rose",Age=19 },
            new Person() {Name="Mike",Age=40 },
            new Person() {Name="steve",Age=25 }
        };
        private AutoResetEvent m_autoResetEvent = new AutoResetEvent(false);
        /// <summary>
        /// 非泛型集合的线程安全通过锁定（lock）非泛型集合的SyncRoot属性来实现
        /// </summary>
        public void SyncRootThreadSafe()
        {
            m_autoResetEvent.Reset();
            Thread thread1=new Thread(()=>
            {
                //确保等待thread2开始之后才运行下面的代码
                m_autoResetEvent.WaitOne();
                lock(ArrayListSyncRoot.SyncRoot)
                {
                    foreach(Person person in ArrayListSyncRoot)
                    {
                        Console.WriteLine("thread1:" + person.Name);
                        Thread.Sleep(1000);
                    }
                }
            }
            );
            thread1.Start();
            Thread thread2 = new Thread(() =>
              {
                  //通知thread1可以执行代码
                  m_autoResetEvent.Set();
                  //沉睡1s确保删除操作是在集合迭代过程中
                  Thread.Sleep(1000);
                  //锁定通过集合的互斥的机制保证了在同一时刻只有一个线程操作集合元素
                  lock (ArrayListSyncRoot.SyncRoot)
                  {
                      ArrayListSyncRoot.Remove(2);
                  }
              });
            thread2.Start();
        }
        private static readonly object m_objectLock = new object();
        /// <summary>
        /// 泛型通过锁定一个静态对象保证了在同一时刻只有一个线程操作集合元素
        /// </summary>
        public void GenericLockThreadSafe()
        {
            m_autoResetEvent.Reset();
            Thread thread1 = new Thread(() =>
            {
                m_autoResetEvent.WaitOne();
                lock (m_objectLock)
                {
                    foreach (Person person in GenericListLock)
                    {
                        Console.WriteLine("t1:" + person.Name);
                        Thread.Sleep(1000);
                    }
                }
            }
            );
            thread1.Start();
            Thread thread2 = new Thread(() =>
            {
                m_autoResetEvent.Set();
                //沉睡1s确保删除操作是在集合迭代过程中
                Thread.Sleep(1000);
                lock (m_objectLock)
                {
                    GenericListLock.RemoveAt(2);
                    Console.WriteLine("删除成功");
                }
            });
            thread2.Start();
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
