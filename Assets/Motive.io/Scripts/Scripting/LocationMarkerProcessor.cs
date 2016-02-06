using Motive.Unity.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LocationMarkerProcessor : ThreadSafeScriptResourceProcessor<LocationMarker>
{
    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, LocationMarker resource)
    {
        MapController.Instance.AddLocationMarker(resource);

        base.ActivateResource(context, resource);
    }

    public override void DeactivateResource(Motive.Core.Scripting.FrameContext context, LocationMarker resource)
    {
        MapController.Instance.RemoveLocationMarker(resource);

        base.DeactivateResource(context, resource);
    }
}
