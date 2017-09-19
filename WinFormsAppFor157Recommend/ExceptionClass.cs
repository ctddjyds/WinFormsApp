using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Tip61
{
    /// <summary>
    /// 类库异常使用原则：
    /// 正常的业务流程不应使用异常来处理；
    /// 不要总是尝试去捕获异常或引发异常，而应允许异常向调用堆栈往上传播；不要过多的使用catch，然后再throw(会造成隐藏了堆栈信息，不知道真正发生异常的地方).
    /// </summary>
    public class ExceptionClass
    {
        public void SaveUser(User user)
        {
            if (user.Age < 0)
            {
                throw new ArgumentOutOfRangeException("Age不能为负数。");
            }
        }
        /// <summary>
        /// 重新引发异常时使用InnerException
        /// </summary>
        /// <returns></returns>
        public User TestInnerException()
        {
            try
            {
                User user = new User();
                this.SaveUser(user);
                return user;
            }
            catch(Exception ex)
            {
                throw new Exception("测试Inner Exception.", ex);
            }
            return null;
        }

        public int TestIntReturnBelowFinally()
        {
            int i;
            try
            {
                i = 1;
            }
            finally
            {
                i = 2;
                Console.WriteLine("\t将int结果改为2，finally执行完毕");
            }
            return i;
        }

        public int TestIntReturnInTry()
        {
            int i;
            try
            {
                return i = 1;
            }
            finally
            {
                i = 2;
                Console.WriteLine("\t将int结果改为2，finally执行完毕");
            }
        }

        public User TestUserReturnInTry()
        {
            User user = new User() { Name = "Mike", BirthDay = new DateTime(2010, 1, 1) };
            try
            {
                return user;
            }
            finally
            {
                user.Name = "Rose";
                user.BirthDay = new DateTime(2010, 2, 2);
                Console.WriteLine("\t将user.Name改为Rose");
            }
        }

        public User TestUserReturnInTry2()
        {
            User user = new User() { Name = "Mike", BirthDay = new DateTime(2010, 1, 1) };
            try
            {
                return user;
            }
            finally
            {
                user.Name = "Rose";
                user.BirthDay = new DateTime(2010, 2, 2);
                user = null;
                Console.WriteLine("\t将user置为anull");
            }
        } 
        public void UserTesterDoerParttern()
        {
            Stopwatch watch = Stopwatch.StartNew();
            int x = 0;
            for (int i = 0; i < 10000; i++)
            {
                try
                {
                    int j = i / x;
                }
                catch
                {
                }
            }
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());

            //Sometimes performance of an exception-throwing member can be improved by breaking the member into two.
            //Tester-Doer模式:函数中写入异常，会降低性能，微软给出了这种模式来减小异常带来的副作用,替代抛异常的优化方式，起到优化设计性能的作用。
            watch = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                if (x == 0)//Tester
                {
                    continue;//Doer
                }
                int j = i / x;
            }
            Console.WriteLine(watch.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }  
        public void TestPaperEncryptException()
        {
            try
            {
                throw new PaperEncryptException("加密试卷失败", "学生ID：123456");
            }
            catch (PaperEncryptException err)
            {

                Console.WriteLine(err.Message);
            }
        }
    }
    public class User
    {
        public int Age { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }
    }
    [global::System.Serializable]
    public class PaperEncryptException : Exception, ISerializable
    {
        private readonly string _paperInfo;
        public PaperEncryptException() { }
        public PaperEncryptException(string message) : base(message) { }
        public PaperEncryptException(string message, Exception inner) : base(message, inner) { }
        public PaperEncryptException(string message, string paperInfo)
            : base(message)
        {
            _paperInfo = paperInfo;
        }
        public PaperEncryptException(string message, string paperInfo, Exception inner)
            : base(message, inner)
        {
            _paperInfo = paperInfo;
        }
        protected PaperEncryptException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public override string Message
        {
            get
            {
                return base.Message + " " + _paperInfo;
            }
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Args", _paperInfo);
            base.GetObjectData(info, context);
        }
    }
}
