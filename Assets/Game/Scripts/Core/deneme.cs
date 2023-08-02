using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class deneme : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public float spawnRadius; // The radius of the circular area
    public int numObjectsToSpawn; // The number of objects to spawn

    public bool stop;
   private Vector3 position;
    void Start()
    {
        StartCoroutine(SpawnObjectsInCircle(this.transform,45.0f,spawnRadius,numObjectsToSpawn,stop));
    }

    private IEnumerator SpawnObjectsInCircle(Transform aTransform,float aRandomAngle,float aSpawnRadius , int aNumObjectsToSpawn, bool aSpawnStop)
    {
        while (!aSpawnStop)
        {
            for (int i = 0; i < aNumObjectsToSpawn; i++)
            {
                var forward = aTransform.forward;
                var transformPos = aTransform.position;
                // spawn position of a point with aSpawnRadius distance
                Vector3 spawnPosition = transformPos + forward * aSpawnRadius;
                // Get the front direction of angle
                var directionAngle = Vector3.SignedAngle(forward, aTransform.InverseTransformPoint(spawnPosition),transformPos);
                directionAngle = Degree_Converter(directionAngle);
                Debug.Log("angle " + directionAngle);
                // Get a random point within the unit circle and scale it to the spawn radius
                float randomAngle = Random.Range(-aRandomAngle, aRandomAngle);
                randomAngle += directionAngle;
                float x = Mathf.Sin(randomAngle * Mathf.Deg2Rad);
                float z = Mathf.Cos(randomAngle * Mathf.Deg2Rad);

                var randomPoint = new Vector3(-x, 0, z);
                spawnPosition += randomPoint;
                
                // Spawn the object at the spawn position
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private float Degree_Converter(float angle)
    {
        if (angle<0)
        {
            angle += 360;
            return angle;
        }
        else
        {
            return angle;
        }
    }
    
    
}
