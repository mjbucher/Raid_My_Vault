using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;
using Motive.AR.Scripting;
using Motive.Core.Json;
using Motive.AR.LocationServices;

public class ScriptExtensions {

    public static void Initialize(ILocationManager locationManager)
    {
        // This code bootstraps the Motive script engine. First, give the script engine
        // a path that it can use for storing state.
        var smPath = StorageManager.EnsureGameFolder("scriptManager");

        ScriptEngine.Instance.Initialize(smPath);

        // This intializes the Alternate Reality and Location Based features.
        ARComponents.Instance.Initialize(locationManager);

        // This tells the JSON reader how to deserialize various object types based on
        // the "type" field.
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.characterTask", typeof(CharacterTask));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.playableContent", typeof(PlayableContent));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.playableContentBatch", typeof(PlayableContentBatch));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.characterMessage", typeof(CharacterMessage));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.screenMessage", typeof(ScreenMessage));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.inventoryCollectibles", typeof(InventoryCollectibles));
        JsonTypeRegistry.Instance.RegisterType("motive.gaming.playerReward", typeof(PlayerReward));

        JsonTypeRegistry.Instance.RegisterType("motive.gaming.inventoryCondition", typeof(InventoryCondition));

        JsonTypeRegistry.Instance.RegisterType("motive.ar.locationTask", typeof(LocationTask));
        JsonTypeRegistry.Instance.RegisterType("motive.ar.locationMarker", typeof(LocationMarker));

        // The Script Resource Processors take the resources from the script processor and
        // direct them to the game components that know what to do with them.
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.core.scriptLauncher", new ScriptLauncherProcessor());

        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.gaming.characterTask", new CharacterTaskProcessor());
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.gaming.inventoryCollectibles", new InventoryCollectiblesProcessor());
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.gaming.playableContent", new PlayableContentProcessor());
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.gaming.playableContentBatch", new PlayableContentBatchProcessor());
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.gaming.playerReward", new PlayerRewardProcessor());

        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.ar.locationTask", new LocationTaskProcessor());
        ScriptEngine.Instance.RegisterScriptResourceProcessor("motive.ar.locationMarker", new LocationMarkerProcessor());

        // Register a condition monitor that knows how to handle inventory conditions.
        ScriptEngine.Instance.RegisterConditionMonitor(new InventoryConditionMonitor());
    }
}
