using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.MovementRef;

namespace Game.Core{
    public class EnemyController : MonoBehaviour, IPolyCounterPart
    {
        [Tooltip("Almost all reference will initialize at the Beginning")]
        [Header("Mover Script Reference")]
        [SerializeField] Mover mover = null;

        [Header("AreaChecker Script Reference")]
        [SerializeField] AreaChecker areaChecker;

        [Header("Reference to the player gameObject")]
        [SerializeField] GameObject player = null;

        [Header("Player MimicStats Script reference")]
        [SerializeField] MimicStats playerStat = null;

        [Header("Enemy MimicStats Script reference")]
        [SerializeField] MimicStats EnemyStat = null;

        [Header("Timer Script Reference")]
        [SerializeField] Timer timer = null;

        [Header("Scale Limit")]
        [Range(0,15)]
        [SerializeField] float maxLevel = 14;
        
        [Header("Speed fraction to adjust speed")]
        [Range(0, 1)]
        [SerializeField] float speedFraction = 1f;

        [Header("Attack Range")]
        [SerializeField] float attackRange = 2f;

        [Header("AI PATROL")]
        [SerializeField] PatrolPaths patrolPath;
        [SerializeField] float wayPointTolerance = 3f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] int currentWaypointIndex = 0;
        [Header("reference to the object closest to")]
        [SerializeField] GameObject nearestObject = null;
        

        [Header("Initial Position")]
        [SerializeField] Vector3 guardPosition;
        [Header("Initial Scale")]
        [SerializeField] Vector3 intialScale;

        [SerializeField]List<GameObject> enemiesGB = new List<GameObject>();
        

        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        bool isCanMove = true;
        float initialRange;

        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
           // if(GetComponent<AreaChecker>() != null) areaChecker = GetComponent<AreaChecker>();
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");
            if(playerStat == null) playerStat = player.GetComponent<MimicStats>();
            if(EnemyStat == null)  EnemyStat = GetComponent<MimicStats>();
            if (timer == null){
                timer = FindObjectOfType<Timer>();
                timer.timeRunOut += TimeRunOut;
            }
            intialScale = transform.localScale;
            initialRange = attackRange;

            guardPosition = transform.position;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject gb in enemies){
                if(gb.gameObject != this.gameObject){
                    enemiesGB.Add(gb);
                }
            }
            
        }

    

        private void FixedUpdate()
        {
            if(isCanMove){
                if (CheckPlayer())
                {
                   // AttackPlayer();
                   
                }
                else if (areaChecker.hasNearest() && CheckLevelObstacle())
                {
                    CheckNearestAbsorbable();
                }
                else
                {
                    PatrolBehaviour();
                }
            }

            AbsorbedByPlayer();
            AbsorbedByEnemy();

            updateTimers();

        }

        private bool CheckPlayer()
        {
            bool isInLevel = playerStat.GetLevel() < EnemyStat.GetLevel();
            bool isInRange = Vector3.Distance(this.transform.position, player.transform.position) < attackRange;
            return (isInLevel && isInRange);
        }

        private void AttackPlayer()
        {
            mover.Cancel();
            mover.StartMoveAction(player.transform.position,speedFraction);
          //  AbsorbPlayer();
        }

        private void updateTimers()
        {
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {   //check the if close to a waypoint
                    timeSinceArrivedAtWaypoint = 0; // reset time Dwelled in waypoint to zero to start the dwelling
                    CycleWayPoint(); //check next waypoint
                }
                nextPosition = GetCurrentPosition(); //get and set the position of the waypoint based on the currentIndex
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition,speedFraction); //move to a waypoint
            }

        }


        private void CheckNearestAbsorbable()
        {
            mover.Cancel();
            //mover.AdjustHeight();
            nearestObject = areaChecker.GetNearestObject(); //get the nearest object
            if (nearestObject != null)
            {
                mover.StartMoveAction(nearestObject.transform.position,speedFraction); // move and eat nearest obstacle
                if(!nearestObject.activeSelf){
                    areaChecker.RemoveFromList(nearestObject);
                    nearestObject = null;
                }
                
            }
        }

        private bool CheckLevelObstacle()
        {
            nearestObject = areaChecker.GetNearestObject();
            bool isGreater = nearestObject.GetComponent<ObstacleStats>().GetLevel() <= EnemyStat.GetLevel();
            nearestObject = null;
            return isGreater;
        }


        /*void AbsorbPlayer(){ //absorb player hole
            float scalDif = Vector3.Distance(transform.localScale,player.transform.localScale);
            Debug.Log(scalDif);
            if (Vector3.Distance(this.transform.position, player.transform.position) <= scalDif &&
                !player.GetComponent<PlayerController>().GetIsDead()){
                player.GetComponent<PlayerController>().Dead();
            }
        }*/

        
        void AbsorbedByPlayer(){ // can be refactored later still 
            float scalDif = Vector3.Distance(transform.localScale, player.transform.localScale);
            if (Vector3.Distance(this.transform.position, player.transform.position) <= scalDif &&
            playerStat.GetLevel() > EnemyStat.GetLevel())
            {
               playerStat.SetPoints(playerStat.GetPoints() + 50);
               StartCoroutine(Dead());
            }
        }

        void AbsorbedByEnemy(){ // can be refactored later
            GameObject nearestEnemy = GetNearestEnemy();
            float scalDif = Vector3.Distance(transform.localScale, nearestEnemy.transform.localScale);
            if (Vector3.Distance(this.transform.position, nearestEnemy.transform.position) <= scalDif &&
            nearestEnemy.GetComponent<MimicStats>().GetLevel() > EnemyStat.GetLevel())
            {
                StartCoroutine(Dead());
            }
        }
        

        public GameObject GetNearestEnemy()
        {
            GameObject nearestEnemy = null;
            float minSqrDistance = Mathf.Infinity;
            for (int i = 0; i < enemiesGB.Count; i++)
            {
                float sqrDistanceToCenter = (this.transform.position - enemiesGB[i].transform.position).sqrMagnitude;
                if (sqrDistanceToCenter < minSqrDistance)
                {
                    minSqrDistance = sqrDistanceToCenter;
                    nearestEnemy = enemiesGB[i];
                }
            }
            return nearestEnemy;
        }

        public void ResetPost(){
            StartCoroutine(Dead());
        }

        IEnumerator Dead()
        {
            isCanMove = false;
            mover.Teleport(guardPosition);
            yield return new WaitForSeconds(4f);
            isCanMove = true;
            
        }

        private bool AtWaypoint()
        {
            float wayPointDistance = Vector3.Distance(this.transform.position, GetCurrentPosition());
            return wayPointDistance < wayPointTolerance;
        }

        private Vector3 GetCurrentPosition()
        {
            return patrolPath.GetPosition(currentWaypointIndex);
        }

        private void CycleWayPoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private void IncreaseScale()
        {
            float level = EnemyStat.GetLevel();
            if(level < maxLevel) {
                float increasedScale = level * .3f;
                this.transform.localScale = new Vector3(intialScale.x + increasedScale, intialScale.y + increasedScale, intialScale.z + increasedScale);
                attackRange =  initialRange + increasedScale;
            }
            
            
        }

        public void UpdateStats(){
            //IncreaseScale();
        }

        void TimeRunOut(bool isTimeRunOut)
        {
            if(isTimeRunOut){
                mover.SetSpeed(0f);
                mover.Cancel();
                isCanMove = false;
            }
            
        }

        private void OnDrawGizmos()
        {   
            //draw attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}

