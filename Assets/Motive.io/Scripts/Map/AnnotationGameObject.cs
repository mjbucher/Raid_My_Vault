using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AnnotationGameObject : MonoBehaviour, IPointerClickHandler  {

    public MapAnnotation Annotation { get; set; }

    private Vector3 m_scale;
    private Vector3 m_selectedScale;

	// Use this for initialization
	void Start () {
        m_scale = transform.localScale;
        m_selectedScale = m_scale * 2;
	}
	
	// Update is called once per frame
	void Update () {
	    if (MapController.Instance.SelectedAnnotation == Annotation)
        {
            transform.localScale = m_selectedScale;
        }
        else
        {
            transform.localScale = m_scale;
        }
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        MapController.Instance.SelectAnnotation(Annotation);
    }
}
