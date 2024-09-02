using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RedPointSystem : MonoBehaviour
{
    public delegate void OnPointNumChange(RedPointNode node); //���仯֪ͨ
    RedPointNode mRootNode; //�����Root�ڵ�

    static List<string> redPointTreeList = new List<string> //��ʼ�������
    {
        //����������Ϣ

        RedPointConst.main,
        RedPointConst.mail,
        RedPointConst.mailSystem,
        RedPointConst.mailTeam,
        RedPointConst.mailTeamInfo1,
        RedPointConst.mailTeamInfo2,
    };

    /// <summary>
    /// ��ʼ��������ṹ
    /// </summary>
    public void InitRedPointTreeNode()
    {
        mRootNode = new RedPointNode(); //���ڵ�
        mRootNode.nodeName = RedPointConst.main; //���ø��ڵ�����

        foreach (var s in redPointTreeList)
        {
            AddNewRedPointToTree(s);
        }
    }

    /// <summary>
    /// �������нڵ㣨�Ӹ��ڵ㿪ʼ��
    /// </summary>
    public void Traverse()
    {
        TraverseTree(mRootNode);
    }

    /// <summary>
    /// �����ýڵ��µ����нڵ�
    /// </summary>
    /// <param name="node"></param>
    void TraverseTree(RedPointNode node)
    {
        Debug.Log(node.nodeName);
        if (node.dicChildren.Count == 0)
        {
            return;
        }

        foreach (var item in node.dicChildren.Values)
        {
            TraverseTree(item);
        }
    }

    /// <summary>
    /// �ں����������½ڵ�
    /// </summary>
    /// <param name="strNode"></param>
    public void AddNewRedPointToTree(string strNode)
    {
        var node = mRootNode;
        var treeNodeAy = strNode.Split('.'); //�и�ڵ���Ϣ
        if (treeNodeAy[0] != mRootNode.nodeName) //������ڵ㲻���ϣ����������ýڵ�
        {
            Debug.LogError("RedPointTree Root Node Error:" + treeNodeAy[0]);
            return;
        }

        if (treeNodeAy.Length > 1) //��������ӽڵ�
        {
            for (int i = 1; i < treeNodeAy.Length; i++)
            {
                //���treeNodeAy[i]�ڵ㻹���ǵ�ǰ�ڵ���ӽڵ㣬�����
                if (!node.dicChildren.ContainsKey(treeNodeAy[i]))
                {
                    node.dicChildren.Add(treeNodeAy[i], new RedPointNode());
                }
                node.dicChildren[treeNodeAy[i]].nodeName = treeNodeAy[i];
                node.dicChildren[treeNodeAy[i]].parent = node;

                node = node.dicChildren[treeNodeAy[i]]; //�����ӽڵ㣬��������
            }
        }
    }

    public void RemoveRedPointFromTree(string strNode)
    {
        var node = mRootNode;

        var treeNodeAy = strNode.Split('.'); //�и�ڵ���Ϣ
        if (treeNodeAy[0] != mRootNode.nodeName) //������ڵ㲻���ϣ����������ýڵ�
        {
            Debug.LogError("RedPointTree Root Node Error:" + treeNodeAy[0]);
            return;
        }

        if (treeNodeAy.Length > 1) //��������ӽڵ�
        {
            //������ȡ��ĩĿ��ڵ�
            for (int i = 1; i < treeNodeAy.Length; i++)
            {
                //�жϸýڵ��Ƿ��ں������
                if (!node.dicChildren.ContainsKey(treeNodeAy[i]))
                {
                    Debug.LogError("Does Not Contains Child Node: " + treeNodeAy[i]);
                    return;
                }

                node = node.dicChildren[treeNodeAy[i]];
            }

            RemoveNode(strNode, node);

            //���Ŀ��ڵ��Լ�ֻ�е�ǰ�ڵ�յĸ��ڵ�
            //while (parent.dicChildren.Count > 0)
            //{
            //    if (parent.dicChildren.Count > 1)
            //    {
            //        RemoveNode(strNode, node, parent);
            //        return;
            //    }
            //    else if (!parent.parent)
            //    {
            //        RemoveNode(strNode, node, parent);
            //        return;
            //    }

            //    node = parent;
            //    parent = node.parent;
            //}
        }
        else
        {
            Debug.LogError("You Are Trying To Delete Root!");
        }
    }

    void RemoveNode(string strNode, RedPointNode node)
    {
        SetInvoke(strNode, 0);
        node.parent.dicChildren.Remove(node.nodeName);
        node.parent = null;
    }

    /// <summary>
    /// ���ú��ص���������Ƴ�����ӵ���Ҫ���°��¼���
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="callBack"></param>
    public void SetRedPointNodeCallBack(string strNode, RedPointSystem.OnPointNumChange callBack)
    {
        var nodeList = strNode.Split('.'); //�������ڵ�
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.main)
            {
                //���ڵ㲻��
                Debug.LogError("Get Wrong Root Node! Current Is " + nodeList[0]);
                return;
            }
        }

        var node = mRootNode;

        //��������key����ȡ���һ���ڵ���ӻص�
        for (int i = 1; i < nodeList.Length; i++)
        {
            //�жϸýڵ��Ƿ��ں������
            if (!node.dicChildren.ContainsKey(nodeList[i]))
            {
                Debug.LogError("Does Not Contains Child Node: " + nodeList[i]);
                return;
            }
            node = node.dicChildren[nodeList[i]]; //��ȡ��ǰ�������Ľڵ�

            if (i == nodeList.Length - 1) //���һ���ڵ����ûص�
            {
                node.numChangeFunc = callBack;
                return;
            }
        }
    }

    /// <summary>
    /// ����ָ���ڵ�����
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="rpNum"></param>
    public void SetInvoke(string strNode, int rpNum)
    {
        var nodeList = strNode.Split('.'); //�������ڵ�

        //�жϸ��ڵ��Ƿ����
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.main)
            {
                Debug.LogError("Get Wrong Root Node! Current Is " + nodeList[0]);
                return;
            }
        }

        var node = mRootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            //�жϸñ����ڵ��Ƿ�������
            if (!node.dicChildren.ContainsKey(nodeList[i]))
            {
                Debug.LogError("Does Not Contains Child Node: " + nodeList[i]);
                return;
            }

            node = node.dicChildren[nodeList[i]];

            if (i == nodeList.Length - 1) //���һ���ڵ�
            {
                node.SetRedPointNum(rpNum); //���ýڵ�ĺ������
            }
        }
    }
}

