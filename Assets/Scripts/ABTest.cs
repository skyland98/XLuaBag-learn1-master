using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ABMgr.Instance.LoadResAsync("model", "Cube", (obj) => {
            (obj as GameObject).transform.position = Vector3.up;
        });

        //Object obj = ABMgr.Instance.LoadRes("model", "Cube");
        //Instantiate(obj);

        //Object obj2 = ABMgr.Instance.LoadRes("model", "Cube");
        //Instantiate(obj2,Vector3.up,transform.rotation);


        //Debug.Log(transform.position);
        //加载AB包
        //AssetBundle ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "model");
        //加载AB包中的资源
        //GameObject obj = ab.LoadAsset<GameObject>("Cube");
        //Instantiate(obj);

        //StartCoroutine(LoadABRes("model", "Cube"));

    }

    IEnumerator LoadABRes(string ABName,string resName)
    {
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/" + ABName);
        yield return abcr;

        AssetBundleRequest abq = abcr.assetBundle.LoadAssetAsync(resName,typeof(GameObject));
        yield return abq;
        Instantiate(abq.asset as GameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
