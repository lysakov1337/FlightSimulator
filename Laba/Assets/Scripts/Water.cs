using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            gameManager.CrashPlane();
        }
    }
}
