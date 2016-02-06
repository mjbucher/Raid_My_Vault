using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Motive.Core.Diagnostics;
using Motive.AR.LocationServices;
using Motive.Core.Timing;
using Motive.AR.Social;
using Motive.Core.Social;

public class SelectedLocationPanel : Panel<MapAnnotation> {

    public Text Title;
    public Text Subtitle;
    public Text Distance;
    public Text Description;
    public Text TimeLeft;
    public Button Button;

    MapAnnotation m_mapAnnotation;
    ValuablesCollection m_valuables;
    LocationFence m_fence;
    LocationTaskDriver m_driver;

    public override void DidShow(MapAnnotation data)
    {
        if (m_fence != null)
        {
            m_fence.StopWatching();
            m_fence = null;
        }

        m_mapAnnotation = data;
        bool showButton = false;
        string buttonText = "";

        if (data.LocationTask != null)
        {
            Title.text = data.LocationTask.Title;

            var charId = data.LocationTask.CharacterId;
            string charName = null;

            if (charId != null)
            {
                var character = CharacterDirectory.Instance.GetCharacter(charId);

                if (character != null)
                {
                    charName = character.Alias;
                }
            }

            if (charName != null)
            {
                Subtitle.text = charName;

                if (!string.IsNullOrEmpty(data.Location.Name))
                {
                    Subtitle.text += " at " + data.Location.Name;
                }
            }
            else
            {
                Subtitle.text = data.Location.Name;
            }

            m_driver = TaskManager.Instance.GetDriver(data.LocationTask) as LocationTaskDriver;
        }
        else
        {
            m_driver = null;
            Title.text = data.Location.Name;
            Subtitle.text = data.Location.Subtitle;
        }

        if (data.LocationTask != null)
        {
            Description.text = data.LocationTask.Description;

            if (data.LocationTask.Action == TaskAction.Take)
            {
                showButton = true;
                buttonText = "Take Items";
            }
            else if (data.LocationTask.Action == TaskAction.Query)
            {
                showButton = true;
                buttonText = "Access Database";
            }
        }
        else
        {
            buttonText = "Collect";

            string description = "";

            m_valuables = LocationTreasureChestManager.Instance.GetValuablesForLocation(data.Location);
            var sets = CollectibleDirectory.Instance.GetCollectibleSets(m_valuables);

            if (sets != null)
            {
                foreach (var s in sets)
                {
                    showButton = true;
                    description += string.Format("{0} {1}\n",
                        s.Count, s.Collectible.Title);
                }
            }

            Description.text = description;
        }

        Button.gameObject.SetActive(showButton);
        Button.interactable = true;

        if (showButton)
        {
            Button.GetComponentInChildren<Text>().text = buttonText;
        }

        bool setFence = true;
        double minRange = 0;
        double maxRange = 100;

        if (m_driver != null)
        {
            if (m_driver.Task.ActionRange != null)
            {
                minRange = m_driver.Task.ActionRange.Min.GetValueOrDefault(0);
                maxRange = m_driver.Task.ActionRange.Max.GetValueOrDefault(double.MaxValue);
            }
            else
            {
                setFence = false;
            }
        }

        if (setFence)
        {
            m_fence = LocationFence.Watch(
                SystemPositionService.Instance.LocationTracker,
                m_mapAnnotation.Location,
                minRange, maxRange, (locations) =>
                {
                    Button.interactable = true;
                },
                () =>
                {
                    Button.interactable = false;
                }
            );
        }

        base.DidShow(data);
    }

    public override void DidHide()
    {
        if (m_fence != null)
        {
            m_fence.StopWatching();
            m_fence = null;
        }
    }

    void Update()
    {
        var d = SystemPositionService.Instance.Position.GetDistanceFrom(m_mapAnnotation.Location.Coordinates);

        Distance.text = string.Format("{0:0}m", d);

        if (m_driver != null && m_driver.TimeoutTimer != null)
        {
            TimeLeft.gameObject.SetActive(true);

            var dt = m_driver.TimeoutTimer.FireTime - ClockManager.Instance.Now;

            TimeLeft.text = string.Format("{0:00}:{1:00}:{2:00}", dt.Hours, dt.Minutes, dt.Seconds);
        }
        else
        {
            TimeLeft.gameObject.SetActive(false);
        }
    }

    public void Action()
    {
        if (m_driver != null)
        {
            m_driver.Action();
        }
        else if (m_valuables != null)
        {
            UserActionDriver.Instance.Post("collect");

            Inventory.Instance.Add(m_valuables.CollectibleCounts);

            // In a real implementation, we could implement a feature where these
            // locations would re-spawn after a while.
            MapController.Instance.MapView.RemoveAnnotation(m_mapAnnotation);
            MapController.Instance.SelectAnnotation(null);
        }
    }
}
