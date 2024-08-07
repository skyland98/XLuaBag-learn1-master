using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

//�Զ�����
public class Test
{
    public void Speak(string str)
    {
        Debug.Log(str);
    }
}

namespace Liu
{
    public class Test2
    {
        public void Speak(string str)
        {
            Debug.Log("Test2:"+str);
        }
    }
}

#region ���� List �ֵ�

public class Lesson3
{
    public int[] array = new int[5] { 1,2,3,4,5};

    public List<int> list = new List<int>(); 
    
    public Dictionary<int,string> dict = new Dictionary<int,string>();

}

#endregion

#region ��չ����(Ҫ��������)

//luaͨ������Ļ��Ƶ���c#�� Ч�ʽϵ�
[LuaCallCSharp]
public static class Tools
{
    public static void Move(this Lesson4 obj)
    {
        Debug.Log(obj.name + "�ƶ�");
    }
}

public class Lesson4
{
    public string name = "liu";
    public void Speak(string str)
    {
        Debug.Log(str);
    }

    public static void Eat()
    {
        Debug.Log("�Զ���");
    }
}
#endregion

#region ref��out

public class Lesson5
{
    public int RefFun(int a,ref int b,ref int c,int d)
    {
        b = a + d;
        c = a - d;
        return 100;
    }
    public int OutFun(int a, out int b, out int c, int d)
    {
        b = a;
        c = d;
        return 200;
    }
    public int RefOutFun(int a, ref int b ,out int c)
    {
        b = a * 10;
        c = a * 20;
        return 300;
    }
}

#endregion

#region ί�к��¼�
public class Lesson7
{
    public UnityAction del;

    public event UnityAction eventAction;

    public void DoEvent()
    {
        eventAction();
    }
}

#endregion

#region ��ά����

public class Lesson8
{
    public int[,] array = new int[2, 3] { { 1,2,3},{ 4,5,6} };

   
}

#endregion

#region ϵͳ���ͼ�����  ʹ��Э��Ҫ�ӵ�����

public static class Lesson10
{
    [CSharpCallLua]
    public static List<Type> csharpCallLuaList = new List<Type>() {
        typeof(UnityAction<float>),
        typeof(UnityAction<bool>),
        typeof(System.Collections.IEnumerator)
    };
}

#endregion

#region

public class Lesson12
{
    public  interface ITest
    {

    }

    public class TestFather
    {

    }

    public class TestChild:TestFather,ITest
    {

    }

    public void TestFun1<T>(T a,T b) where T:TestFather
    {
        Debug.Log("�в�����Լ���ķ��ͷ���");
    }
    public void TestFun2<T>(T a )
    {
        Debug.Log("�в�����Լ���ķ��ͷ���");
    }

    public void TestFun3<T>() where T : TestFather
    {
        Debug.Log("�޲�����Լ���ķ��ͷ���");
    }
    public void TestFun4<T>(T a) where T : ITest
    {
        Debug.Log("�в�����Լ�� ��Լ���Ǹ��ӿ�");
    }
}

#endregion

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
        LuaMgr.GetInstance().Init();
        LuaMgr.GetInstance().DoLuaFile("Main");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
