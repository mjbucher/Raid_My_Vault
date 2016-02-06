using UnityEngine;
using System.Collections;
using Motive.AR.LocationServices;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class DebugPlayerLocation : MonoBehaviour {
    public enum DebugLocation
    {
        Vancouver,
        SanFrancisco,
        Paris,
        Berlin,
        London,
        Manhattan,
        Tokyo,
        Moscow,
        Nairobi,
        Shanghai,
        LosAngeles,
        Perth,
        Sydney,
        CapeTown,
		Philadelphia,
        //RocketChicken
        //RDS,
        //SwitchUnited
    }

    public DebugLocation PlayerLocation = DebugLocation.Vancouver;
    public bool KeyboardMovesPlayer;
    public float PlayerSpeed = 50f;

    public bool UseAnchorPosition = false;

    public MapController MapController;

    private DebugLocation m_currLocation;

    private Dictionary<DebugLocation, Coordinates> m_debugCoords;

	// Use this for initialization
	void Start () {
        // Some debug coordinates to get you started--add your own!
        m_debugCoords = new Dictionary<DebugLocation, Coordinates>()
        {
            { DebugLocation.Vancouver, new Coordinates(49.283056, -123.118675) },
            { DebugLocation.SanFrancisco, new Coordinates(37.787629, -122.406694) },
            { DebugLocation.Paris, new Coordinates(48.8567, 2.3508) },
            { DebugLocation.Berlin, new Coordinates(52.5167, 13.3833) },
            { DebugLocation.London, new Coordinates(51.5072, 0.1275) },
            { DebugLocation.Manhattan, new Coordinates(40.7903, -73.9597) },
            { DebugLocation.Tokyo, new Coordinates(35.6833, 139.6833) },
            { DebugLocation.Moscow, new Coordinates(55.7500, 37.6167) },
            { DebugLocation.Nairobi, new Coordinates(-1.2833, 36.8167) },
            { DebugLocation.Shanghai, new Coordinates(31.2000, 121.5000) },
            { DebugLocation.LosAngeles, new Coordinates(34.0500, -118.2500) },
            { DebugLocation.Perth, new Coordinates(-31.9522, 115.8589) },
            { DebugLocation.Sydney, new Coordinates(-33.8650, 151.2094) },
            { DebugLocation.CapeTown, new Coordinates(-33.9253, 18.4239) },
			{ DebugLocation.Philadelphia, new Coordinates(39.9500, -75.1667)},
            //{ DebugLocation.RocketChicken, new Coordinates(49.2746716, -123.0194646) },
            //{ DebugLocation.RDS, new Coordinates(54.35809, 18.61798) },
            //{ DebugLocation.SwitchUnited, new Coordinates(49.266,-123.103) },
        };

        WarpTo(PlayerLocation);
	}
	
    void WarpTo(DebugLocation loc)
    {
        if (!Application.isMobilePlatform)
        {
            if (m_debugCoords.ContainsKey(loc))
            {
                SystemPositionService.Instance.DebugSetPosition(m_debugCoords[loc]);
            }

            if (MapController)
            {
                MapController.CenterMap();
            }
        }

        m_currLocation = loc;

#if DEBUG
        if (UseAnchorPosition)
        {
            UserLocationService.Instance.AnchorPosition = m_debugCoords[loc];
        }
#endif
    }

	// Update is called once per frame
	void Update () {
	    if (!Application.isMobilePlatform)
        {
            if (m_currLocation != PlayerLocation)
            {
                WarpTo(PlayerLocation);
            }
        }

        if (KeyboardMovesPlayer)
        {
            Vector2 keyMove = Vector2.zero;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                keyMove.y += 1;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                keyMove.x += 1;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                keyMove.x -= 1;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                keyMove.y -= 1;
            }

            if (keyMove.magnitude > 0 && SystemPositionService.Instance.HasLocationData)
            {
                var rad = Mathf.Atan2(keyMove.x, keyMove.y);
                var heading = rad * 180.0 / Mathf.PI;

                var coords = SystemPositionService.Instance.Position;
                var newCoords = coords.AddRadial(heading, PlayerSpeed * Time.deltaTime);
                SystemPositionService.Instance.DebugSetPosition(newCoords);
            }
        }

	}
}
