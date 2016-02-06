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

using Motive.Core.Json;
using Motive.Core.Scripting;
using Motive.Core.Globalization;
using Motive.Core.Models;
using Motive.Core.Media;

public class CharacterMessageResponse : TextMediaResponse
{

}

public class CharacterMessage : TextMediaContent
{
    public ObjectReference CharacterReference { get; set; }
    public CharacterMessageResponse[] Responses { get; set; }

    public string CharacterId
    {
        get
        {
            if (CharacterReference != null)
            {
                return CharacterReference.ObjectId;
            }

            return null;
        }
    }

    public CharacterMessage ()
    {
    }

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