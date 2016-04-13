using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class UISizer : MonoBehaviour
{
    public bool useParentSize;
    RectTransform parentRectTransform;
    public

	void Update ()
    {
        CheckControls();
	}

    void CheckControls ()
    {
        if (useParentSize)
        {
            SetSizeToParent();
        }
    }

    void SetSizeToParent ()
    {
        // get parent
        parentRectTransform = GetComponentInParent<RectTransform>();
        // set my size to parents
        RectTransform myRect = GetComponent<RectTransform>();
        myRect.rect.Set(parentRectTransform.rect.x, parentRectTransform.rect.y, parentRectTransform.rect.width, parentRectTransform.rect.height);
    }
}
