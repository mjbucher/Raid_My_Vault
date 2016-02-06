using UnityEngine;
using System.Collections;
using Motive.Core.Scripting;
using UnityEngine.UI;

public class ScreenDialogPanel : Panel<ResourcePanelData<ScreenMessage>> {
    public TablePanel TablePanel;
    public Text Text;
    public ScreenDialogResponseItem ResponseItem;

    void Awake()
    {
        TablePanel = GetComponentInChildren<TablePanel>();
    }

    public override void DidShow(ResourcePanelData<ScreenMessage> data)
    {
        TablePanel.Clear();

        Text.text = data.Resource.Text;

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
