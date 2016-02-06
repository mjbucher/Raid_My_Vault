using UnityEngine;
using System.Collections;
using Rand = UnityEngine.Random;
namespace SPROJ.Raid_My_Vault.UI_Building
{
	public class UI_Events : MonoBehaviour 
	{
		private float speed = 5.0f;
		// make class for UI button | Prefab eventually
		// make enum list for the item list (maybe handled in the game manager | or a separate class
		public void MakeItem(GameObject _item)
		{
			Debug.Log("makeItem called");
			// new item spawned
			GameObject _newItem = Instantiate<GameObject>( _item);
			// item follows the mouse
			StartCoroutine(FollowMouse(_newItem));
			
		}
		
		public IEnumerator FollowMouse(GameObject _item)
		{
			Debug.Log("follow mouse started");
			while (Input.GetMouseButton(0) == true)
			{
				float _mouseXR = (Mathf.Round(Input.mousePosition.x * 2)) / 2;
				float _mouseYR = (Mathf.Round(Input.mousePosition.y * 2)) / 2;
				Vector3 _mouseVR = new Vector3(_mouseXR, _mouseYR , 0.0f);
				_item.transform.position = _mouseVR * Time.deltaTime;
			}
			Debug.Log ("Leaving loop");
			Debug.Log ("checking underneath");
			RaycastHit _rayHit;
			Ray _ray = new Ray( _item.transform.position, Vector3.down);
			Debug.Log("did i hit?");
			if (Physics.Raycast(_ray, out _rayHit))
			{
				if (_rayHit.collider.gameObject.tag == "floor")
				{
					_item.transform.position = _rayHit.collider.gameObject.transform.position;
					Debug.Log("yes");
				}
			}
			else 
			{
				Debug.Log ("no");
				DestroyObject(_item);
			}
			yield return null;

		}
			
	}
}

