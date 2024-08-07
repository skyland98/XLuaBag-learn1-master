using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

/// <summary>
/// Lua管理器
/// 提供lua解析器
/// 保证解析器的唯一性
/// </summary>

//无参无返回的委托
public delegate void CustomCall();

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        LuaMgr.GetInstance().Init();
        LuaMgr.GetInstance().DoLuaFile("Main");

        int i = LuaMgr.GetInstance().Global.Get<int>("testNumber");
        Debug.Log( "testNumber:"+i);

        CustomCall call = LuaMgr.GetInstance().Global.Get<CustomCall>("testFun");
        call();
        //Lua解析器 在Unity中执行Lua
        //LuaEnv luaEnv = new LuaEnv();
        //luaEnv.DoString("print('你好世界')");

        //luaEnv.AddLoader(MyCustomLoader);
        //luaEnv.DoString("require('Main')");
        //清除Lua中我们没有手动释放的对象 垃圾回收
        //luaEnv.Tick();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private byte[] MyCustomLoader(ref string filePath)
    //{
    //    string path = Application.dataPath + "/Lua/" + filePath + ".lua";
    //    Debug.Log(path);

    //    if(File.Exists(path))
    //    {
    //        return File.ReadAllBytes(path);
    //    }
    //    else
    //    {
    //        Debug.Log("重定向失败");
    //    }

    //    return null;
    //}

}
