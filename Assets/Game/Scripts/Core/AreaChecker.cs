using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class AreaChecker : MonoBehaviour
    {
        [Header("Holder of object in Range")]
        [SerializeField] List<GameObject> ObjectsInRange = new List<GameObject>();
        GameObject nearestGameObject = null;

        public void OnTriggerEnter(Collider other)
        {
            // Debug.Log(other.name);
            if (other.gameObject.tag == "Obstacle")
            {
                ObjectsInRange.Add(other.gameObject);
            }

        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Obstacle")
            {
                ObjectsInRange.Remove(other.gameObject);
            }
        }



        public GameObject GetNearestObject()
        {
            float minSqrDistance = Mathf.Infinity;
            for (int i = 0; i < ObjectsInRange.Count; i++)
            {
                float sqrDistanceToCenter = (this.transform.position - ObjectsInRange[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    nearestGameObject = ObjectsInRange[i];
                }
            }
            return nearestGameObject;
        }

        public bool hasNearest()
        {
            return ObjectsInRange.Count > 0;
        }

        public void RemoveFromList(GameObject obstacle)
        {
            if (ObjectsInRange.Contains(obstacle))
            {
                ObjectsInRange.Remove(obstacle);
            }
        }

        public void ClearList()
        {
            ObjectsInRange.Clear();
        }
    }
}

