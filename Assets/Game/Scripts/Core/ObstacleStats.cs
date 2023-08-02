using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public class ObstacleStats : MonoBehaviour
    {
        [SerializeField] int points;
        [SerializeField] float level;
        [SerializeField] ObstacleController obsCon;
        [SerializeField] Material defaultMat;

        private void Start()
        {
            if (obsCon == null) obsCon = transform.GetComponentInParent<ObstacleController>();
        }

        public float GetLevel()
        {
            return level;
        }

        public int GetPoints()
        {
            return points;
        }

        public void CallReactivate()
        {
            this.GetComponent<Collider>().enabled = true;
            obsCon.ReactivateObstacle();
        }

        public void ReturnDefaultMat(){
            GetComponent<Renderer>().material = defaultMat;
        }
    }
}

