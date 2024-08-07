using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class Lesson_MD5 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Application.persistentDataPath);
        ABUpdateMgr.Instance.CheckUpdate((isOver) =>
        {
            if(isOver)
            {
                print("检测更新结束，隐藏进度条");
            }
            else 
            {
                print("网络出错，提示玩家检测网络");
            }
        }, (str) =>
        {
            print(str);
        });

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private string GetMD5(string filePath)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Open))
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //得到数据的MD5码 16个字节 数组
            byte[] md5Info = md5.ComputeHash(file);

            file.Close();

            //把16个字节转换为 16进制 拼接成字符串 为了减少md5码的长度
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < md5Info.Length; i++)
            {
                sb.Append(md5Info[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }

}
