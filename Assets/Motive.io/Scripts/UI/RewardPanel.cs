using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RewardPanel : Panel<PlayerReward> {
    public Text DescriptionText;
    public override void DidShow(PlayerReward data)
    {
        var text = "";

        if (data.Reward != null)
        {
            Inventory.Instance.Add(data.Reward.CollectibleCounts);

            if (data.Reward.CollectibleCounts != null)
            {
                foreach (var cc in data.Reward.CollectibleCounts)
                {
                    var collectible = CollectibleDirectory.Instance.GetCollectible(cc.CollectibleId);

                    if (collectible != null)
                    {
                        text += string.Format("{0} {1}\n", cc.Count, collectible.Title);
                    }
                }
            }
        }

        DescriptionText.text = text;

        base.DidShow(data);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
