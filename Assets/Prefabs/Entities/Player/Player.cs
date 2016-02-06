using UnityEngine;
using System.Collections;

public class Player : Entity 
{

	public float interactionRadius = 3.0f;
	[HideInInspector] public DetectionState detectionType = DetectionState.None;
}
