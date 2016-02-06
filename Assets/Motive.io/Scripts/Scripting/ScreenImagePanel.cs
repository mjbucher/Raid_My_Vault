using UnityEngine;
using System.Collections;
using Motive.Core.Models;
using UnityEngine.UI;

public class ScreenImagePanel : Panel<MediaContent> {
    public RawImage Image;

    public override void DidShow(MediaContent data)
    {
        base.DidShow(data);

        if (data.MediaItem != null)
        {
            StartCoroutine(ImageLoader.LoadImage(data.MediaItem.Url, Image));
        }
    }
}
