using UnityEngine;
using System.Collections;
using Motive.Core.Utilities;
using Motive.Core.Social;
using Motive.AR.Social;
using System;
using Motive.Core.WebServices;

public class UserActionDriver : Singleton<UserActionDriver> {
    public void Post(string action, Action<bool> onComplete = null)
    {
        if (SystemPositionService.Instance.HasLocationData &&
            MotiveAuthenticator.Instance.IsUserAuthenticated)
        {
            UserActivityService.Instance.Post(
                new UserLocationAction(SystemPositionService.Instance.Position, action),
                onComplete);
        }
    }
}
