using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class CharacterTaskProcessor : ThreadSafeScriptResourceProcessor<CharacterTask> {

    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, CharacterTask resource)
    {
        if (!context.IsClosed)
        {
            TaskManager.Instance.ActivateCharacterTask(context, resource);
        }
    }

    public override void DeactivateResource(Motive.Core.Scripting.FrameContext context, CharacterTask resource)
    {
        TaskManager.Instance.DeactivateTask(resource);
    }
}
