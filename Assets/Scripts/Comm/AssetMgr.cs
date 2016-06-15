using UnityEngine;
using System.Collections;

public class AssetMgr : MonoBehaviour {

    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public static readonly string PathURL =
#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
	"file://" + Application.dataPath + "/StreamingAssets/";
#else
 string.Empty;
#endif


    //本地读取
    private IEnumerator LoaclLoadAssetBundle(string path)
    {
        WWW bundle = new WWW(path);

        yield return bundle;

        //加载到游戏中
        yield return Instantiate(bundle.assetBundle.mainAsset);

        bundle.assetBundle.Unload(false);
    } 
    //网络加载
    private IEnumerator NetLoadAssetBundle(string path)
    {
        WWW www = new WWW(path);

        yield return www;

        var bundle = www.assetBundle; 

        //AssetBundle.l
    } 
}
