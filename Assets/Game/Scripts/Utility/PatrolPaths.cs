using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.MovementRef{
    public class PatrolPaths : MonoBehaviour
    {
        [Tooltip("Change color line and waypoint")]
        [SerializeField] Color pathColor;
        [Tooltip("Adjust size of waypoint radius")]
        const float wayPointGizmoRadius = 0.3f;
        private void OnDrawGizmos()
        {   //Visualize patrol path
            Gizmos.color = pathColor;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetPosition(i), wayPointGizmoRadius);
                int j = GetNextIndex(i);
                Gizmos.DrawLine(GetPosition(i), GetPosition(j));
            }
        }

        public int GetNextIndex(int i) //get next waypoint index
        {
            if (i < this.transform.childCount - 1)
            {
                return i + 1;
            }
            else
            {
                return 0;
            }
        }

        public Vector3 GetPosition(int i) //get waypoint position
        {
              return transform.GetChild(i).position;
        }
    }

}
