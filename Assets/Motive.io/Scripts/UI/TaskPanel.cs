using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TaskPanel : Panel {

    public LocationTaskItem LocationTaskItem;
    public CharacterTaskItem CharacterTaskItem;
    public GameObject PlayerTaskItem;
    public GameObject ItemsPanel;
    public TablePanel TablePanel;

    void Awake()
    {
        if (!TablePanel)
        {
            TablePanel = GetComponentInChildren<TablePanel>();
        }
    }

    private T AddPrefab<T>(T prefab, PlayerTask task) where T : PlayerTaskItem
    {
        var item = TablePanel.AddItem<T>(prefab);

        item.Title.text = task.Title;
        item.Description.text = task.Description;
        item.CompleteButton.interactable = TaskManager.Instance.CanComplete(task);

        if (task.Action == TaskAction.Give)
        {
            // Overwrite the description with info about what we need to give
            var sets = CollectibleDirectory.Instance.GetCollectibleSets(task.ActionItems);

            if (sets != null)
            {
                string text = "You need: ";

                bool first = true;

                foreach (var set in sets)
                {
                    text += string.Format("{0} {1} {2}",
                        first ? "" : ",", set.Count, set.Collectible.Title);
                }

                item.Description.text = text;
            }
        }

        if (task.ImageUrl != null)
        {
            StartCoroutine(ImageLoader.LoadImage(task.ImageUrl, item.Image));
        }

        return item;
    }

    public override void DidShow(object data)
    {
        TablePanel.Clear();

        var drivers = TaskManager.Instance.ActiveTaskDrivers;

        foreach (var driver in drivers)
        {
            var task = driver.Task;

            if (task is LocationTask)
            {
                var ltask = task as LocationTask;

                var item = AddPrefab<LocationTaskItem>(LocationTaskItem, task);

                item.Driver = driver as LocationTaskDriver;

                if (item.Driver.Task.IsHidden)
                {
                    item.CompleteButton.gameObject.SetActive(false);
                }
                else if (item.Driver.Task.Locations != null && item.Driver.Task.Locations.Length > 0)
                {
                    item.CompleteButton.gameObject.SetActive(true);

                    item.CompleteButton.GetComponentInChildren<Text>().text =
                        item.Driver.Task.Locations[0].Name;
                }

                if (ltask.ImageUrl == null)
                {
                    // We can use the character image instead

                    var character = CharacterDirectory.Instance.GetCharacter(ltask.CharacterId);

                    if (character != null && character.ImageUrl != null)
                    {
                        StartCoroutine(ImageLoader.LoadImage(character.ImageUrl, item.Image));
                    }
                }
            }
            else if (driver.Task is CharacterTask)
            {
                var ctask = task as CharacterTask;

                var item = AddPrefab<CharacterTaskItem>(CharacterTaskItem, ctask);

                item.Driver = driver as PlayerTaskDriver<CharacterTask>;

                if (ctask.ImageUrl == null)
                {
                    // We can use the character image instead

                    var character = CharacterDirectory.Instance.GetCharacter(ctask.CharacterId);

                    if (character != null && character.ImageUrl != null)
                    {
                        StartCoroutine(ImageLoader.LoadImage(character.ImageUrl, item.Image));
                    }
                }
            }
        }
        base.DidShow(data);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
