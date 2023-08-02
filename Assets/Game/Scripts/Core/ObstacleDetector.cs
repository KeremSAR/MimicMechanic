using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.MovementRef;
using UnityEngine;
using UnityEngine.Serialization;
using MimicSpace;

namespace Game.Core{
    public class ObstacleDetector : MonoBehaviour
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
        private Vector3 middlePoint;
        [SerializeField] private ParticleSystem PS;
        private MeshRenderer sphere;
        private Movement _movement; 
        
        private void Start()
        {
            if (GetComponentInParent<MimicStats>() != null)
            {
                mimicStats = GetComponentInParent<MimicStats>();
            }
            if (GetComponentInParent<Movement>() != null)
            {
                _movement = GetComponentInParent<Movement>();
            }
            if (scoreManager == null)
            {
                scoreManager = FindObjectOfType<ScoreManager>();
            }

            sphere = GetComponent<MeshRenderer>();

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
                    //myMimic.ResetMimic2();
                    renderer = other.GetComponent<Renderer>();
                    middlePoint = other.GetComponent<Renderer>().bounds.center;
                    _movement.enabled = false;
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
            myMimic.ResetMimic();
            other.gameObject.GetComponent<ObstacleStats>().CallReactivate();
           // PS.Play();
           other.GetComponent<MeshExploder>().Explode();
            mimicStats.SetPoints(mimicStats.GetPoints() + obStats.GetPoints());
            mimicStats.CalculatePointsToLevel();
            mimicStats.GetComponent<IPolyCounterPart>().UpdateStats();
            scoreManager.updateStanding();
            other.gameObject.SetActive(false);
            targetTransform = null;
            renderer = null;
            move = false;
            _movement.enabled = true;
            myMimic.GrowMimicLegs();
            sphere.enabled = true;
           // myMimic.transform.DOPunchScale(new Vector3(1.2f, 1.2f, 1.2f), .3f, 10);
        }

        private void FixedUpdate()
        {
            if (move)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, Color.black, 3f * Time.deltaTime);
                myMimic.transform.position = Vector3.Lerp(myMimic.transform.position, middlePoint, 3f* Time.deltaTime);
            }
        }
    }

}
