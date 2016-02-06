using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Motive.Core.Scripting;

public class TextMediaResponseItem : MonoBehaviour
{
    public Text Text;
    public Button Button;
    public RawImage Image;

    public virtual void Populate(TextMediaResponse response)
    {
        Text.text = response.Text;

        if (Image && response.ImageUrl != null)
        {
            StartCoroutine(ImageLoader.LoadImage(response.ImageUrl, Image));
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
