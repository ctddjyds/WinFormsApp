using DotNetty.Codecs;
using DotNetty.Handlers.Logging;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyApplication
{
    public class MutiClient
    {
        private Bootstrap bootstrap = new Bootstrap();
        //会话对象组 
        private List<IChannel> channels = new List<IChannel>();
        public async void init(int count)
        {
            var group = new MultithreadEventLoopGroup();//
            bootstrap.Group(group)
            .Channel<TcpSocketChannel>()
            .Option(ChannelOption.TcpNodelay, true)
            .Handler(new ActionChannelInitializer<ISocketChannel>(channel => 
            {
                IChannelPipeline pipeline = channel.Pipeline;
                pipeline.AddLast(new LoggingHandler());
                pipeline.AddLast("framing-enc", new LengthFieldPrepender(2));
                pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(ushort.MaxValue, 0, 2, 0, 2));
                // IdleStateHandler
                pipeline.AddLast("timeout", new IdleStateHandler(0, 0, 10));
                pipeline.AddLast("echo", new EchoClientHandler());
                
            }));
            for(int i=0; i<count;i++)
            {
                IChannel clientChannel = await bootstrap.ConnectAsync("192.168.1.100", 8888);
                channels.Add(clientChannel);
            }
        } 
        private IChannel getChannel(int count)
        {
            IChannel channel = channels[count];
            if (!channel.Active)
            {
                //重连 
                reconnect(channel);
                //尝试获取下一个channel 
                return getChannel(++count);
            }
            return channel; 
        } 
        private async void reconnect(IChannel channel)
        {  
            channel = await bootstrap.ConnectAsync("192.168.1.100", 8888);
            channels.Add(channel);
        }

    }
}
