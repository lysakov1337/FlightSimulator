using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : MonoBehaviour
{
    [SerializeField] private PlaneController planeController;
    [SerializeField] private Animator[] animators;
    private static readonly int ChassisOut = Animator.StringToHash("ChassisOut");


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && planeController.CheckIsGrounded() == false)
        {
            RelocateChassis();
        }
        
    }

    private void RelocateChassis()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            bool chassisCondition = !animators[i].GetBool(ChassisOut);
            animators[i].SetBool(ChassisOut, chassisCondition);
        }
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ring") == false)
        {
            planeController.ChangeIsGrounded(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        planeController.ChangeIsGrounded(false);
    }
}
