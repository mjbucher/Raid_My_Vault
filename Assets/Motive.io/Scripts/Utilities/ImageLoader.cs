using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public static class ImageLoader
{
    public static IEnumerator LoadImage(string url, RawImage image)
    {
        var local = WebServices.Instance.MediaDownloadManager.GetPathForItem(url);
        var localUri = (new Uri(local)).AbsoluteUri;

        var www = new WWW(localUri);

        yield return www;

        image.texture = www.texture;
    }

    public static IEnumerator LoadImage(string url, Action<Texture> onLoad)
    {
        var local = WebServices.Instance.MediaDownloadManager.GetPathForItem(url);
        var localUri = (new Uri(local)).AbsoluteUri;

        var www = new WWW(localUri);

        yield return www;

        onLoad(www.texture);
    }

    public static IEnumerator LoadImage(string url, GameObject obj, Dictionary<string, Texture> textureCache = null)
    {
        if (textureCache != null && textureCache.ContainsKey(url))
        {
            obj.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = textureCache[url];
        }

        obj.gameObject.SetActive(false);

        var local = WebServices.Instance.MediaDownloadManager.GetPathForItem(url);
        var localUri = (new Uri(local)).AbsoluteUri;

        var www = new WWW(localUri);

        yield return www;

        obj.gameObject.SetActive(true);

        obj.gameObject.GetComponentInChildren<Renderer>().material.mainTexture = www.texture;

        if (textureCache != null)
        {
            textureCache[url] = www.texture;
        }
    }
}
