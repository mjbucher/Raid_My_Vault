using UnityEngine;
using System.Collections;
using Motive.Unity.Scripting;

public class PlayerRewardProcessor : ThreadSafeScriptResourceProcessor<PlayerReward> 
{
    public override void ActivateResource(Motive.Core.Scripting.ResourceActivationContext context, PlayerReward resource)
    {
        if (!context.IsClosed)
        {
            PanelManager.Instance.Show<RewardPanel>(resource);
            context.Close();
        }
    }
}
