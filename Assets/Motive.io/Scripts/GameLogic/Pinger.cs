using UnityEngine;
using System.Collections;
using Motive.Core.WebServices;
using System;
using Motive.Core.Social;
using Motive.AR.Social;

public class Pinger : MonoBehaviour {

    public int Interval;

    DateTime m_lastPing;

	// Use this for initialization
	void Start () {
        m_lastPing = DateTime.Now.AddSeconds(-Interval);

	    if (MotiveAuthenticator.Instance.IsUserAuthenticated &&
            SystemPositionService.Instance.Position != null)
        {
            Ping();
        }
	}

    private void Ping()
    {
        m_lastPing = DateTime.Now;

        UserActionDriver.Instance.Post("ping");
    }
	
	// Update is called once per frame
	void Update () {
        if (MotiveAuthenticator.Instance.IsUserAuthenticated &&
            SystemPositionService.Instance.Position != null)
        {
            if (DateTime.Now >= m_lastPing.AddSeconds(Interval))
            {
                Ping();
            }
        }
	}
}
