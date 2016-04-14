using UnityEngine;
using System.Collections;
using RMV;

namespace RMV
{
    public class ActiveInventory : MonoBehaviour
    {
        public int numActiveItems = 4;

        void OnGUI()
        {
            
        }
           
        void DrawActivebar ()
        {
            for (int x = 0; x < numActiveItems; x++)
            {
                Rect rect = new Rect();
            }
        }

    }
}


