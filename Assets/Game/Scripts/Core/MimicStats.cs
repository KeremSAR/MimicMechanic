using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Core
{
    public class MimicStats : MonoBehaviour
    {
        [SerializeField] float level = 1;
        [SerializeField] int points = 0;
        [SerializeField] float speed = 2f;
        [Header("Point Before Increasing Scale")]
        [SerializeField] float factor = 30;


        //level will increase base on score
        //scale will be  level = points/factor 
        //speed will gradually increase by level

        public float GetLevel()
        {
            return level;
        }

        public int GetPoints()
        {
            return points;
        }

        public float GetSpeed()
        {
            return speed;
        }

        public void SetPoints(int points)
        {
            this.points = points;
        }

        public void CalculatePointsToLevel()
        {
            if (points > factor)
            {
                level = points / factor;
            }
        }
    }
}
