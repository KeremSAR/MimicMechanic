using System;
using System.Collections;
using System.Collections.Generic;
using Game.MovementRef;
using UnityEngine;
using UnityEngine.Serialization;
using MimicSpace;
using UnityEngine.AI;

namespace Game.Core{
    public class Absorber : MonoBehaviour
    {
        [SerializeField] float timeBeforeDisable = 2f;
        [Header("Reference to MimicStat of the parent")]
        [SerializeField] MimicStats mimicStats;
        [Header("Reference to interacted obstacleStats")]
        [SerializeField] ObstacleStats obStats;
        [Header("Reference ScoreManager Script")]
        [SerializeField] ScoreManager scoreManager;
        
        [SerializeField] private Mimic myMimic;
        private bool move = false;
        private Transform targetTransform;
        private Renderer renderer;
        public Vector3 middlePoint;
        [SerializeField] private ParticleSystem PS;
        [SerializeField] private MeshRenderer sphere;
        [SerializeField] private Color _color;
        private NavMeshAgent navMeshAgent;
        private float t;
        private void Start()
        {
            if (GetComponentInParent<MimicStats>() != null)
            {
                mimicStats = GetComponentInParent<MimicStats>();
            }
            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<ScoreManager>();
            }
            if (GetComponentInParent<NavMeshAgent>() != null)
            {
                navMeshAgent = GetComponentInParent<NavMeshAgent>();
            }
        }

        public void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Obstacle")
            {
                obStats = other.GetComponent<ObstacleStats>();
                if (obStats.GetLevel() <= mimicStats.GetLevel())
                {
                    other.enabled = false;
                    sphere.enabled = false;
                    //countdown to set active false
                    //the reset position
                    //send back to the pool
                    
                    // Renderer renderer = other.GetComponent<Renderer>();
               
                    renderer = other.GetComponent<Renderer>();
                    middlePoint = other.GetComponent<Renderer>().bounds.center;
                    myMimic.SuckMimicLegs();
                    move = true;

                    /*if (renderer != null)
                    {
                        renderer.material.color = Color.Lerp(renderer.material.color, Color.black, 1f);
                    }*/
                    StartCoroutine(DisableAfterFall(other.gameObject));
                }
                

            }

        }

        IEnumerator DisableAfterFall(GameObject other)
        {
            yield return new WaitForSecondsRealtime(timeBeforeDisable);
            //other.gameObject.SetActive(false);
            other.gameObject.GetComponent<ObstacleStats>().CallReactivate();
            PS.Play();
            mimicStats.SetPoints(mimicStats.GetPoints() + obStats.GetPoints());
            mimicStats.CalculatePointsToLevel();
            mimicStats.GetComponent<IPolyCounterPart>().UpdateStats();
            scoreManager.updateStanding();
            other.gameObject.SetActive(false);
            targetTransform = null;
            renderer = null;
            move = false;
            myMimic.GrowMimicLegs();
            sphere.enabled = true;
        }

        private void FixedUpdate()
        {
            if (move)
            {
                t += Time.deltaTime / 3f;
                renderer.material.color = Color.Lerp(renderer.material.color, _color, 3f * Time.deltaTime);
               // myMimic.transform.position = Vector3.Lerp(myMimic.transform.position, middlePoint, 3f* Time.deltaTime);
                navMeshAgent.baseOffset = Mathf.Lerp(navMeshAgent.baseOffset, middlePoint.y, 3f* Time.deltaTime);
            }
            else
            {
                navMeshAgent.baseOffset = Mathf.Lerp(navMeshAgent.baseOffset, .5f, 3f * Time.deltaTime);
            }
        }
    }

}
