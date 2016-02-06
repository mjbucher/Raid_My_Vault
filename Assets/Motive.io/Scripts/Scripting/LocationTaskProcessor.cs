using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class LocationTaskProcessor : ThreadSafeScriptResourceProcessor<LocationTask> {
    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, LocationTask resource)
    {
        if (!context.IsClosed)
        {
            TaskManager.Instance.ActivateLocationTask(context, resource);
        }
    }

    public override void DeactivateResource(Motive.Core.Scripting.FrameContext context, LocationTask resource)
    {
        TaskManager.Instance.DeactivateTask(resource);
    }
}
