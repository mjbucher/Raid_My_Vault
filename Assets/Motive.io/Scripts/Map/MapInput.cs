using System;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Motive.Core.Utilities;

/// <summary>
/// Calculate swipe and zoom for the map based on multi-touch.
/// </summary>
public class MapInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler  {

    private float m_lastPinchMagnitude;

    private bool m_pointerDown;

	private Vector2 m_lastTranslatePos;
	private Vector2 m_translateVelocity;
	private Vector2 m_translation;

	private float TranslateDamp = 0.33f;
	private float TranslateEpsilon = 1.0f;

	static double m_log2 = Math.Log(2);

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
        Debug.Log("*****Press SPACE to simulate a pinch between the mouse and the screen center.*****");
#endif
	}

    public bool IsTranslating
    {
		get; private set;
    }

    public Vector2 Translation
    {
        get {
            if (IsTranslating)
            {
				return m_translation;
            }

            return new Vector2(0, 0);
        }
    }

    public bool IsPinching { get; private set; }

    public double ZoomDelta
    {
		get; set;
    }

	Vector2 GetVTranslate()
	{
		Vector2 pos = Input.mousePosition;

		var trans = pos - m_lastTranslatePos;
		m_lastTranslatePos = pos;

		return trans / Time.smoothDeltaTime;
	}

	// Update is called once per frame
    void Update()
    {
        bool pinching = false;

        if (Application.isMobilePlatform)
        {
            pinching = Input.touchCount > 1;
        }
        else
        {
            pinching = m_pointerDown && (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftShift));
        }
		
		if (pinching)
		{
			IsTranslating = false;

			Vector2 touch1Pos;
			Vector2 touch2Pos;

			if (Application.isMobilePlatform)
			{
				touch1Pos = Input.touches[0].position;
				touch2Pos = Input.touches[1].position;
			}
			else
			{
				touch1Pos = Input.mousePosition;
				touch2Pos = new Vector2(Screen.width / 2, Screen.height / 2);
			}

			Vector2 currentPinch = (touch1Pos - touch2Pos);

			if (!IsPinching)
			{
				IsPinching = true;
			}
			else
			{
				var pinchFactor = (currentPinch.magnitude / m_lastPinchMagnitude);
				ZoomDelta = Math.Log(pinchFactor) / m_log2;
			}

			m_lastPinchMagnitude = currentPinch.magnitude;
		}
		else
		{
			if (IsPinching)
			{
				PinchEnded();
			}
		}

		if (!pinching && m_pointerDown)
		{
			if (!IsTranslating)
			{
				m_lastTranslatePos = Input.mousePosition;
			}

			IsTranslating = true;

			var lastV = m_translateVelocity;
			var currV = GetVTranslate();

			m_translateVelocity = ((lastV + currV * 3) / 4);

			m_translation = m_translateVelocity * Time.smoothDeltaTime;
		}
		else
		{
			if (m_translateVelocity.sqrMagnitude > TranslateEpsilon)
			{
				var vx = MathHelper.Approach(m_translateVelocity.x, 0, TranslateDamp, Time.deltaTime);
				var vy = MathHelper.Approach(m_translateVelocity.y, 0, TranslateDamp, Time.deltaTime);

				m_translateVelocity = new Vector2(vx, vy);

				m_translation = m_translateVelocity * Time.deltaTime;
			}
			else
			{
				IsTranslating = false;
			}
		}

    }

	public void CancelPan()
	{
		IsTranslating = false;
	}

    void PinchEnded()
    {
        IsPinching = false;
		ZoomDelta = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_pointerDown = true;
		m_lastTranslatePos = Input.mousePosition;
		m_translateVelocity = Vector2.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PinchEnded();
        m_pointerDown = false;
    }
}
