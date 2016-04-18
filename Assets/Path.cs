using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace RMV
{
    [System.Serializable]
    public class Path : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> pathPoints;
        public GameObject currentPoint;
        private int currentPointIndex;
        private GameObject nextPoint;
        private int nextPointIndex;
        public GameObject NextPoint { get { return nextPoint; } set { nextPoint = value; } }

        public GameObject UpdateToNextPoint()
        {
            currentPointIndex = nextPointIndex;
            currentPoint = pathPoints[currentPointIndex];
            nextPointIndex = currentPointIndex + 1 > pathPoints.Count - 1 ? 0 : nextPointIndex++;
            Debug.Log("Next Point Selected");
            return currentPoint;

        }

        public void AbandonPath(GameObject _target)
        {
            currentPoint = _target;
        }

        public void ResumePath()
        {
            currentPoint = pathPoints[currentPointIndex];
        }
        
         
    }
}



