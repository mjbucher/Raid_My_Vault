//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Motive.Core.Json;
using Motive.Core.Scripting;
using Motive.Core.Globalization;
using Motive.Core.Models;

public static class PlayerTaskAttributes
{
    public const string Hidden = "hidden";
}

public abstract class PlayerTask : ScriptResource, IMediaItemProvider
{
    public string Title { get { return LocalizedText.GetText(LocalizedTitle); } }
    public string Description { get; set; }
    public ValuablesCollection ActionItems { get; set; }
    public ValuablesCollection Reward { get; set; }
    public string Action { get; set; }
    public string[] Attributes { get; set; }
    public ScriptTimer Timeout { get; set; }

    public LocalizedText LocalizedTitle { get; set; }
    public LocalizedMedia LocalizedImage { get; set; }

    public string ImageUrl
    {
        get
        {
            var item = LocalizedMedia.GetMediaItem(LocalizedImage);

            if (item != null)
            {
                return item.Url;
            }

            return null;
        }
    }

    public bool HasAttribute(string attribute)
    {
        if (Attributes != null) {
            return Attributes.Contains(attribute);
        }

        return false;
    }

    public PlayerTask (string resourceType) : base(resourceType)
    {
    }

    public virtual void GetMediaItems(IList<Motive.Core.Media.MediaItem> items)
    {
        if (LocalizedImage != null)
        {
            LocalizedImage.GetMediaItems(items);
        }
    }
}
