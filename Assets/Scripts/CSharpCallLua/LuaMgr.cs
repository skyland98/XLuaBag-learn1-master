using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaMgr : BaseManager<LuaMgr>
{
    private LuaEnv luaEnv;

    /// <summary>
    /// 得到Lua中的_G
    /// </summary>
    public LuaTable Global
    {
        get
        {
            return luaEnv.Global;
        }
    }

    public void Init()
    {
        if(luaEnv != null)
            return;
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(MyCustomLoader);
        luaEnv.AddLoader(MyCustomABLoader);
    }

    public void DoLuaFile(string fileName)
    {
        string str = string.Format("require('{0}')",fileName);
        DoString(str);
    }


    public void DoString(string str)
    {
        if(luaEnv == null)
        {
            Debug.Log("解析器未初始化");
            return;
        }

        luaEnv.DoString(str);
    }

    //释放lua垃圾
    public void Tick()
    {
        if (luaEnv == null)
        {
            Debug.Log("解析器未初始化");
            return;
        }
        luaEnv.Tick();
    }

    //销毁解析器
    public void Dispose()
    {
        if (luaEnv == null)
        {
            Debug.Log("解析器未初始化");
            return;
        }
        luaEnv.Dispose();
        luaEnv = null;
    }

    private byte[] MyCustomLoader(ref string filePath)
    {
        string path = Application.dataPath + "/Lua/" + filePath + ".lua";
        //Debug.Log(path);

        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }
        else
        {
            Debug.Log("重定向失败");
        }

        return null;
    }

    private byte[] MyCustomABLoader(ref string filePath)
    {
        //Debug.Log("进入AB包加载重定向");
        //string path = Application.streamingAssetsPath + "/lua";
        //AssetBundle ab = AssetBundle.LoadFromFile(path);
        //TextAsset tx = ab.LoadAsset<TextAsset>(filePath +".lua");
        //return tx.bytes;

        TextAsset lua = ABMgr.Instance.LoadRes("lua", filePath + ".lua") as TextAsset;

        if (lua != null)
            return lua.bytes;
        else
            Debug.Log("MyCustomABLoader重定向失败，文件名为" + filePath);

        return null;
    }
}
