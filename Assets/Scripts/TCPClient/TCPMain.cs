//Main.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�������ģ�鵥������Ϊ��
        if (NetManager.Instance == null)
        {
            //����һ��������
            GameObject netGameObj = new GameObject("Net");
            //����NetManager�ű�
            netGameObj.AddComponent<NetManager>();
        }
        //���ӷ�����
        NetManager.Instance.ConnectServer("127.0.0.1", 8080);
    }
}


