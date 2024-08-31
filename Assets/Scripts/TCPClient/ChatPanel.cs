//ChatPanel.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    //发送按钮
    public Button sendBtn;
    //输入框
    public InputField inputField;
    //Scroll View
    public ScrollRect sr;

    // Start is called before the first frame update
    void Start()
    {
        //为按钮添加点击事件监听函数
        sendBtn.onClick.AddListener(() =>
        {
            //当输入框的内容不是空时，发送消息到服务器，并更新Scroll View的聊天内容
            if (inputField.text != "")
            {
                NetManager.Instance.Send(inputField.text);
                UpdateChatInfo("我：" + inputField.text);
                //发送完将输入框的内容清空
                inputField.text = "";
            }
        });
    }

    //更新聊天内容
    public void UpdateChatInfo(string msgInfo)
    {
        //在Scroll View中的Content中动态创建Text预制体
        Text chatInfoText = Instantiate(Resources.Load<Text>("UI/MsgInfoText"), sr.content);
        //修改Text的内容
        chatInfoText.text = msgInfo;
    }
}

