using Motive.Core.Scripting;
using Motive.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AnnotationMarker : ScriptObject, IMediaItemProvider
{
    public string[] StoryTags { get; set; }
    public string[] LocationTypes { get; set; }
    public MediaElement Marker { get; set; }

    public void GetMediaItems(IList<Motive.Core.Media.MediaItem> items)
    {
        MediaElement.GetMediaItems(Marker, items);
    }
}
