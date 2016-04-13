using UnityEngine;
using System.Collections;
using RMV;

[ExecuteInEditMode]
public class LevelEditorUIManager : MonoBehaviour
{
    GameMaster GM;
    public bool liveUpdate;
    [Header("Bottom Pop Up Controls")]
    public RectTransform InventoryPopUp;
    public Vector3 activePosition = Vector3.zero;
    public Vector3 inactivePosition = new Vector3(0, -480.0f, 0);
    public bool isActive;
    //[Header("Inventory Controls")]
    public InventoryCategory activeCategory = InventoryCategory.Room;

    public Transform InstantiationPoint;
    public GameObject RoomPrefab;


    void Update ()
    {
        if (liveUpdate)
            CheckControls();
    }

    void CheckControls ()
    {
        // check pop up position
        InventoryPopUp.localPosition = isActive ? activePosition : inactivePosition;
    }

    public void PlayIt ()
    {
        GameObject room = Instantiate(RoomPrefab, InstantiationPoint.position, Quaternion.identity) as GameObject;
        isActive = false;
        InventoryPopUp.localPosition = inactivePosition;
    }
}
