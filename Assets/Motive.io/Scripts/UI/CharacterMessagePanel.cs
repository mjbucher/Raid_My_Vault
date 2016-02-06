using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Motive.Unity.Media;
using Motive.Core.Scripting;

public class CharacterMessagePanel : Panel<ResourcePanelData<CharacterMessage>>
{

    public Text Text;
    public RawImage Image;
    public Text CharacterName;

    string m_uriToShow;

    public override void DidShow(ResourcePanelData<CharacterMessage> data)
    {
        var character = CharacterDirectory.Instance.GetCharacter(data.Resource.CharacterId);

        if (character != null && character.ProfileImage != null)
        {
            m_uriToShow = character.ProfileImage.Url;

            CharacterName.text = character.Alias;
        }

        Text.text = data.Resource.Text;

        if (data.Resource.MediaItem != null &&
            data.Resource.MediaItem.MediaType == Motive.Core.Media.MediaType.Audio)
        {
            var localUrl =
                WebServices.Instance.MediaDownloadManager.GetPathForItem(data.Resource.MediaItem.Url);

            UnityAudioPlayerChannel.Instance.Play(new Uri(localUrl));
        }
    }

    public override void DidHide()
    {
        Text.text = null;
        Image.texture = null;

        base.DidHide();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (m_uriToShow != null)
        {
            StopAllCoroutines();

            StartCoroutine(ImageLoader.LoadImage(m_uriToShow, Image));
            m_uriToShow = null;
        }	
	}
}
