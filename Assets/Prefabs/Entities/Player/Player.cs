using UnityEngine;
using System.Collections;

public class Player : Entity 
{
	[Header("Player Detection")]
	public float interactionRadius = 3.0f;

    private int keys = 0;
    public int AddKeys { set { keys += value; } }
    public int CheckKeys { get { return keys; } }

}
