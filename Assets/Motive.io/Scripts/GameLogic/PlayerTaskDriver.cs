using Motive.AR.Social;
using Motive.Core.Scripting;
using Motive.Core.Social;
using Motive.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IPlayerTaskDriver
{
    PlayerTask Task { get; }
    ResourceActivationContext ActivationContext { get; }
    Timer TimeoutTimer { get; }
    void Start();
    void Stop();
}

public class PlayerTaskDriver<T> : IPlayerTaskDriver
    where T : PlayerTask
{
    public T Task { get; private set; }
    public ResourceActivationContext ActivationContext { get; private set; }
    public Timer TimeoutTimer { get; private set; }

    public PlayerTaskDriver(ResourceActivationContext context, T task)
    {
        ActivationContext = context;
        Task = task;
    }

    public virtual void Action()
    {
        if (!string.IsNullOrEmpty(Task.Action))
        {
            ActivationContext.FireEvent(Task.Action);

            UserActionDriver.Instance.Post(Task.Action);

            switch (Task.Action)
            {
                case TaskAction.Take:
                    if (Task.ActionItems != null)
                    {
                        Inventory.Instance.Add(Task.ActionItems.CollectibleCounts);

                        TaskManager.Instance.ClosePlayerTask(Task);
                    }

                    break;
                case TaskAction.Give:
                    if (Task.ActionItems != null)
                    {
                        Inventory.Instance.Remove(Task.ActionItems.CollectibleCounts);

                        TaskManager.Instance.ClosePlayerTask(Task);
                    }

                    break;
            }
        }
    }

    public virtual void Start()
    {
        TimeoutTimer = ActivationContext.StartTimeoutTimer(Task.Timeout);
    }

    public virtual void Stop()
    {
        if (TimeoutTimer != null)
        {
            TimeoutTimer.Cancel();
            TimeoutTimer = null;
        }
    }

    PlayerTask IPlayerTaskDriver.Task
    {
        get { return Task; }
    }
}
