using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ṹ
/// </summary>
public class RedPointConst
{
    public const string main = "Main"; //������
    public const string mail = "Main.Mail"; //�������ʼ�
    public const string mailSystem = "Main.Mail.System"; //�ʼ�ϵͳ
    public const string mailTeam = "Main.Mail.Team"; //�ʼ�����
    public const string mailTeamInfo1 = "Main.Mail.Team.Info1"; //�ʼ�������Ϣ
    public const string mailTeamInfo2 = "Main.Mail.Team.Info2"; //�ʼ�������Ϣ
}


public class RedPointNode
{
    public string nodeName = string.Empty; //�ڵ�����
    public int pointNum = 0; //�������
    public RedPointNode parent = null; //���ڵ�
    public RedPointSystem.OnPointNumChange numChangeFunc; //�����仯�Ļص�����

    //�ӽڵ�
    public Dictionary<string, RedPointNode> dicChildren = new Dictionary<string, RedPointNode>();

    /// <summary>
    /// ���õ�ǰ�ڵ�ĺ������
    /// </summary>
    /// <param name="rpNum"></param>
    public void SetRedPointNum(int rpNum)
    {
        if (dicChildren.Count > 0) //�������ֻ������Ҷ�ӽڵ�
        {
            Debug.LogError("Only Can Set Leaf Nodes!");
            return;
        }
        pointNum = rpNum;

        NotifyPointNumChange();

        //����֪ͨ���
        if (nodeName != RedPointConst.main && parent.nodeName != string.Empty)
        {
            parent.ChangePredPointNum();
        }
    }

    /// <summary>
    /// ���㵱ǰ�������
    /// </summary>
    public void ChangePredPointNum()
    {
        int num = 0;

        //����������
        foreach (var node in dicChildren.Values)
        {
            num += node.pointNum;
        }
        if (num != pointNum) //����б仯
        {
            pointNum = num;
            NotifyPointNumChange();
        }

        //����֪ͨ���
        if (nodeName != RedPointConst.main && parent.nodeName != string.Empty)
        {
            parent.ChangePredPointNum();
        }
    }

    /// <summary>
    /// ֪ͨ��������仯
    /// </summary>
    public void NotifyPointNumChange()
    {
        numChangeFunc?.Invoke(this);
    }
}


