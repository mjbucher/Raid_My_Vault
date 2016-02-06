using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Enterance : MonoBehaviour 
{
	public GameObject enterancePoint;
	//GameObject enteranceModel;
	public bool isOpen = false;


	void Update ()
	{
		if (isOpen)
		{
			enterancePoint.SetActive(false);
		}
		else{
			enterancePoint.SetActive(true);
		}
	}
}
