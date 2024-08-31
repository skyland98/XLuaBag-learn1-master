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

    //�ͻ���socket
    private Socket socket;
    //�Ƿ�����״̬
    private bool isConnect;
    //������Ϣ����
    private Queue<string> sendQueue = new Queue<string>();
    //������Ϣ����
    private Queue<string> receiveQueue = new Queue<string>();
    //����յ���Ϣ���ֽ�����
    private byte[] receiveBytes = new byte[1024 * 1024];

    //UI��壨����Υ���˵���ģʽ�����˼�룬������Ŀ�Ƚϼ򵥣�ͼ�����ֱ��������õ�UI����ˣ�
    //ʵ����Ŀ���Է�װһ��UI�����ࣨ����ģʽ������UI���£��յ���Ϣͨ��UI������ȥ���¶�Ӧ��UI���
    private ChatPanel chatPanel;

    private void Awake()
    {
        //��ʼ����������
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //�õ�UI��壨����Υ���˵���ģʽ�����˼�룬������Ŀ�Ƚϼ򵥣�ͼ�����ֱ��������õ�UI����ˣ�
        //ʵ����Ŀ���Է�װһ��UI�����ࣨ����ģʽ������UI���£��յ���Ϣͨ��UI������ȥ���¶�Ӧ��UI���
        GameObject chatPanelObj = GameObject.Find("ChatPanel");
        chatPanel = chatPanelObj.GetComponent<ChatPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        //����յ���Ϣ�����д�����Ϣ������UI�������
        if (receiveQueue.Count > 0)
        {
            //����ͼ���㣬ʵ����Ŀ���Է�װһ��UI�����ࣨ����ģʽ������UI���£��յ���Ϣͨ��UI������ȥ���¶�Ӧ��UI���
            chatPanel.UpdateChatInfo("���ˣ�" + receiveQueue.Dequeue());
        }
    }

    /// <summary>
    /// ���ӷ�����
    /// </summary>
    /// <param name="ip">������IP��ַ</param>
    /// <param name="port">����������˿ں�</param>
    public void ConnectServer(string ip, int port)
    {
        //���������״̬���Ͳ�ִ�������߼���
        if (isConnect)
            return;

        //�����ظ�����socket
        if (socket == null)
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //���ӷ�����
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

        //����������Ϣ�߳�
        ThreadPool.QueueUserWorkItem(SendMsg_Thread);
        //����������Ϣ�߳�
        ThreadPool.QueueUserWorkItem(ReceiveMsg_Thread);
    }

    //������Ϣ
    public void Send(string msg)
    {
        //����Ϣ���뵽��Ϣ������
        sendQueue.Enqueue(msg);
    }

    private void SendMsg_Thread(object obj)
    {
        while (isConnect)
        {
            //�����Ϣ����������Ϣ��������Ϣ
            if (sendQueue.Count > 0)
            {
                socket.Send(Encoding.UTF8.GetBytes(sendQueue.Dequeue()));
            }
        }
    }

    //������Ϣ
    private void ReceiveMsg_Thread(object obj)
    {
        print("���������Ƿ��յ���Ϣ");
        int msgLength;
        while (isConnect)
        {
            //�ж���û���յ���Ϣ
            if (socket.Available > 0)
            {
                msgLength = socket.Receive(receiveBytes);
                print("���յ���Ϣ������Ϊ" + msgLength);
                //�յ���Ϣ�������л�������յ���Ϣ�����У���Update�в�ͣ�����û���յ�����Ϣ���յ��˾���UI�������
                receiveQueue.Enqueue(Encoding.UTF8.GetString(receiveBytes, 0, msgLength));
            }
        }
    }

    //�ͷ�����
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


