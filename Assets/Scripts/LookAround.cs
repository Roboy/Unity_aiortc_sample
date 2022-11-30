
using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LookAround : MonoBehaviour
{
    private float speed = 3f;
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(transform.position, Vector3.down, speed * Input.GetAxis("Mouse X"));
            transform.RotateAround(transform.position, transform.right,speed * Input.GetAxis("Mouse Y"));
        }
    }
}
