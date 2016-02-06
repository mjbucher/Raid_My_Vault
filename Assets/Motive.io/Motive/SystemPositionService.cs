using UnityEngine;
using System.Collections;
using Motive.AR.LocationServices;
using Motive.AR.Scripting;
using System;
using Motive.Core.Utilities;

public class SystemPositionService : SingletonComponent<SystemPositionService> {

    public LocationTracker LocationTracker { get; private set; }
    public ILocationManager LocationManager { get { return m_unityLocationManager; } }

    private UnityLocationManager m_unityLocationManager;

    public bool HasLocationData {get; private set;}

    public Coordinates Position { get; private set; }

    public event Action<Coordinates> PositionUpdated;

    protected override void Awake()
    {
        base.Awake();

        m_unityLocationManager = gameObject.AddComponent<UnityLocationManager>();
    }

    public void Initialize()
    {
        LocationTracker = UserLocationService.Instance.CreateLocationTracker();
        LocationTracker.Updated += LocationTracker_Updated;

        LocationTracker.Start();
    }

    void LocationTracker_Updated(LocationTracker sender, LocationReading reading)
    {
        SetSystemPosition(reading.Coordinates);
    }

    public void DebugSetPosition(Coordinates coords)
    {
        m_unityLocationManager.DebugSetPosition(coords);

        if (LocationTracker == null)
        {
            // If we don't have a location tracker yet, 
            // update the system position directly.
            SetSystemPosition(coords);
        }
    }

    private void SetSystemPosition(Coordinates coords)
    {
        if (coords != null)
        {
            Position = coords;

            HasLocationData = true;

            if (PositionUpdated != null)
            {
                PositionUpdated(coords);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
