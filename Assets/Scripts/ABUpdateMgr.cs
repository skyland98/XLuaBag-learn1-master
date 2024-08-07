using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ABUpdateMgr : MonoBehaviour
{
    private static ABUpdateMgr instance;
    public static ABUpdateMgr Instance
    {  
        get 
        { 
            if(instance == null)
            {
                GameObject obj = new GameObject("ABUpdateMgr");
                instance= obj.AddComponent<ABUpdateMgr>();
            }
            return instance; 
        } 
    }

    //用于存储远端AB包信息的字典 和 本地对比即可完成更新下载
    private Dictionary<string,ABInfo> remoteABInfo = new Dictionary<string,ABInfo>();

    //用于存储本地AB包信息的字典 
    private Dictionary<string, ABInfo> localABInfo = new Dictionary<string, ABInfo>();

    //这个是待下载AB包列表文件 
    private List<string> downLoadList = new List<string>();

    private string serverIP = "ftp://192.168.124.5";
    public void CheckUpdate(UnityAction<bool> overCallBack,UnityAction<string> updateInfoCallBack)
    {
        remoteABInfo.Clear();
        localABInfo.Clear();
        downLoadList.Clear();


        DownLoadABCompareFile((isOver) => {
            updateInfoCallBack("开始更新资源");
            if (isOver)
            {
                updateInfoCallBack("对比文件下载结束");

                string remoteInfo = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");
                updateInfoCallBack("解析远端对比文件");
                GetRemoteABCompareFileInfo(remoteInfo, remoteABInfo);
                updateInfoCallBack("解析远端对比文件完成");

                GetLocalABCompareFileInfo(() =>
                {
                    updateInfoCallBack("解析本地对比文件完成");

                    updateInfoCallBack("开始对比");
                    foreach (string abName in remoteABInfo.Keys)
                    {
                        if(!localABInfo.ContainsKey(abName))
                            downLoadList.Add(abName);
                        else
                        {
                            if (localABInfo[abName].md5 != remoteABInfo[abName].md5)
                                downLoadList.Add(abName);

                            localABInfo.Remove(abName);
                        }
                    }
                    updateInfoCallBack("对比完成");
                    updateInfoCallBack("删除无用的AB包");
                    //删除无用的AB包
                    foreach (string abName in localABInfo.Keys)
                    {
                        if (File.Exists(Application.persistentDataPath + "/" + abName))
                            File.Delete(Application.persistentDataPath + "/" + abName);
                    }

                    updateInfoCallBack("下载和更新AB包");
                    DownLoadABFile((isOver) =>
                    {
                        if(isOver)
                        {
                            updateInfoCallBack("更新本地AB包为最新");
                            File.WriteAllText(Application.persistentDataPath + "/ABCompareInfo.txt",remoteInfo);
                        }
                        overCallBack(isOver);
                    });

                });
            }
            else
            {
                overCallBack(false);
            }
        });
       
    }

    public async void DownLoadABCompareFile(UnityAction<bool> overCallBack)
    {
        print(Application.persistentDataPath);

        string localPath = Application.persistentDataPath + "/";
        bool isOver = false;
        int reDownLoadMaxNum = 5;
        while(!isOver &&  reDownLoadMaxNum > 0)
        {
            await Task.Run(() => {
                isOver = DownLoadFile("ABCompareInfo.txt", localPath + "/ABCompareInfo_TMP.txt");
            });
            --reDownLoadMaxNum;
        }

        overCallBack?.Invoke(isOver);    
        
    }

    //解析远端和本地的对比文件 并存入字典中
    public void GetRemoteABCompareFileInfo(string info, Dictionary<string, ABInfo> ABInfo)
    {
        //string info = File.ReadAllText(Application.persistentDataPath + "/ABCompareInfo_TMP.txt");

        string[] strs = info.Split("|");

        string[] infos = null;
        for (int i = 0; i < strs.Length; i++)
        {
            infos = strs[i].Split(" ");
            ABInfo abInfo = new ABInfo(infos[0], infos[1], infos[2]);
            //记录每一个远端的AB包信息 好用来对比
            ABInfo.Add(infos[0], abInfo);
        }

        print("AB包对比文件 加载结束");
    }

    public void GetLocalABCompareFileInfo(UnityAction overCallBack)
    {
        if (File.Exists(Application.persistentDataPath + "/ABCompareInfo.txt"))
        {
            StartCoroutine(GetLocalABCompareFileInfo(Application.persistentDataPath + "/ABCompareInfo.txt", overCallBack));
        }
        else if (File.Exists(Application.streamingAssetsPath + "/ABCompareInfo.txt"))
        {
            StartCoroutine(GetLocalABCompareFileInfo(Application.streamingAssetsPath + "/ABCompareInfo.txt", overCallBack));
        }
        else
            overCallBack();
    }

    private IEnumerator GetLocalABCompareFileInfo(string filePath, UnityAction overCallBack)
    {
        UnityWebRequest req = UnityWebRequest.Get(filePath);
        yield return req.SendWebRequest();
        
        GetRemoteABCompareFileInfo(req.downloadHandler.text,localABInfo);

        overCallBack();
    }


    public async void DownLoadABFile(UnityAction<bool> overCallBack)
    {
        

        string localPath = Application.persistentDataPath + "/";
        //是否下载成功
        bool isOver;
        //下载成功的列表
        List<string> tmpList = new List<string>();

        int reDownLoadMaxNum = 5;
        int downLoadOverNum = 0;
        while(downLoadList.Count > 0 && reDownLoadMaxNum >0)
        {
            for (int i = 0; i < downLoadList.Count; i++)
            {
                isOver = false;
                await Task.Run(() => {
                    isOver = DownLoadFile(downLoadList[i], localPath + downLoadList[i]);
                });

                if (isOver)
                {
                    print("下载进度" + ++downLoadOverNum + "/" + downLoadList.Count);
                    tmpList.Add(downLoadList[i]);//下载成功记录下来
                }
            }

            for (int i = 0; i < tmpList.Count; i++)
                downLoadList.Remove(tmpList[i]);

            --reDownLoadMaxNum;
        }

        //告诉外部下载完成     
        overCallBack(downLoadList.Count == 0);

        
    }



    private bool DownLoadFile(string fileName,string localPath)
    {
        try
        {
            string pInfo =
#if UNITY_IOS
"IOS";
#elif UNITY_ANDROID
"Android";
#else
"PC";
#endif

            FtpWebRequest req = FtpWebRequest.Create(new Uri(serverIP + "/AB/"+pInfo+"/" + fileName)) as FtpWebRequest;
            NetworkCredential n = new NetworkCredential("Liu", "Liu123");
            req.Credentials = n;

            req.Proxy = null;
            req.KeepAlive = false;
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            req.UseBinary = true;

            FtpWebResponse res = req.GetResponse() as FtpWebResponse;
            Stream downLoadStream = res.GetResponseStream();

            using (FileStream file = File.Create(localPath))
            {
                //一点一点下载
                byte[] bytes = new byte[2048];

                int contentLength = downLoadStream.Read(bytes, 0, bytes.Length);

                while (contentLength != 0)
                {
                    file.Write(bytes, 0, contentLength);
                    contentLength = downLoadStream.Read(bytes, 0, bytes.Length);
                }

                file.Close();
                downLoadStream.Close();

                print(fileName + "下载成功" );
                return true;
            }
        }
        catch (Exception e)
        {
            print(fileName +"下载失败" + e.Message);
            return false;
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public class ABInfo
    {
        public string name;
        public long size;
        public string md5;

        public ABInfo(string name,string size,string md5)
        {
            this.name = name;
            this.size = long.Parse(size);
            this.md5 = md5;
        }
    }
}
