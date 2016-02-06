using UnityEngine;
using System.Collections;
using Motive.Core.Models;
using Motive.Core.Scripting;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using Motive.Core.Utilities;
using Motive.Core.Globalization;

/// <summary>
/// The Script Manager processes scripts downloaded from the server.
/// This Script Manager uses a straightforward policy: execute any
/// script named "Main."
/// </summary>
public class ScriptManager : SingletonComponent<ScriptManager>
{

    Dictionary<string, ScriptProcessor> m_runningProcessors;
    Dictionary<string, Script> m_allScripts;
    Catalog<Script> m_catalog;

    public event EventHandler ScriptsReset;

    protected override void Awake()
    {
        base.Awake();

        m_runningProcessors = new Dictionary<string, ScriptProcessor>();
    }

    public void StopAllProcessors(Action onComplete)
    {
        var running = m_runningProcessors.Values.ToArray();
        m_runningProcessors.Clear();

        BatchProcessor.Process(running, (proc, onStop) =>
        {
            proc.StopRunning(onStop);
        }, onComplete);
    }
    public void AbortAllProcessors()
    {
        var running = m_runningProcessors.Values.ToArray();
        m_runningProcessors.Clear();

        foreach (var proc in running)
        {
            proc.Abort();
        }
    }

    public void RunScripts()
    {
        if (m_catalog == null)
        {
            throw new ArgumentException("Must set catalog before calling RunScripts");
        }

        m_allScripts = new Dictionary<string, Script>();

        List<Script> toRun = new List<Script>();

        foreach (var script in m_catalog)
        {
            m_allScripts[script.Id] = script;

            if (script.Name.ToLower() == "main")
            {
                toRun.Add(script);
            }
        }

        // Start the soundtrack after all scripts have been launched
        // to make sure only the currently active soundtrack song gets
        // launched.
        BatchProcessor iter = new BatchProcessor(toRun.Count, AudioContentPlayer.Instance.StartSoundtrack);

        foreach (var script in toRun)
        {
            LaunchScript(script, () => { iter++; }, null);
        }

    }

    public void RunScriptCatalog(Catalog<Script> catalog)
    {
        //StopAllProcessors(null);

        m_catalog = catalog;

        RunScripts();
    }

    public void LaunchScript(Script script, Action onRunning, Action onComplete)
    {
        if (m_runningProcessors.ContainsKey(script.Id))
        {
            // Can only run one instance at a time with this implementation.
            // It is entirely possible to run the same script with multiple
            // script processors as long as they have different state files.
            if (onComplete != null)
            {
                onComplete();
            }

            return;
        }

        // Store script state in its own directory so we can reset all
        // scripts easily.
        var stateFile = StorageManager.GetGameFileName("scriptManager", script.Id + ".json");

        var proc = new ScriptProcessor(script, stateFile);

        m_runningProcessors[script.Id] = proc;

        proc.Run(onRunning, onComplete);
    }

    public void Abort()
    {
        AbortAllProcessors();
    }

    public void Reset(Action onComplete)
    {
        StopAllProcessors(() =>
        {
            if (ScriptsReset != null)
            {
                ScriptsReset(this, EventArgs.Empty);
            }

            ScriptEngine.Instance.Reset();
            StorageManager.DeleteGameFolder();

            if (onComplete != null)
            {
                onComplete();
            }
        });
    }

    public void LaunchScript(string id, Action onComplete)
    {
        if (m_allScripts.ContainsKey(id))
        {
            LaunchScript(m_allScripts[id], null, onComplete);
        }
        else
        {
            if (onComplete != null)
            {
                onComplete();
            }
        }
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
