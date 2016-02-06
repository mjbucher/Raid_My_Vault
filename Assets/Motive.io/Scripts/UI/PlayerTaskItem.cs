using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Motive.Core.Timing;
using System;

public class PlayerTaskItem : MonoBehaviour
{
    public Text Title;
    public Text Description;
    public RawImage Image;
    public Button CompleteButton;
    public Text TimeLeft;
}
public class PlayerTaskItem<T> : PlayerTaskItem where T : PlayerTask {

    public PlayerTaskDriver<T> Driver { get; set; }

    public void CompleteTask()
    {
        TaskManager.Instance.Complete(Driver.Task);
    }

    protected virtual void Update()
    {
        if (Driver != null && Driver.TimeoutTimer != null)
        {
            TimeLeft.gameObject.SetActive(true);

            TimeSpan dt = Driver.TimeoutTimer.FireTime - ClockManager.Instance.Now;

            TimeLeft.text = string.Format("{0:00}:{1:00}:{2:00}", dt.Hours, dt.Minutes, dt.Seconds);
        }
        else
        {
            TimeLeft.gameObject.SetActive(false);
        }
    }
}
