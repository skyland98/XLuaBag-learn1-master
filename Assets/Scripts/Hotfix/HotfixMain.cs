using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

[Hotfix]
public class HotfixTest
{
    public HotfixTest(){
        Debug.Log("HotfixTest构造函数");

    }

    public void Speak(string str){
        Debug.Log(str);
    }
 
}

[Hotfix]
public class HotfixMain : MonoBehaviour
{
    HotfixTest hotfixTest;

    public int[] array = new int[] {1,2,3};

    event UnityAction myEvent;

    public int Age{
        get
        {
            return 0;
        }
        set
        {
            Debug.Log(value);
        }
    }
    public int this[int index]{
        get
        {
            if(index >= array.Length || index <0)
            {
                Debug.Log("索引不正确");
                return 0;

            }
            return array[index];
        }
        set
        {
            if(index >= array.Length || index <0)
            {
                Debug.Log("索引不正确");
                return ;

            }
            array[index] = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        LuaMgr.GetInstance().Init();
        LuaMgr.GetInstance().DoLuaFile("Main");

        Add(10,20);
        Speak("你好世界");

        hotfixTest = new HotfixTest();
        hotfixTest.Speak("嘻嘻嘻");

        //StartCoroutine(TestCoroutine());

        this.Age = 100;

        Debug.Log(this.Age);

        myEvent +=TestTest;
        myEvent -=TestTest;
    }

    private void TestTest(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TestCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("C#协程打印一次");
        }
    }

    public int Add(int a,int b)
    {
        return 0;
    }

    public static void Speak(string str)
    {
        Debug.Log("哈哈哈哈");
    }


}
