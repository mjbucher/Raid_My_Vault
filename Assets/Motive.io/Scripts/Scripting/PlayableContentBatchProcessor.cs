using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class PlayableContentBatchProcessor : ThreadSafeScriptResourceProcessor<PlayableContentBatch> {

    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, PlayableContentBatch resource)
    {
        if (!context.IsClosed)
        {
            ContentPlayer.Instance.Play(context, resource);
        }
    }

    public override void DeactivateResource(Motive.Core.Scripting.FrameContext context, PlayableContentBatch resource)
    {
        ContentPlayer.Instance.StopPlaying(resource.Id);
    }
}
