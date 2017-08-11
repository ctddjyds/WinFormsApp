using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormsAppFor157Recommend
{
    public class AsynThreadTaskParallelClass
    {
        //IO操作的MDA（Direct memory access）模式：直接访问内存，是一种不经过CPU而直接进行内存数据存储的数据交换模式，几乎可以不损耗CPU的资源；
        //CLR所提供的异步编程模型就是充分利用硬件的DMA功能来释放CPU的压力；
        //使用线程池进行管理，异步将工作移交给线程池中的某个工作线程来完成，直到异步完成，异步才会通过回调的方式通知线程池，让CLR响应异步完毕； 
        //多线程编程遵循一个原则：类型的静态方法应当保证线程安全，非静态方法不需实现线程安全；      
        public string CallStr { get; set; }
        public void AsyncCallBackImpl(IAsyncResult ar)
        {
            WebRequest request = ar.AsyncState as WebRequest;
            var response = request.EndGetResponse(ar);
            var stream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
            {
                CallStr = reader.ReadLine();
            }
        }
        //线程同步：多个线程在某个对象上执行等待（锁定该对象），直到该对象被解除锁定。
        //CLR中值类型不能在其上执行等待（锁定），引用类型的等待机制分为两类：锁定和信号同步。
        //锁定使用关键字lock和类型Monitor，两者没有实质区别，前者其实是后者的语法糖
        //信号同步机制涉及到的类型都继承于抽象类WaitHandle；底层的原理都是维护一个系统的内核句柄
        //类型有：EventWaitHandle(子类AutoResetEvent和ManualResetEvent)、Semaphore、Mutex
        //EventWaitHandle维护一个由内核产生的布尔类型对象（"阻滞对象"），如果为false表示在它上面等待的线程就阻塞；调用Set方法将其值设置为true，解除阻塞；
        //AutoResetEvent和ManualResetEvent的区别：前者在发送完信号后（调用Set方法），会自动将自己的阻滞状态设置为false;而后者需要手动设置
        //Semaphore维护一个由内核产生的整形变量，如果为0表示在它上面等待的线程就阻塞，如果大于0则解除阻塞，同时每解除一个线程阻塞，其值会减1
        //Mutex提供跨应用程序域阻塞和解除阻塞线程的能力。
        //EventWaitHandle和Semaphore提供的都是单应用程序域内的线程同步功能。

        //使用同步对象注意项，避免锁定不适当的同步对象：
        //1.同步对象在需要同步的多个线程中是可见的同一个对象；
        //2.在非静态方法中，静态变量不能作为同步对象（类型的静态方法应该保证线程安全）；
        //3.值类型对象不能作为同步对象（值类型传递到一个线程时会创建一个副本）；
        //4.避免将字符串作为同步对象
        //5.降低同步对象的可见性

        //线程取消模式：协作式取消（Coorperative cancellation）类型：CancellationTokenSource

        //使用Task取代ThreadPool(ThreadPool不支持线程的取消、完成、失败通知等交互性操作)
        //Task属性： IsCanceled：因为被取消而完成 | IsCompleted：成功完成 | IsFaulted：因为发生异常而完成
        //ContinueWith方法可以在一个任务完成时候发起一个新任务,这种方式天然的支持了任务的完成通知：可以在新任务中获取原任务的结果值
        public  void StartTaskToken(bool isThrow)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> t = null;
            if (isThrow)
                t = new Task<int>(() => AddMethodByThrow(cts.Token), cts.Token);
            else
                t = new Task<int>(() => AddMethod(cts.Token), cts.Token);
            
            t.Start();
            if (isThrow)
                t.ContinueWith(TaskEndByCatch);
            else
                t.ContinueWith(TaskEnd);
            Thread.Sleep(3000);//等待3s,取消线程
            cts.Cancel();
        }
        private  int AddMethod(CancellationToken ct)
        {
            int result = 0;
            while (!ct.IsCancellationRequested)//等待线程取消
            {
                result++;
                Thread.Sleep(100);
            }
            return result;
        }
        private string TaskEnd(Task<int> task)
        {
            return string.Format("任务完成,任务状态： IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2} \r\n 任务完成后的值：{3}",
                task.IsCanceled,task.IsCompleted,task.IsFaulted,task.Result);
        }
        private int AddMethodByThrow(CancellationToken ct)
        {
            int result = 0;
            while (true)
            {
                ct.ThrowIfCancellationRequested();// 通过异常方式取消任务，不被看作一个异常（被理解为取消）
                result++;
                if (result == 5)
                {
                    //throw new Exception("模拟一个异常，可得到IsFaulted=true的状态");
                }
                Thread.Sleep(100);
            }
            return result;
        }
        private string TaskEndByCatch(Task<int> task)
        {
            try
            {
                return string.Format("任务完成时的状态： IsCanceled={0}\tIsCompleted={1}\tIsFaulted={2} \r\n 任务完成后的值：{3}",
                task.IsCanceled, task.IsCompleted, task.IsFaulted, task.Result);
            }
            catch(AggregateException e)
            {
                e.Handle((err) => err is OperationCanceledException);
                return "";
            }
            
        }
        
        //TaskFactory 任务工厂支持多个任务之间共享相同的状态
        public  void StartTaskFactory()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            TaskFactory taskFactory = new TaskFactory();
            Task[] taskArray = new Task[]
            {
                taskFactory.StartNew(()=>AddMethod(cts.Token)),
                taskFactory.StartNew(()=>AddMethod(cts.Token)),
                taskFactory.StartNew(()=>AddMethod(cts.Token))
            };
            //CancellationToken.None指示TasksEnded不能被取消
            taskFactory.ContinueWhenAll(taskArray, TaskArrayEnd, CancellationToken.None);
            Thread.Sleep(1000);
            cts.Cancel();
        }
        private string TaskArrayEnd(Task[] taskArray)
        {
            return "任务完成";
        }
        public void ParallelForMethod()
        {
            string[] stringArr = new string[] { "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh" };
            string result = string.Empty;
            Parallel.For<string>(0, stringArr.Length, () => "-", (i, loopState, subResult) =>
            {
                return subResult += stringArr[i];
            }, (threadEndString) =>
            {
                result += threadEndString;
                Console.WriteLine("Inner:" + threadEndString);
            });
            Console.WriteLine(result);
            Console.ReadKey();
        }
        //
        public void ParallelForExceptionMethod()
        {
            try
            {
                var parallelExceptions = new ConcurrentQueue<Exception>();
                Parallel.For(0, 1, (i) =>
                {
                    try
                    {
                        throw new InvalidOperationException("并行任务中出现的异常");
                    }
                    catch (Exception e)
                    {
                        parallelExceptions.Enqueue(e);
                    }
                    if (parallelExceptions.Count > 0)
                        throw new AggregateException(parallelExceptions);
                });
            }
            catch (AggregateException err)
            {
                foreach (Exception item in err.InnerExceptions)
                {
                    Console.WriteLine("异常类型：{0}{1}来自于：{2}{3}异常内容：{4}", item.InnerException.GetType(), Environment.NewLine, item.InnerException.Source, Environment.NewLine, item.InnerException.Message);
                }
            }
            Console.WriteLine("主线程马上结束");
            Console.ReadKey();
        }
        public void PLINQMethod()
        {
            List<int> intList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var query = from p in intList select p;
            Console.WriteLine("以下是LINQ顺序输出：");
            foreach (int item in query)
            {
                Console.WriteLine(item.ToString());
            }
            Console.WriteLine("以下是PLINQ并行输出：");
            var queryParallel = from p in intList.AsParallel() select p;
            foreach (int item in queryParallel)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }
}
