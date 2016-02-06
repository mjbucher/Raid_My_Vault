using UnityEngine;
using System.Collections;
using Motive.Core.Models;
using Motive.Unity.Scripting;

public class ScriptLauncherProcessor : ThreadSafeScriptResourceProcessor<ScriptLauncher> {

    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, ScriptLauncher resource)
    {
        if (!context.IsClosed && resource.ScriptReference != null)
        {
            ScriptManager.Instance.LaunchScript(resource.ScriptReference.ObjectId, () =>
            {
                context.Close();
            });
        }
    }
}
