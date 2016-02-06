using UnityEngine;
using System.Collections;

public class CharacterDialogPanel : CharacterMessagePanel
{
    public CharacterDialogResponseItem ResponseItem;
    public GameObject ItemsPanel;

    public TablePanel TablePanel;

    void Awake()
    {
        TablePanel = GetComponentInChildren<TablePanel>();
    }

    public override void DidShow(ResourcePanelData<CharacterMessage> data)
    {
        TablePanel.Clear();

        foreach (var response in data.Resource.Responses)
        {
            var item = TablePanel.AddItem(ResponseItem);

            item.Populate(response);

            var evtid = response.Event;

            item.Button.onClick.AddListener(() =>
            {
                if (evtid != null)
                {
                    data.ActivationContext.FireEvent(evtid);
                }

                Back();
            });
        }

        base.DidShow(data);
    }
}
