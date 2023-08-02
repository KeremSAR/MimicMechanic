using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Core;
using MimicSpace;

namespace Game.MovementRef{
    public class Mover : MonoBehaviour, IAction
    {
        [Header("Speed of movement can be override by Mimicstat")]
        public float speed = 2f;

        Ray lastRay;
        NavMeshAgent navMeshAgent;
        private Mimic _mimic;
        Vector3 velocity = Vector3.zero;


        private float t = 0;
       // [SerializeField] float maxSpeed = 6f;
        private void Awake() {
            if (GetComponent<NavMeshAgent>() != null)
            {
                navMeshAgent = GetComponent<NavMeshAgent>();
                navMeshAgent.speed = speed;
            }
            
            if (GetComponent<Mimic>() != null)
                _mimic = GetComponent<Mimic>();
            
        }

        public void Move(Vector3 playerPos)
        {
            // rigidBody.MovePosition(rigidBody.position + shooterPosition * speed * Time.deltaTime);
            transform.position += playerPos * speed * Time.deltaTime;
        }

        public void StartMoveAction(Vector3 destination,float speedFraction)
        {   //calls in the PlayerController to start movement
            MoveTo(destination,speedFraction);
        }

        public void MoveTo(Vector3 destination,float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = speed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void MoveToPosition(Vector3 destination){
            transform.LookAt(destination);
            transform.position = Vector3.MoveTowards(transform.position,destination,speed*Time.deltaTime);
        }

        public void Stop()
        {   //Hault the movement of navmesh agent
            navMeshAgent.isStopped = true;
        }

        public void Cancel()
        {   //called when new action is triggered or called
            //Debug.Log("Stopped");
            Stop();
        }

        public void Teleport(Vector3 pos){
            Cancel();
            navMeshAgent.enabled = false;
            this.transform.position = pos;
            navMeshAgent.enabled = true;
        }

        public void SetSpeed(float speedAdjustment){
            speed = speedAdjustment;
        }

        private void Update()
        {
            _mimic.velocity = velocity;
        }
    }
}

