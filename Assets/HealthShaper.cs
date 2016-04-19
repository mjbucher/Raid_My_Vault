using UnityEngine;
using System.Collections;

public class HealthShaper : MonoBehaviour
{
    RectTransform myTrans;
    Player player;
    HealthManager playerHealthManager;

    void  Awake ()
    {
        myTrans = GetComponent<RectTransform>();

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }


    void Update ()
    {
        myTrans.localScale = Vector3.right * (player.health / 100.0f);
    }
}
