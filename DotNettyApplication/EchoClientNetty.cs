using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Tls;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyApplication
{
    public class EchoClientNetty
    {
        bool IsSsl = false;
        private char m_OpenCode = (char)2;
        private char m_CloseCode = (char)3;
        public async Task RunClientAsync()
        {

            var group = new MultithreadEventLoopGroup();//

            X509Certificate2 cert = null;
            string targetHost = null;
            if (IsSsl)
            {
                cert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dotnetty.com.pfx"), "password");
                targetHost = cert.GetNameInfo(X509NameType.DnsName, false);
            }
            try
            {
                var bootstrap = new Bootstrap();
                bootstrap
                    .Group(group)
                    .Channel<TcpSocketChannel>()
                    .Option(ChannelOption.TcpNodelay, true)
                    .Handler(new ActionChannelInitializer<ISocketChannel>(channel =>
                    {
                        IChannelPipeline pipeline = channel.Pipeline;

                        if (cert != null)
                        {
                            pipeline.AddLast("tls", new TlsHandler(stream => new SslStream(stream, true, (sender, certificate, chain, errors) => true), new ClientTlsSettings(targetHost)));
                        }
                        pipeline.AddLast(new LoggingHandler());
                        pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                        pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));

                        pipeline.AddLast("echo", new EchoClientHandler());
                    }));

                IChannel clientChannel = await bootstrap.ConnectAsync(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2112));

                // 建立死循环，类同于While(true)
                //for (;;)
                {
                    //Console.WriteLine("input you data:");
                    // 根据设置建立缓存区大小
                    IByteBuffer initialMessage = Unpooled.Buffer(1024); // （1）
                    string r = string.Format("{0}", m_OpenCode);
                    // 将数据流写入缓冲区
                    if (r == null)
                        throw new InvalidOperationException();
                    initialMessage.WriteBytes(Encoding.UTF8.GetBytes(r)); // (2)
                                                                          // 将缓冲区数据流写入到管道中
                    await clientChannel.WriteAndFlushAsync(initialMessage); // (3)
                    //if (r.Contains("bye"))
                    //    break;
                }
                //Console.ReadLine();
                await clientChannel.CloseAsync();
            }  
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            } 
            finally
            {
                await group.ShutdownGracefullyAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            }
        }
    }
}
