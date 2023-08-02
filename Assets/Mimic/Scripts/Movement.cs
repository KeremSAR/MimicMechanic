using System.Collections;
using System.Collections.Generic;
using Game.Core;
using UnityEngine;


namespace MimicSpace
{
    /// <summary>
    /// This is a very basic movement script, if you want to replace it
    /// Just don't forget to update the Mimic's velocity vector with a Vector3(x, 0, z)
    /// </summary>
    public class Movement : MonoBehaviour, IPolyCounterPart
    {
        [Header("Controls")]
        [Tooltip("Body Height from ground")]
        [Range(0.5f, 5f)]
        public float height = 0.8f;
        public float speed = 5f;
        Vector3 velocity = Vector3.zero;
        public float velocityLerpCoef = 4f;
        Mimic myMimic;
        public FloatingJoystick fixedJoystick;
        private MimicStats _mimicStats;
        private float _maxLevel = 5f;
        private void Start()
        {
            myMimic = GetComponent<Mimic>();
            _mimicStats = GetComponent<MimicStats>();
        }
        
        
     
       
        void Update()
        {
            
            velocity = Vector3.Lerp(velocity, new Vector3(fixedJoystick.Horizontal, 0, fixedJoystick.Vertical).normalized * speed, velocityLerpCoef * Time.deltaTime);

            // Assigning velocity to the mimic to assure great leg placement
            myMimic.velocity = velocity;

            transform.position = transform.position + velocity * Time.deltaTime;
            RaycastHit hit;
            Vector3 destHeight = transform.position;
            if (Physics.Raycast(transform.position + Vector3.up * 5f, -Vector3.up, out hit))
                destHeight = new Vector3(transform.position.x, hit.point.y + height, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destHeight, velocityLerpCoef * Time.deltaTime);
            
        }

        public void UpdateStats()
        {
            float level = _mimicStats.GetLevel();
            if (level < _maxLevel)
            {
                height += 0.1f;
            }
        }
    }

}