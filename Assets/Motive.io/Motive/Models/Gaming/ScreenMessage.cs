using Motive.Core.Media;
using Motive.Core.Models;
using Motive.Core.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ScreenMessageResponse : TextMediaResponse
{

}

public class ScreenMessage : TextMediaContent
{
    public ScreenMessageResponse[] Responses { get; set; }

    public override void GetMediaItems(IList<MediaItem> items)
    {
        if (Responses != null)
        {
            foreach (var response in Responses)
            {
                response.GetMediaItems(items);
            }
        }

        base.GetMediaItems(items);
    }
}
