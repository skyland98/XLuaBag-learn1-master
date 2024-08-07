using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MoveABToSA 
{
    //[MenuItem("AB包工具/移动选中的资源到StreaningAssets")]
    public static void MoveABToStreamingAssets()
    {
        Object[] selectedAsset = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);

        if (selectedAsset.Length == 0)
            return;

        //用于存储信息的 字符串
        string abCompareInfo = "";

        foreach (Object asset in selectedAsset)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            string fileName = assetPath.Substring(assetPath.LastIndexOf('/'));

            if (fileName.IndexOf(".") != -1)
                continue;

            AssetDatabase.CopyAsset(assetPath,"Assets/StreamingAssets" + fileName);

            FileInfo info = new FileInfo(Application.streamingAssetsPath + fileName);
            abCompareInfo += info.Name + " " + info.Length + " " + CreateABCompare.GetMD5(info.FullName);
            abCompareInfo += "|";
        }
        abCompareInfo = abCompareInfo.Substring(0, abCompareInfo.Length - 1);

        File.WriteAllText(Application.streamingAssetsPath + "/ABCompareInfo.txt", abCompareInfo);
    }
}
