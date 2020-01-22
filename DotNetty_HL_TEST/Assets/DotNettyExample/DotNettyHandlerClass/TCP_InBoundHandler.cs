using System;
using System.Text;
using DotNetty.Buffers;
using DotNetty.Common.Utilities;
using DotNetty.Transport.Channels;
using UnityEngine;
//using static RMI__EncryptManager;

public class TCP_InBoundHandler : ChannelHandlerAdapter, IChannelHandler
{
	//각각, 채널 등록, 해제시에 불러와지는 Callback 메소드. 세부사항은 자바 Netty 프레임워크 동작방식 참조.
    public override void ChannelRegistered(IChannelHandlerContext context) { Debug.Log("ChannelRegistered"); }
    public override void ChannelUnregistered(IChannelHandlerContext context) { Debug.Log("ChannelUnregistered"); }  


	//TCP 통신이 가능해졌을 때 실행되는 Callback 메소드. 연결에 성공하였을 때의 처리를 하면 된다.
    void IChannelHandler.ChannelActive(IChannelHandlerContext context)
    {        
        Debug.Log("NetworkManager:TCP ChannelActive! LocalAddress=" + context.Channel.LocalAddress);

		//context.Channel 은 NetworkManager 의 tcpChannel 과 동일하다.
    }

	//TCP 통신이 불가능해졌을 때 실행되는 Callback 메소드. 연결이 끊겼을때의 처리를 하면 된다.
    void IChannelHandler.ChannelInactive(IChannelHandlerContext context)
    {
        Debug.Log("NetworkManager:TCP ChannelInactive!");
        
		//context.Channel 은 NetworkManager 의 tcpChannel 과 동일하다.
		
		//Channel.CloseAsync(); 를 호출하였을때도 ChannelInactive 부분이 호출된다. 
    }




    //TCP 데이터 수신시.
    public override void ChannelRead(IChannelHandlerContext context, object message)
    {
        var byteBuffer = message as IByteBuffer;
        if (byteBuffer != null)
        {
            Debug.Log("Received from server: " + byteBuffer.ToString(Encoding.UTF8));
        }
        context.WriteAsync(message);
    }
	
	//DotNetty 워커 스레드에서 예외 발생시 호출되는 Callback 메소드.
    public override void ExceptionCaught(IChannelHandlerContext context, Exception e)
    {
        Debug.Log(e);
		
		//예외가 발생한 채널의 연결을 종료...
        context.Channel.CloseAsync();
    }
}