using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private PointsManager pointsManager;
    public int number;
    void Start()
    {
        pointsManager = transform.parent.GetComponent<PointsManager>();
    }

    public void SetNumber(int n)
    {
        number = n;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plane"))
        {
            pointsManager.AddPoint(number);
        }
    }
}
