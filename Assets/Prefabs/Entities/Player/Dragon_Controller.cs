using UnityEngine;
using System.Collections;


namespace SPROJ.Raid_My_Vault.Player_Controller
{
	public class Dragon_Controller : MonoBehaviour {

		public float speed = 5.0f;
		public float snapValue = 0.5f;
		public bool input = false;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () 
		{
			float _inputX = Input.GetAxisRaw("Horizontal");
			float _inputY = Input.GetAxisRaw("Vertical");
			while (_inputX != 0 || _inputY != 0)
			{
				if (input == false)
				{
					MovePlayer(_inputX, _inputY);
					input = true;
				}
			}
			input = false;
		}

		public void MovePlayer(float _inputX, float _inputY)
		{
			Vector3 _currentPos = transform.position;
	//		Vector3 _targetPos = new Vector3 (_currentPos.x + _inputX, transform.position.y, _currentPos + _inputY);
			//gameObject.transform.Translate(new Vector3 (_inputX * snapValue , 0, _inputY * snapValue) * speed * Time.deltaTime);
			gameObject.transform.position = new Vector3 (_currentPos.x + snapValue, transform.position.y, _currentPos.y + snapValue);
		}
	}
};


