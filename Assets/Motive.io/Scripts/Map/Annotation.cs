using UnityEngine;
using System.Collections;
using Motive.Unity.Maps;
using Motive.AR.LocationServices;
using UnityEngine.EventSystems;
using Motive.Core.Utilities;
using Motive.UI;

public class MapAnnotation : IAnnotation
{
    public Location Location { get; private set; }
    public LocationTask LocationTask {get; set;}
    public MediaElement Marker { get; set; }
    public MapAnnotation(Location location)
    {
        Location = location;
    }

    public Motive.AR.LocationServices.Coordinates Coordinates
    {
        get { return Location.Coordinates; }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MapController.Instance.SelectAnnotation(this);
    }
}

public class UserAnnotation : IAnnotation
{
    public Coordinates Coordinates
    {
        get; set;
    }

    public void UpdateCoordinates(Coordinates coords, float dt)
    {
        if (Coordinates == null)
        {
            Coordinates = coords;
        }
        else
        {
            Coordinates = Coordinates.Approach(coords, 0.1, dt);
        }
    }
}