using UnityEngine;
using System.Collections;

public class instantiateObject : MonoBehaviour 
{


	public Rigidbody placeableObject;
	//public BoxCollider objectCollider;
	public BoxCollider gridCollider;

	// Update is called once per frame
	public void createObject(GameObject _item) 
	{

		Instantiate(_item, new Vector3(0,0,0), Quaternion.identity );
	}

}
