//Main.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCPMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //如果网络模块单例对象为空
        if (NetManager.Instance == null)
        {
            //创建一个空物体
            GameObject netGameObj = new GameObject("Net");
            //挂载NetManager脚本
            netGameObj.AddComponent<NetManager>();
        }
        //连接服务器
        NetManager.Instance.ConnectServer("127.0.0.1", 8080);
    }
}


