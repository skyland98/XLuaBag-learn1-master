using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPointTest : MonoBehaviour
{
    public Text txtMail;
    public Text txtMailSystem;
    public Text txtMailTeam;
    public Text txtMailTeamInfo1;
    public Text txtMailTeamInfo2;

    RedPointSystem rps = new RedPointSystem();

    void Start()
    {
        //初始化红点树
        rps.InitRedPointTreeNode();

        //绑定回调
        rps.SetRedPointNodeCallBack(RedPointConst.mail, MailCallBack);
        rps.SetRedPointNodeCallBack(RedPointConst.mailSystem, MailSystemCallBack);
        rps.SetRedPointNodeCallBack(RedPointConst.mailTeamInfo1, MailTeamInfo1CallBack);
        rps.SetRedPointNodeCallBack(RedPointConst.mailTeamInfo2, MailTeamInfo2CallBack);
        rps.SetRedPointNodeCallBack(RedPointConst.mailTeam, MailTeamCallBack);

        //修改红点数量
        rps.SetInvoke(RedPointConst.mailSystem, 3);
        rps.SetInvoke(RedPointConst.mailTeamInfo1, 2);
        rps.SetInvoke(RedPointConst.mailTeamInfo2, 1);

        rps.Traverse(); //打印树
    }

    //回调事件
    void MailCallBack(RedPointNode node)
    {
        txtMail.text = node.pointNum.ToString();
        txtMail.gameObject.SetActive(node.pointNum > 0);
        Debug.Log("NodeName: " + node.nodeName + " PointNum:" + node.pointNum);
    }

    void MailTeamCallBack(RedPointNode node)
    {
        txtMailTeam.text = node.pointNum.ToString();
        txtMailTeam.gameObject.SetActive(node.pointNum > 0);
        Debug.Log("NodeName: " + node.nodeName + " PointNum:" + node.pointNum);
    }

    void MailSystemCallBack(RedPointNode node)
    {
        txtMailSystem.text = node.pointNum.ToString();
        txtMailSystem.gameObject.SetActive(node.pointNum > 0);
        Debug.Log("NodeName: " + node.nodeName + " PointNum:" + node.pointNum);
    }

    void MailTeamInfo1CallBack(RedPointNode node)
    {
        txtMailTeamInfo1.text = node.pointNum.ToString();
        txtMailTeamInfo1.gameObject.SetActive(node.pointNum > 0);
        Debug.Log("NodeName: " + node.nodeName + " PointNum:" + node.pointNum);
    }

    void MailTeamInfo2CallBack(RedPointNode node)
    {
        txtMailTeamInfo2.text = node.pointNum.ToString();
        txtMailTeamInfo2.gameObject.SetActive(node.pointNum > 0);
        Debug.Log("NodeName: " + node.nodeName + " PointNum:" + node.pointNum);
    }

    //移除指定红点
    public void RemoveRedPoint()
    {
        rps.RemoveRedPointFromTree(RedPointConst.mailTeamInfo1);

        rps.Traverse(); //打印树
    }

    //添加指定红点
    public void AddRedPoint()
    {
        rps.AddNewRedPointToTree(RedPointConst.mailTeamInfo1);
        rps.SetRedPointNodeCallBack(RedPointConst.mailTeamInfo1, MailTeamInfo1CallBack);
        rps.SetInvoke(RedPointConst.mailTeamInfo1, 2);

        rps.Traverse(); //打印树
    }
}

