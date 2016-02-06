using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;
using Motive.AR.LocationServices;
using Motive.Core.Diagnostics;
using System.Linq;
using Motive.Core.Timing;

public class LocationTaskDriver : PlayerTaskDriver<LocationTask> {
    LocationTrigger m_trigger;

    public LocationTaskDriver(ResourceActivationContext context, LocationTask task)
        : base(context, task)
    {
    }

    public override void Start()
    {
        if (!ActivationContext.IsOpen)
        {
            return;
        }

        if (!Task.IsHidden)
        {
            MapController.Instance.AddTaskLocations(Task);
        }

        if (Task.Locations != null &&
            Task.Locations.Length > 0 &&
            Task.ActionRange != null)
        {
            m_trigger = LocationTrigger.Wait(
                Task.Locations,
                Task.ActionRange.Min.GetValueOrDefault(),
                Task.ActionRange.Max.GetValueOrDefault(),
                (inRangeLocations) =>
                {
                    if (Task.Action == TaskAction.InRange)
                    {
                        // That's it! Task is comlete.
                        TaskManager.Instance.ClosePlayerTask(Task);
                    }
                }
            );
        }

        base.Start();
    }

    public override void Stop()
    {
        MapController.Instance.RemoveTaskLocations(Task);

        if (m_trigger != null)
        {
            m_trigger.StopWaiting();
            m_trigger = null;
        }

        base.Stop();
    }
}
