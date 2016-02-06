using Motive.AR.LocationServices;
using Motive.Core.Scripting;
using Motive.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LocationMarker : ScriptObject, IMediaItemProvider
{
    public MediaElement Marker { get; set; }

    public Location[] Locations { get; set; }

    public void GetMediaItems(IList<Motive.Core.Media.MediaItem> items)
    {
        MediaElement.GetMediaItems(Marker, items);
    }
}
