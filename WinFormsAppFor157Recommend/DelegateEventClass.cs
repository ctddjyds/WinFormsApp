using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormsAppFor157Recommend
{
    delegate int AddHandler(int i,int j);
    delegate void PrintHandler(string msg);
    public class DelegateEventClass
    {
        public int Add(int i,int j)
        {
            return i + j;
        }
        public void PrintString(string msg)
        {
            Console.WriteLine(msg);
        }
        /// <summary>
        /// 使用基本的委托定义
        /// </summary>
        public void NoUseFCL()
        {
            AddHandler add = Add;
            PrintHandler print = PrintString;
            print(add(12, 13).ToString());
        }
        /// <summary>
        /// 使用自带的委托
        /// </summary>
        public void UseFCL()
        {
            Func<int, int, int> add = Add;//new Func<int, int, int>(Add);
            Action<String> print = PrintString;
            print(add(12, 13).ToString());
        }
        /// <summary>
        /// 使用匿名方法
        /// </summary>
        public void UseAnonymousMethods()
        {
            //Func<int, int, int> add = new Func<int, int, int>(delegate(int i, int j)
            //{
            //    return i + j;
            //});
            //Action<string> print = new Action<string>(delegate(string msg)
            //{
            //    Console.WriteLine(msg);
            //});
            //print(add(1, 2).ToString()); 
            Func<int, int, int> add = delegate (int i, int j)
            {
                return i + j;
            };
            Action<String> print = delegate (string msg)
            {
                Console.WriteLine(msg);
            };
            print(add(12, 13).ToString());
        }
        /// <summary>
        /// 使用lambda表达式
        /// </summary>
        public void UseLambda()
        {
            Func<int, int, int> add =(int i, int j)=>
            {
                return i + j;
            };
            Action<String> print = (string msg)=>
            {
                Console.WriteLine(msg);
            };
            print(add(12, 13).ToString());
        }
        public string FindListName(string name)
        {
            List<string> listTest = new List<string> { "ee","dt","dg","dgfe"};
            //第一种写法
            //return listTest.Find(new Predicate<Student>(delegate(Student target)
            //{
            //    if (target == name)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}));
            //第二种写法
            //return listTest.Find(new Predicate<Student>((target) =>
            //    {
            //        if (target == name)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }));
            //第三种写法
            //return listTest.Find((target) =>
            //    {
            //        if (target == name)
            //        {
            //            return true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    });
            //第四种写法
            return listTest.Find( target => target == name);
        }

        /// <summary>
        /// 闭包对象：如果匿名方法（lambda表达式）引用了某个局部变量，具有数据共享和延迟的特性
        /// 编译器会自动将该局部变量添加到闭包对象（ClosureObject）中作为公共变量使用
        /// </summary>
        public void UseClosureObject()
        {
            List<Action> lists = new List<Action>();
            for(int i=0;i<5; i++)
            {
                Action t = () =>
                  {
                      Console.WriteLine(i.ToString());
                  };
                lists.Add(t);
            }
            /* 等价于以上的Lambda表达式（匿名方法）
            ClosureObject closureObj = new ClosureObject();
            for(closureObj.i=0;closureObj.i<5;closureObj.i++)
            {
                Action t = closureObj.ClosureFunc;
                lists.Add(t);
            }
            */
            foreach (Action act in lists)
            { act(); }
        }
        class ClosureObject
        {
            public int i;
            public void ClosureFunc()
            {
                Console.WriteLine(i.ToString());
            }
        }

        public void UseStandardEventModel()
        {
            FileUploader fl = new FileUploader();
            fl.FileUploaded += Progress;
            fl.Upload();
        }
        private void Progress(object sender, FileUploadedEventArgs e)
        {
            Console.WriteLine(e.FileProgress);
        }
        class FileUploader
        {
            /// <summary>
            /// event关键字为委托施加保护
            /// EventHandler是标准的委托事件模型
            /// </summary>
            public event EventHandler<FileUploadedEventArgs> FileUploaded;
            public void Upload()
            {
                FileUploadedEventArgs e = new FileUploadedEventArgs() { FileProgress = 100 };

                while (e.FileProgress > 0)
                {
                    //传输代码，省略
                    e.FileProgress--;
                    if (FileUploaded != null)
                    {
                        FileUploaded(this, e);
                    }
                }
            }
        }
        class FileUploadedEventArgs : EventArgs
        {
            public int FileProgress { get; set; }
        }
    }
}
