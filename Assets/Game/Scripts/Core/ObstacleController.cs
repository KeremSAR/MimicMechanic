using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public class ObstacleController : MonoBehaviour
    {   
        [Header("Reference to Child Obstacle")]
        //[SerializeField] GameObject obstacleAssign;
      //  [SerializeField] Rigidbody rbObstacle;
        [Header("Initial Position and Rotation")]
        [SerializeField] Vector3 obstaclePos;
        [SerializeField] Quaternion obstacleRot;
        [Header("Respawn Timer")]
        [SerializeField] float timeBeforeRespawn;


        void Start()
        {
            if (gameObject == null)
            {
                //obstacleAssign = this.transform.GetChild(0).gameObject;
              //  rbObstacle = obstacleAssign.GetComponent<Rigidbody>();
                obstaclePos = transform.position;
                obstacleRot = transform.rotation;
            }
        }

        public void ReactivateObstacle()
        {

           // StartCoroutine(ReactivateAbsorbedObstacle());
            //update score
        }

        IEnumerator ReactivateAbsorbedObstacle()
        {
            yield return new WaitForSecondsRealtime(timeBeforeRespawn);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
               // rbObstacle.angularVelocity = Vector3.zero;
                //rbObstacle.velocity = Vector3.zero;
                gameObject.transform.position = obstaclePos;
                gameObject.transform.rotation = obstacleRot;
            }
        }

    }
}

