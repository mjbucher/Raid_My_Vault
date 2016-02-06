using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class PlayableContentProcessor : ThreadSafeScriptResourceProcessor<PlayableContent> {

    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, PlayableContent resource)
    {
        ContentPlayer.Instance.Play(context, resource);
    }

    public override void DeactivateResource(Motive.Core.Scripting.FrameContext context, PlayableContent resource)
    {
        ContentPlayer.Instance.StopPlaying(resource.Id);
    }
}
