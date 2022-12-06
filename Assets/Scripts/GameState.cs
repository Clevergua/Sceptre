using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private AssetBundle assetBundle;
    [SerializeField]
    private Object[] objects;
    [SerializeField]
    private AssetBundleManifest assetBundleManifest;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownloadAssetBundle());
    }
    IEnumerator DownloadAssetBundle()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "lua_scripts");
        var unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return unityWebRequest.SendWebRequest();
        assetBundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
        objects = assetBundle.LoadAllAssets();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
