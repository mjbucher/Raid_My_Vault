using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Motive.Unity.Media;
using System;
using Motive.Core.Models;
using Motive.Core.Scripting;

public class TextMediaPopupPanel : Panel<ITextMediaContent> {
	public Text Text;

    public override void DidShow(ITextMediaContent data)
	{
		Text.text = data.Text;

		base.DidShow (data);

        if (data.MediaItem != null && data.MediaItem.MediaType == Motive.Core.Media.MediaType.Audio)
        {
            var localUrl = WebServices.Instance.MediaDownloadManager.GetPathForItem(data.MediaItem.Url);

            UnityAudioPlayerChannel.Instance.Play(new Uri(localUrl));
        }
	}
}
