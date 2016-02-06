using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingPanel : Panel {

    public Text StatusText;
    public Text DownloadFilesText;
    public Text DownloadSizeText;

    public override void DidShow(object data)
    {
        DownloadFilesText.gameObject.SetActive(false);
        DownloadSizeText.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
