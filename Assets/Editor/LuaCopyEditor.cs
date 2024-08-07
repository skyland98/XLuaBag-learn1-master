using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class LuaCopyEditor : Editor
{
    
    [MenuItem("XLua/自动生成txt后缀的Lua")]
    public static void CopyLuaToText()
    {
        string path = Application.dataPath + "/Lua/";

        if(!Directory.Exists(path))
           return;

        string[] strs = Directory.GetFiles(path,"*.lua");

        string newPath = Application.dataPath +"/LuaTxt/";

        if(!Directory.Exists(newPath))
           Directory.CreateDirectory(newPath);
        else
        {
            string[] oldFileStrs = Directory.GetFiles(newPath,"*.txt");
            for(int i = 0;i<oldFileStrs.Length;i++)
            {
                File.Delete(oldFileStrs[i]);
            }
        }

        List<string> newFilesNames = new List<string>();
        string fileName;
        
        for(int i = 0;i<strs.Length;i++)
        {
            fileName = newPath + strs[i].Substring(strs[i].LastIndexOf("/")+1) + ".txt";
            newFilesNames.Add(fileName);
            File.Copy(strs[i],fileName);
        }

        AssetDatabase.Refresh();

        for(int i = 0;i<newFilesNames.Count;i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(newFilesNames[i].Substring(newFilesNames[i].IndexOf("Assets")));
            if(importer != null) 
                importer.assetBundleName ="lua";
        }

    }

}
