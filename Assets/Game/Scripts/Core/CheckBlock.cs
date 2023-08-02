using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public class CheckBlock : MonoBehaviour
    {   
        [Header("Player Reference")]
        [SerializeField] GameObject player = null;
        [Header("Layer for linecast hit")]
        [SerializeField] LayerMask rayMask;
        [Header("Material to switch when object blocks players view")]
        [SerializeField] Material opaqueMat;
        [SerializeField] GameObject obstacleHit;

        List<ObstacleStats> obstacleList = new List<ObstacleStats>();

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            ChangeOpaqueLineCast();
            if(obstacleHit != null) return;
            restoreHit();
        }

        private void ChangeOpaqueLineCast()
        {
            RaycastHit hit;
            //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 1f);
            if (Physics.Linecast(transform.position, player.transform.position, out hit, rayMask))
            {
                if (hit.transform.gameObject.tag == "Obstacle")
                {
                    obstacleHit = hit.transform.gameObject;
                    if (obstacleHit != null)
                    {
                        obstacleHit.GetComponent<Renderer>().material = opaqueMat;
                        if(obstacleList.Contains(obstacleHit.GetComponent<ObstacleStats>()))return;
                        obstacleList.Add(obstacleHit.GetComponent<ObstacleStats>());                    
                    }
                }
            }
            else
            {
                if (obstacleHit != null)
                {
                     obstacleHit.GetComponent<ObstacleStats>().ReturnDefaultMat();
                     obstacleHit = null;
                }
            }
        }

        void restoreHit(){
            if(obstacleList.Count > 0){
                foreach(ObstacleStats obs in obstacleList){
                    obs.ReturnDefaultMat();
                }
                obstacleList.Clear();
            }

        }

    }
}

