//ChatPanel.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    //���Ͱ�ť
    public Button sendBtn;
    //�����
    public InputField inputField;
    //Scroll View
    public ScrollRect sr;

    // Start is called before the first frame update
    void Start()
    {
        //Ϊ��ť��ӵ���¼���������
        sendBtn.onClick.AddListener(() =>
        {
            //�����������ݲ��ǿ�ʱ��������Ϣ����������������Scroll View����������
            if (inputField.text != "")
            {
                NetManager.Instance.Send(inputField.text);
                UpdateChatInfo("�ң�" + inputField.text);
                //�����꽫�������������
                inputField.text = "";
            }
        });
    }

    //������������
    public void UpdateChatInfo(string msgInfo)
    {
        //��Scroll View�е�Content�ж�̬����TextԤ����
        Text chatInfoText = Instantiate(Resources.Load<Text>("UI/MsgInfoText"), sr.content);
        //�޸�Text������
        chatInfoText.text = msgInfo;
    }
}

