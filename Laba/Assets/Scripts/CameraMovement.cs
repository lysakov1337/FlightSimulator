using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 3, -6);


    private void FixedUpdate()
    {
        Vector3 localOffset = target.right * targetOffset.x + target.up * targetOffset.y + target.forward * targetOffset.z;
        Vector3 desiredPosition = target.position + localOffset;
        
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 1f);
    }
    
    
}
