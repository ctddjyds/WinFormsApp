using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsAppFor157Recommend
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
            m_backgroudWorker = new BackgroundWorker();
        }
        public AsynThreadTaskParallelClass AttpClass = new AsynThreadTaskParallelClass();
        private void buttonAPMAsyn_Click(object sender, EventArgs e)
        {
            var request = HttpWebRequest.Create("http://www.sina.com.cn");
            request.BeginGetResponse(AttpClass.AsyncCallBackImpl, request);
            this.textBoxPage.Text = AttpClass.CallStr;
        }
        //设置为false表示任何在它上面进行等待的线程都被阻塞
        private AutoResetEvent m_autoResetEvent = new AutoResetEvent(false);
        private ManualResetEvent m_manualResetEvent = new ManualResetEvent(false);
        private void buttonAutoReset_Click(object sender, EventArgs e)
        {
            StartThreadOne();
            //如果需要同时使两个线程都阻塞，直到接收到主线程的信号再继续工作，则需要使用ManualResetEvent
            StartThreadTwo();
        }
        private void StartThreadOne()
        {
            Thread twork = new Thread(() =>
            {
                this.labelAutoReset.Text = "线程启动......" + Environment.NewLine;
                this.labelAutoReset.Text += "开始阻塞，等待其它线程信号...." + Environment.NewLine;
                m_autoResetEvent.WaitOne();//设置为m_autoResetEvent上的等待（阻塞）线程
                this.labelAutoReset.Text += "接收到其它线程信号，结束线程！";
            });
            twork.IsBackground = true;
            twork.Start();
        }
        private void StartThreadTwo()
        {
            Thread twork = new Thread(() =>
            {
                this.labelManualReset.Text = "线程启动......" + Environment.NewLine;
                this.labelManualReset.Text += "开始阻塞，等待其它线程信号...." + Environment.NewLine;
                m_manualResetEvent.WaitOne();//设置为ResetEvent上的等待（阻塞）线程
                this.labelManualReset.Text += "接收到其它线程信号，结束线程！";
            });
            twork.IsBackground = true;
            twork.Priority = ThreadPriority.Highest;
            twork.Start();
        }
        
        private void buttonSet_Click(object sender, EventArgs e)
        {
            //给在AutoResetEvent上等待的线程一个信号,取消阻塞，继续执行
            m_autoResetEvent.Set();
            m_manualResetEvent.Set();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            m_manualResetEvent.Reset();
        }

        /// <summary>
        /// 当停止一个线程时，线程不会立即停止，线程只会在它觉得合适的时候才退出
        /// 协作式取消（Coorperative cancellation）类型：如果线程需要被停止，线程在工作的同时检测开放给调用者的接口（Cancled）,
        /// 如果检测到Cancled，线程自己才会负责退出；关键类型 CancellationTokenSource
        /// </summary>
        private void btnThreadAbort_Click(object sender, EventArgs e)
        {
            CoorperativeCancellation();
        }
        private CancellationTokenSource m_cts = new CancellationTokenSource();
        private void CoorperativeCancellation()
        {
            Thread twork = new Thread(() =>
            {
                while (true)
                {
                    //IsCancellationRequested属性作为需要取消工作的标识
                    if (m_cts.Token.IsCancellationRequested)
                    {
                        MessageBox.Show("线程被终止");
                        break;
                    }
                    //Register方法传递一个Action的委托， 在线程停止时被回调
                    //m_cts.Token.Register(() => { Console.WriteLine("线程被终止"); });
                }
                this.labelThreadAbort.Text = DateTime.Now.ToString();
                //Console.WriteLine(DateTime.Now.ToString());
                Thread.Sleep(1000);

            });
            //twork.IsBackground = true;
            twork.Start();
            this.labelThreadAbort.Text = "开始线程";
            //Console.ReadLine();
            m_cts.Cancel();//通知线程协作式取消工作
        }

        /// <summary>
        /// BackgroundWorker给工作线程和UI线程提供交互能力
        /// 报告进度、支持完成回调、取消任务、暂停任务等
        /// </summary>
        private BackgroundWorker m_backgroudWorker;
        private void btnBackgroudWorker_Click(object sender, EventArgs e)
        {
            m_backgroudWorker.DoWork += M_backgroudWorker_DoWork;
            m_backgroudWorker.ProgressChanged += M_backgroudWorker_ProgressChanged;
            m_backgroudWorker.RunWorkerAsync();
        }

        private void M_backgroudWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.labelBackGroudWorker.Text = e.ProgressPercentage.ToString();
        }

        private void M_backgroudWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker backWorker = sender as BackgroundWorker;
            for(int i=0;i<10;i++)
            {
                backWorker.ReportProgress(i);
                Thread.Sleep(100);
            }
        }       

        /// <summary>
        /// 委托协变和逆变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCovariant_Click(object sender, EventArgs e)
        {
            CovariantContravariantClass cv=new CovariantContravariantClass();
            cv.DelegateCovariant();
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            AttpClass.StartTaskToken(false);
        }

        private void btnTaskThrow_Click(object sender, EventArgs e)
        {
            AttpClass.StartTaskToken(true);
        }

        private void btnTaskFactory_Click(object sender, EventArgs e)
        {
            AttpClass.StartTaskFactory();
        }
        //用于表示主线程，在本例中就是UI线程
        Thread mainThread;
        private bool CheckAccess()
        {
            return mainThread == Thread.CurrentThread;
        }
        private void VerifyAccess()
        {
            if (!CheckAccess())
                throw new InvalidOperationException("调用线程无法访问此对象，因为另一个线程拥有此对象");
        }
        private void btnTaskAsync_Click(object sender, EventArgs e)
        {
            //当前线程就是主线程
            mainThread = Thread.CurrentThread;
            Task t = new Task(() =>
            {
                while (true)
                {
                    if (!CheckAccess())
                        this.labelTaskAsync.BeginInvoke(new Action(() =>
                        {
                            this.labelTaskAsync.Text = DateTime.Now.ToString();
                        }));
                    else
                        this.labelTaskAsync.Text = DateTime.Now.ToString();
                    Thread.Sleep(1000);
                }
            });
            //如果有异常，就启动一个新任务,没有将异常传递到主线程
            t.ContinueWith((task) =>
            {
                try
                {
                    task.Wait();
                }
                catch (AggregateException ex)
                {
                    foreach (Exception inner in ex.InnerExceptions)
                    {
                        MessageBox.Show(string.Format("异常类型：{0}{1}来自于：{2}{3}异常内容：{4}", inner.GetType(), Environment.NewLine, inner.Source, Environment.NewLine, inner.Message));
                    }
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            t.Start();
        }

        #region server

        //用于保存非对称加密（数字证书）的公钥
        string publicKey = string.Empty;
        //用于保存非对称加密（数字证书）的私钥
        string pfxKey = string.Empty;

        ///======================
        ///服务器端代码
        ///======================

        ///用于跟客户端通信的socket
        Socket serverCommunicateSocket;
        ///定义接受缓存块的大小
        static int serverBufferSize = 1024;
        ///缓存块
        byte[] bytesReceivedFromClient = new byte[serverBufferSize];
        ///密钥K
        string key = string.Empty;
        StringBuilder messageFromClient = new StringBuilder();
        private void buttonStartServer_Click(object sender, EventArgs e)
        {
            //先生成数字证书（模拟，即非对称密钥对）
            RSAKeyInit();
            //负责侦听
            StartListen();
        }
        private void RSAKeyInit()
        {
            RSAProcessor.CreateRSAKey(ref publicKey, ref pfxKey);
        }

        private void StartListen()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 8009);
            //负责侦听的socket
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(iep);
            listenSocket.Listen(50);
            listenSocket.BeginAccept(new AsyncCallback(this.Accepted), listenSocket);
            ListBoxServerShow("开始侦听。。。");
            buttonStartServer.Enabled = false;
        }

        ///负责客户端的连接，并开始将自己置于接收状态
        void Accepted(IAsyncResult result)
        {
            Socket listenSocket = result.AsyncState as Socket;
            //初始化和客户端进行通信的socket
            serverCommunicateSocket = listenSocket.EndAccept(result);
            ListBoxServerShow("有客户端连接到。。。");
            serverCommunicateSocket.BeginReceive(bytesReceivedFromClient, 0, serverBufferSize, SocketFlags.None, new AsyncCallback(this.ReceivedFromClient), null);
        }

        ///负责处理接受自客户端的数据
        void ReceivedFromClient(IAsyncResult result)
        {
            int read = serverCommunicateSocket.EndReceive(result);
            if (read > 0)
            {
                messageFromClient.Append(UTF32Encoding.Default.GetString(bytesReceivedFromClient, 0, read));
                //处理并显示数据
                ProcessAndShowInServer();
                serverCommunicateSocket.BeginReceive(bytesReceivedFromClient, 0, serverBufferSize, 0, new AsyncCallback(ReceivedFromClient), null);
            }
        }
        private RijndaelCrypt m_RijndaelCrypt = new RijndaelCrypt();
        private void ProcessAndShowInServer()
        {
            string msg = messageFromClient.ToString();
            //如果接收到<EOF>则表示完成完成一次，否则继续将自己置于接收状态
            if (msg.IndexOf("<EOF>") > -1)
            {
                //如果客户端发送key，则负责初始化key
                if (msg.IndexOf("<KEY>") > -1)
                {
                    //用私钥解密发送过来的Key信息
                    key = RSAProcessor.RSADecrypt(pfxKey, msg.Substring(0, msg.Length - 10));
                    ListBoxServerShow(string.Format("接收到客户端密钥：{0}", key));
                }
                else
                {
                    //解密SSL通道中发送过来的密文并显式
                    ListBoxServerShow(string.Format("接收到客户端消息：{0}", m_RijndaelCrypt.DencryptString(msg.Substring(0, msg.Length - 5), key)));
                }
                messageFromClient.Clear();
            }
        }
        ///负责向客户端发送数据
        private void buttonStartSendToClient_Click(object sender, EventArgs e)
        {
            //加密消息体
            string msg = string.Format("{0}{1}", m_RijndaelCrypt.EncryptString(DateTime.Now.ToString(), key), "<EOF>");
            m_RijndaelCrypt.DencryptString(msg.Substring(0, msg.Length - 5), key);
            byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
            serverCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
            ListBoxServerShow(string.Format("发送：{0}", msg));
        }
        private void ListBoxServerShow(string msg)
        {
            listBoxServer.BeginInvoke(new Action(() =>
            {
                listBoxServer.Items.Add(msg);
            }));
        }
        #endregion server

        #region client
        ///======================
        ///客户端代码
        ///======================

        ///用于跟服务器通信的socket
        Socket clientCommunicateSocket;
        ///用于暂存接收到的字符串
        StringBuilder messageFromServer = new StringBuilder();
        ///定义接受缓存块的大小
        static int clientBufferSize = 1024;
        ///缓存块
        byte[] bytesReceivedFromServer = new byte[clientBufferSize];
        //随机生成的key，在这里硬编码为key123
        string keyCreateRandom = "key123";

        private void buttonConnectAndReceiveMsg_Click(object sender, EventArgs e)
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.1.100"), 8009);
            Socket connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectSocket.BeginConnect(iep, new AsyncCallback(this.Connected), connectSocket);
            buttonConnectAndReceiveMsg.Enabled = false;
        }

        void Connected(IAsyncResult result)
        {
            clientCommunicateSocket = result.AsyncState as Socket;
            clientCommunicateSocket.EndConnect(result);
            clientCommunicateSocket.BeginReceive(bytesReceivedFromServer, 0, clientBufferSize, SocketFlags.None, new AsyncCallback(this.ReceivedFromServer), null);
            ListBoxClientShow("客户端连接上服务器。。。");
            //连接成功便发送密钥K给服务器
            SendKey();
        }

        void ReceivedFromServer(IAsyncResult result)
        {
            int read = clientCommunicateSocket.EndReceive(result);
            if (read > 0)
            {
                messageFromServer.Append(UTF32Encoding.Default.GetString(bytesReceivedFromServer, 0, read));
                //处理并显示客户端数据
                ProcessAndShowInClient();
                clientCommunicateSocket.BeginReceive(bytesReceivedFromServer, 0, clientBufferSize, 0, new AsyncCallback(ReceivedFromServer), null);
            }
        }

        private void ProcessAndShowInClient()
        {
            //如果接收到<EOF>则表示完成一次接收，否则继续将自己置于接收状态
            if (messageFromServer.ToString().IndexOf("<EOF>") > -1)
            {
                //解密消息体并呈现出来
                ListBoxClientShow(string.Format("接收到服务器消息：{0}", m_RijndaelCrypt.DencryptString(messageFromServer.ToString().Substring(0, messageFromServer.ToString().Length - 5), keyCreateRandom)));
                messageFromServer.Clear();
            }
        }

        private void buttonStartSendToServer_Click(object sender, EventArgs e)
        {
            //加密消息体
            string msg = string.Format("{0}{1}", m_RijndaelCrypt.EncryptString(DateTime.Now.ToString(), keyCreateRandom), "<EOF>");
            byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
            clientCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
            ListBoxClientShow(string.Format("发送：{0}", msg));
        }

        private void SendKey()
        {
            string msg = RSAProcessor.RSAEncrypt(publicKey, keyCreateRandom) + "<KEY><EOF>";
            byte[] msgBytes = UTF32Encoding.Default.GetBytes(msg);
            clientCommunicateSocket.BeginSend(msgBytes, 0, msgBytes.Length, SocketFlags.None, null, null);
            ListBoxClientShow(string.Format("发送：{0}", keyCreateRandom));
        }

        private void ListBoxClientShow(string msg)
        {
            listBoxClient.BeginInvoke(new Action(() =>
            {
                listBoxClient.Items.Add(msg);
            }));
        }
        #endregion client

        //private void buttonStartThreads_Click(object sender, EventArgs e)
        //{
        //    SampleClass sample1 = new SampleClass();
        //    SampleClass sample2 = new SampleClass();
        //    sample1.StartT1();
        //    sample2.StartT2();
        //}

        //class SampleClass
        //{
        //    public static List<string> TempList = new List<string>() { "init0", "init1", "init2" };
        //    static AutoResetEvent autoSet = new AutoResetEvent(false);
        //    object syncObj = new object();

        //    public void StartT1()
        //    {
        //        Thread t1 = new Thread(() =>
        //        {
        //            //确保等待t2开始之后才运行下面的代码
        //            autoSet.WaitOne();
        //            lock (syncObj)
        //            {
        //                foreach (var item in TempList)
        //                {
        //                    Thread.Sleep(1000);
        //                }
        //            }
        //        });
        //        t1.IsBackground = true;
        //        t1.Start();
        //    }

        //    public void StartT2()
        //    {
        //        Thread t2 = new Thread(() =>
        //        {
        //            //通知t1可以执行代码
        //            autoSet.Set();
        //            //沉睡1秒是为了确保删除操作在t1的迭代过程中
        //            Thread.Sleep(1000);
        //            lock (syncObj)
        //            {
        //                TempList.RemoveAt(1);
        //            }
        //        });
        //        t2.IsBackground = true;
        //        t2.Start();
        //    }
        //}
    }
}
