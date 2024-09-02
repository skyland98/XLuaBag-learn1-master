using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红点树结构
/// </summary>
public class RedPointConst
{
    public const string main = "Main"; //主界面
    public const string mail = "Main.Mail"; //主界面邮件
    public const string mailSystem = "Main.Mail.System"; //邮件系统
    public const string mailTeam = "Main.Mail.Team"; //邮件队伍
    public const string mailTeamInfo1 = "Main.Mail.Team.Info1"; //邮件队伍信息
    public const string mailTeamInfo2 = "Main.Mail.Team.Info2"; //邮件队伍信息
}


public class RedPointNode
{
    public string nodeName = string.Empty; //节点名称
    public int pointNum = 0; //红点数量
    public RedPointNode parent = null; //父节点
    public RedPointSystem.OnPointNumChange numChangeFunc; //发生变化的回调函数

    //子节点
    public Dictionary<string, RedPointNode> dicChildren = new Dictionary<string, RedPointNode>();

    /// <summary>
    /// 设置当前节点的红点数量
    /// </summary>
    /// <param name="rpNum"></param>
    public void SetRedPointNum(int rpNum)
    {
        if (dicChildren.Count > 0) //红点数量只能设置叶子节点
        {
            Debug.LogError("Only Can Set Leaf Nodes!");
            return;
        }
        pointNum = rpNum;

        NotifyPointNumChange();

        //向上通知红点
        if (nodeName != RedPointConst.main && parent.nodeName != string.Empty)
        {
            parent.ChangePredPointNum();
        }
    }

    /// <summary>
    /// 计算当前红点数量
    /// </summary>
    public void ChangePredPointNum()
    {
        int num = 0;

        //计算红点总数
        foreach (var node in dicChildren.Values)
        {
            num += node.pointNum;
        }
        if (num != pointNum) //红点有变化
        {
            pointNum = num;
            NotifyPointNumChange();
        }

        //向上通知红点
        if (nodeName != RedPointConst.main && parent.nodeName != string.Empty)
        {
            parent.ChangePredPointNum();
        }
    }

    /// <summary>
    /// 通知红点数量变化
    /// </summary>
    public void NotifyPointNumChange()
    {
        numChangeFunc?.Invoke(this);
    }
}


