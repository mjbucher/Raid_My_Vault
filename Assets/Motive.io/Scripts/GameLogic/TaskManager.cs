using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;
using System.Collections.Generic;
using Motive.Core.Scripting;
using System.Linq;
using System;

public static class TaskAction
{
    public const string InRange = "in_range";
    public const string Give = "give";
    public const string Take = "take";
    public const string Query = "query";
}

class TaskContainer
{
    public PlayerTask Task { get; set; }
    public ResourceActivationContext Context { get; set; }
}

public class TaskManager : Singleton<TaskManager> {
    Dictionary<string, IPlayerTaskDriver> m_drivers;

    public event EventHandler Updated;

    public IEnumerable<IPlayerTaskDriver> AllTaskDrivers
    {
        get
        {
            return m_drivers.Values.ToArray();
        }
    }

    public IEnumerable<IPlayerTaskDriver> ActiveTaskDrivers
    {
        get
        {
            return m_drivers.Values.Where(t => !t.ActivationContext.IsClosed).ToArray();
        }
    }

    public TaskManager()
    {
        m_drivers = new Dictionary<string, IPlayerTaskDriver>();
        ScriptManager.Instance.ScriptsReset += Instance_ScriptsReset;
    }

    void Instance_ScriptsReset(object sender, EventArgs e)
    {
        m_drivers.Clear();

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }
    }

    public bool CanComplete(PlayerTask task)
    {
        if (task.Action == TaskAction.Give)
        {
            if (task.ActionItems != null)
            {
                if (task.ActionItems.CollectibleCounts != null)
                {
                    bool isMet = true;

                    foreach (var cc in task.ActionItems.CollectibleCounts)
                    {
                        var count = Inventory.Instance.GetCount(cc.CollectibleId);

                        if (count < cc.Count)
                        {
                            isMet = false;
                            break;
                        }
                    }

                    if (!isMet)
                    {
                        return isMet;
                    }
                }
            }
        }

        // Other actions can be completed?
        return true;
    }

    public void Complete(PlayerTask task)
    {
        if (CanComplete(task))
        {
            if (task.Action == TaskAction.Give)
            {
                if (task.ActionItems != null)
                {
                    Inventory.Instance.Remove(task.ActionItems.CollectibleCounts);
                }
            }
            else if (task.Action == TaskAction.Take)
            {
                if (task.ActionItems != null)
                {
                    Inventory.Instance.Add(task.ActionItems.CollectibleCounts);
                }
            }

            if (Updated != null)
            {
                Updated(this, EventArgs.Empty);
            }

            ClosePlayerTask(task);
        }
    }

    public IPlayerTaskDriver GetDriver(PlayerTask task)
    {
        if (m_drivers.ContainsKey(task.Id))
        {
            return m_drivers[task.Id];
        }

        return null;
    }

    public void ClosePlayerTask(PlayerTask task)
    {
        var driver = GetDriver(task);

        if (driver != null)
        {
            driver.ActivationContext.Close();
        }

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }

        DeactivateTask(task);
    }

    public void ActivatePlayerTaskDriver(ResourceActivationContext context, IPlayerTaskDriver driver)
    {
        if (!context.IsClosed)
        {
            context.Open();

            m_drivers[driver.Task.Id] = driver;

            driver.Start();
        }

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }
    }

    public void ActivateLocationTask(ResourceActivationContext context, LocationTask task)
    {
        var driver = new LocationTaskDriver(context, task);

        ActivatePlayerTaskDriver(context, driver);
    }

    public void ActivateCharacterTask(ResourceActivationContext context, CharacterTask task)
    {
        ActivatePlayerTaskDriver(context, new PlayerTaskDriver<CharacterTask>(context, task));
    }

    public void DeactivateTask(PlayerTask task)
    {
        if (m_drivers.ContainsKey(task.Id))
        {
            var driver = m_drivers[task.Id];
            m_drivers.Remove(task.Id);

            driver.Stop();
        }

        if (Updated != null)
        {
            Updated(this, EventArgs.Empty);
        }
    }
}
