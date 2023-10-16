using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Networking;

public class BundleLoader : MonoBehaviour
{
    public static BundleLoader Instance = null;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(this);
    }
    private void OnDestroy()
    {
        Instance = null;
    }
    public void DownloadBundle(string url, string fileName, Action callback)
    {
        StartCoroutine(_DownloadBundle(url, fileName, callback));
    }
    private IEnumerator _DownloadBundle(string url, string fileName, Action callback)
    {
        var bundleRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return bundleRequest.SendWebRequest();
        if(string.IsNullOrEmpty(bundleRequest.error) == false)
        {
            Debug.LogError(bundleRequest.error);
            yield break;
        }
        var bundleArray = bundleRequest.downloadHandler.data;
        DataHelper.WriteByteArrayData(bundleArray, fileName, fileName);
        callback?.Invoke();
    }
    public void LoadAssetFromBundle(string path, string objName, Action<GameObject> callback)
    {
        StartCoroutine(_LoadAssetFromBundle(path, objName, callback));
    }
    public void LoadAssetFromBundle(string path, string objName, Action<Sprite> callback)
    {
        StartCoroutine(_LoadAssetFromBundle(path, objName, callback));
    }
    private IEnumerator _LoadAssetFromBundle(string path, string objName, Action<GameObject> callback)
    {
        var datas = File.ReadAllBytes(path);
        if(datas == null)
        {
            Debug.LogError($"load bundle path maybe is wrong");
            yield break;
        }
        var bundle = AssetBundle.LoadFromMemory(datas);
        yield return bundle;
        var objResult = bundle.LoadAssetAsync<GameObject>(objName);
        yield return objResult;
        var obj = (GameObject)objResult.asset;
        callback?.Invoke(obj);
    }
    private IEnumerator _LoadAssetFromBundle(string path, string objName, Action<Sprite> callback)
    {
        var datas = File.ReadAllBytes(path);
        if (datas == null)
        {
            Debug.LogError($"load bundle path maybe is wrong");
            yield break;
        }
        var bundle = AssetBundle.LoadFromMemory(datas);
        yield return bundle;
        var objResult = bundle.LoadAssetAsync<Sprite>(objName);
        yield return objResult;
        var obj = (Sprite)objResult.asset;
        callback?.Invoke(obj);
    }

}
