using UnityEngine;
using System.Collections;

namespace Utility
{
	public class KeepOnLoad : MonoBehaviour 
	{
		public void Awake ()
		{
			DontDestroyOnLoad(this.gameObject);
		}
	}
}

