using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PanelManager : SingletonComponent<PanelManager> {

    Dictionary<string, Panel> m_panelDict;
    Panel m_currPanel;

    protected override void Awake()
    {
        base.Awake();

		// Set the frame rate to 60
		Application.targetFrameRate = 60;

        var allPanels = GetComponentsInChildren<Panel>(true);

        m_panelDict = new Dictionary<string, Panel>();

        foreach (var panel in allPanels)
        {
            panel.gameObject.SetActive(false);

            if (!panel.PrePositioned)
            {
                panel.transform.localPosition = Vector3.zero;
            }
            m_panelDict[panel.GetType().Name] = panel;
        }
    }

    public void Hide(Panel p)
    {
        p.gameObject.SetActive(false);

        if (p == m_currPanel)
        {
            m_currPanel = null;
        }

        var onClose = p.OnClose;
        p.OnClose = null;

        if (onClose != null)
        {
            onClose();
        }

        // We might be showing the same panel,
        // so don't tell it that it hid.
        if (p != m_currPanel)
        {
            p.DidHide();
        }
    }

    void ShowPanel(Panel p, object data, Action onClose)
    {
        p.OnClose = onClose;
        p.gameObject.SetActive(true);
        p.DidShow(data);

        m_currPanel = p;
    }

    public T Show<T>(object data = null, Action onClose = null) where T : Panel
    {
        Panel p = null;

        if (m_panelDict.TryGetValue(typeof(T).Name, out p))
        {
            if (m_currPanel)
            {
                if (m_currPanel.WaitForUserHide)
                {
                    var call = m_currPanel.OnClose;

                    m_currPanel.OnClose = () =>
                    {
                        if (call != null)
                        {
                            call();
                        }

                        ShowPanel(p, data, onClose);
                    };
                }
                else
                {
                    Hide(m_currPanel);

                    ShowPanel(p, data, onClose);
                }
            }
            else
            {
                ShowPanel(p, data, onClose);
            }
        }

        return p as T;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    internal void HideAll()
    {
        if (m_currPanel != null)
        {
            Hide(m_currPanel);
        }
    }
}
