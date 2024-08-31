//NetManager.cs

using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetManager : MonoBehaviour
{
    public static NetManager Instance => instance;
    private static NetManager instance;

    //客户端socket
    private Socket socket;
    //是否连接状态
    private bool isConnect;
    //发送消息队列
    private Queue<string> sendQueue = new Queue<string>();
    //接收消息队列
    private Queue<string> receiveQueue = new Queue<string>();
    //存放收到消息的字节数组
    private byte[] receiveBytes = new byte[1024 * 1024];

    //UI面板（这里违背了单例模式的设计思想，由于项目比较简单，图方便就直接在这里得到UI面板了）
    //实际项目可以封装一个UI管理类（单例模式）管理UI更新，收到消息通过UI管理类去更新对应的UI面板
    private ChatPanel chatPanel;

    private void Awake()
    {
        //初始化单例对象
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //得到UI面板（这里违背了单例模式的设计思想，由于项目比较简单，图方便就直接在这里得到UI面板了）
        //实际项目可以封装一个UI管理类（单例模式）管理UI更新，收到消息通过UI管理类去更新对应的UI面板
        GameObject chatPanelObj = GameObject.Find("ChatPanel");
        chatPanel = chatPanelObj.GetComponent<ChatPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //如果收到消息队列中存在消息，则让UI更新面板
        if (receiveQueue.Count > 0)
        {
            //这里图方便，实际项目可以封装一个UI管理类（单例模式）管理UI更新，收到消息通过UI管理类去更新对应的UI面板
            chatPanel.UpdateChatInfo("他人：" + receiveQueue.Dequeue());
        }
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    /// <param name="ip">服务器IP地址</param>
    /// <param name="port">服务器程序端口号</param>
    public void ConnectServer(string ip, int port)
    {
        //如果在连接状态，就不执行连接逻辑了
        if (isConnect)
            return;

        //避免重复创建socket
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //连接服务器
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

        try
        {
            socket.Connect(ipEndPoint);
        }
        catch (SocketException e)
        {
            print(e.ErrorCode + e.Message);
            return;
        }

        isConnect = true;

        //开启发送消息线程
        ThreadPool.QueueUserWorkItem(SendMsg_Thread);
        //开启接收消息线程
        ThreadPool.QueueUserWorkItem(ReceiveMsg_Thread);
    }

    //发送消息
    public void Send(string msg)
    {
        //将消息放入到消息队列中
        sendQueue.Enqueue(msg);
    }

    private void SendMsg_Thread(object obj)
    {
        while (isConnect)
        {
            //如果消息队列中有消息，则发送消息
            if (sendQueue.Count > 0)
            {
                socket.Send(Encoding.UTF8.GetBytes(sendQueue.Dequeue()));
            }
        }
    }

    //接收消息
    private void ReceiveMsg_Thread(object obj)
    {
        print("持续监听是否收到消息");
        int msgLength;
        while (isConnect)
        {
            //判断有没有收到消息
            if (socket.Available > 0)
            {
                msgLength = socket.Receive(receiveBytes);
                print("接收到消息，长度为" + msgLength);
                //收到消息，反序列化后放入收到消息队列中，在Update中不停检测有没有收到的消息，收到了就让UI更新面板
                receiveQueue.Enqueue(Encoding.UTF8.GetString(receiveBytes, 0, msgLength));
            }
        }
    }

    //释放连接
    public void Close()
    {
        if (socket != null && isConnect)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            isConnect = false;
        }
    }

    private void OnDestroy()
    {
        Close();
    }
}


