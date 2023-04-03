using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRotator : MonoBehaviour
{
   public float speed;
   public Transform rotateAround;
   private void Start()
   {
       //rotateAround = this.transform.position + new Vector3(0, 0.1f, 0);
   }

   void Update()
    {
        //rotateAround = this.transform.position + new Vector3(0, 0.1f, 0);
        // Spin the object around the target at 20 degrees/second.
        this.transform.RotateAround(rotateAround.position, Vector3.back, speed * Time.deltaTime);
        //this.transform.Rotate(rotateAround,speed*Time.deltaTime);
    }
}
