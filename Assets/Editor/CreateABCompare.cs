using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;



public class CreateABCompare 
{
    //[MenuItem("AB包工具/创建对比文件")]
    public static void CreateABCompareFile()
    {
        DirectoryInfo directory = Directory.CreateDirectory("E:\\VR\\AR\\AssetBundles\\PC\\");
        FileInfo[] fileInfos = directory.GetFiles();

        //用于存储信息的 字符串
        string abCompareInfo = "";

        foreach (FileInfo info in fileInfos)
        {
            //没有后缀的才是AB包
            if(info.Extension == "")
            {
                //Debug.Log(info.Name);
                abCompareInfo += info.Name + " " + info.Length + " " + GetMD5(info.FullName);
                abCompareInfo += "|";
            }
        }

        abCompareInfo = abCompareInfo.Substring(0,abCompareInfo.Length - 1);

        //Debug.Log(abCompareInfo);
        File.WriteAllText("E:\\VR\\AR\\AssetBundles\\PC\\ABCompareInfo.txt", abCompareInfo);
        Debug.Log("AB包对比文件生成成功");
    }

    public static string GetMD5(string filePath)
    {        

        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //得到数据的MD5码 16个字节 数组
            byte[] md5Info = md5.ComputeHash(file);

            file.Close();

            //把16个字节转换为 16进制 拼接成字符串 为了减少md5码的长度
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Info.Length; i++)
            {
                sb.Append(md5Info[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

}
