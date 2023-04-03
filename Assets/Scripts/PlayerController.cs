using System;
//using Joystick_Pack.Scripts.Joysticks;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private Vector3 direction;
    private Camera Cam;

    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float TurnSpeed;
    [SerializeField] private FloatingJoystick _floatingJoystick;
         

    private void Start()
    {
        Cam = Camera.main;
       
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            JoyisticMovement();
        }
       
       
    }
    
    private void Update()
    {
            
    }

    private void JoyisticMovement()
    {
        float horizantal = _floatingJoystick.Horizontal;
        float vertical = _floatingJoystick.Vertical;
            
        Vector3 addedPos = new Vector3(horizantal * PlayerSpeed * Time.deltaTime, 0,
            vertical * PlayerSpeed * Time.deltaTime);
        var transform1 = transform;
        transform1.position += addedPos;

        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizantal;
        transform.rotation = Quaternion.Slerp(transform1.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.deltaTime);
        //  transform.rotation = Quaternion.LookRotation(direction);
    }
            
           
}
/* private void Controller()
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = Cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
            {
                this.direction = ray.GetPoint(distance);
            }
           
            //transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x,0f, direction.z), PlayerSpeed* Time.deltaTime);
            var offset = direction - transform.position;
            if (offset.magnitude > 1f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), TurnSpeed * Time.deltaTime);
                //transform.LookAt(direction);
            }
        }*/
