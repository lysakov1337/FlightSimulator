using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

enum EngineState
{
    MaximumPower,
    MinimumPower
}

public class PlaneController : MonoBehaviour
{
    [SerializeField] private Text altitude;
    [SerializeField] private Text airSpeed;

    [SerializeField] private float speedMaxPower;
    [SerializeField] private float reverseSpeedMaxPower;
    
    [SerializeField] private float speedMinPower;
    [SerializeField] private float reverseSpeedMinPower;

    [SerializeField] private Transform forceTransform;
    [SerializeField] private Rigidbody rigidbody;

    private AudioSource audioSource;
    public bool isGrounded;
    private EngineState currentEngineState;

    private float moveDirection;
    private float sideMoveDirection;
    private float verticalArrowValue;
    private float horizontalArrowValue;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        audioSource = GetComponent<AudioSource>();
        currentEngineState = EngineState.MinimumPower;
    }

    private void Update()
    {
        ChangeEngineState();
        ChangeAudioSound();
        CheckAltitude();
        CheckAirSpeed();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        moveDirection = Input.GetAxis("Vertical");
        sideMoveDirection = Input.GetAxis("Horizontal"); 
        verticalArrowValue = Input.GetAxisRaw("Vertical_Arrow");
        horizontalArrowValue = Input.GetAxisRaw("Horizontal_Arrow");

        switch (currentEngineState)
        {
            case EngineState.MaximumPower:
                MaximumPowerMove();
                break;
            case EngineState.MinimumPower:
                MinimumPowerMove();
                break;
        }
    }

    private void MinimumPowerMove()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        if (moveDirection > 0)
        {
            rigidbody.AddForce(transform.forward * speedMinPower);
        }
        else
        {
            rigidbody.AddForce(transform.forward * (moveDirection * reverseSpeedMinPower));
        }
        
        if (Math.Abs(sideMoveDirection) > 0f && transform.InverseTransformVector(rigidbody.velocity).z > 2f)
        {
            float rotationSpeed = 0.35f;
            transform.rotation *= Quaternion.Euler(0f, sideMoveDirection * rotationSpeed, 0f);
        }
        
        if (transform.InverseTransformDirection(rigidbody.velocity).z > 30f)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * 30f;
        }
        
    }

    private void MaximumPowerMove()
    {
        if (moveDirection > 0)
        {
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.AddForce(transform.forward * (moveDirection * speedMaxPower));
        }
        else if (moveDirection < 0 && isGrounded)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
            rigidbody.AddForce(transform.forward * (moveDirection * reverseSpeedMaxPower));
        }
        

        if (Math.Abs(horizontalArrowValue) > 0 && rigidbody.velocity.magnitude >= 0)
        {
            rigidbody.AddForceAtPosition(transform.up * 2f, transform.TransformPoint(-horizontalArrowValue * 3f, 0f, 0f), ForceMode.Force);
            rigidbody.AddForceAtPosition(-transform.up * 2f, transform.TransformPoint(horizontalArrowValue * 3f, 0f, 0f), ForceMode.Force);
            if (!isGrounded)
            {
                rigidbody.velocity = transform.forward * transform.InverseTransformDirection(rigidbody.velocity).z;
            }
        }
        
        if (Math.Abs(verticalArrowValue) > 0)
        {
            float xAxisValue = verticalArrowValue > 0 ? 0.25f : 0.1f;
            float speedCoefficient = Mathf.Clamp(rigidbody.velocity.magnitude * xAxisValue, 0f, 3.5f);
            Vector3 force = transform.up * speedCoefficient;
            rigidbody.AddForceAtPosition(force, transform.TransformPoint(0f, 0f, verticalArrowValue * 4f), ForceMode.Force);
            
            if (!isGrounded)
            {
                rigidbody.velocity = transform.forward * transform.InverseTransformDirection(rigidbody.velocity).z;
            }
        }
        
        if (transform.InverseTransformDirection(rigidbody.velocity).z > 10f && Math.Abs(sideMoveDirection) > 0f)
        {
            rigidbody.AddForceAtPosition(-transform.right * (0.5f * sideMoveDirection), transform.TransformPoint(0f, 0f, -4f), ForceMode.Force);
            if (!isGrounded)
            {
                rigidbody.velocity = transform.forward * transform.InverseTransformDirection(rigidbody.velocity).z;
            }
        }

        if (transform.localEulerAngles.z > 20f && transform.localEulerAngles.z < 170f)
        {
            rigidbody.AddForceAtPosition( (transform.up * 0.4f - transform.right) * 0.5f,forceTransform.position, ForceMode.Force);
            if (!isGrounded)
            {
                rigidbody.velocity = transform.forward * transform.InverseTransformDirection(rigidbody.velocity).z;
            }
        }
        else if (transform.localEulerAngles.z > 200f && transform.localEulerAngles.z < 350f)
        {
            rigidbody.AddForceAtPosition((transform.up  * 0.4f + transform.right) * 0.5f, forceTransform.position, ForceMode.Force);
            if (!isGrounded)
            {
                rigidbody.velocity = transform.forward * transform.InverseTransformDirection(rigidbody.velocity).z;
            }
        }

        if (transform.InverseTransformDirection(rigidbody.velocity).z > 450f)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * 450f;
        }
    }

    private void ChangeEngineState()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            currentEngineState = currentEngineState == EngineState.MinimumPower ? EngineState.MaximumPower : EngineState.MinimumPower;
        }
    }
    public void ChangeIsGrounded(bool isGrounded)
    {
        this.isGrounded = isGrounded;
    }

    public bool CheckIsGrounded()
    {
        return isGrounded;
    }

    private void ChangeAudioSound()
    {
        audioSource.volume = transform.InverseTransformDirection(rigidbody.velocity).z / 450;
        audioSource.volume = Mathf.Clamp(audioSource.volume, 0.1f, 0.7f);
    }

    private void CheckAltitude()
    {
        altitude.text = "Altitude: " + (int)transform.position.y + " ft";
    }

    private void CheckAirSpeed()
    {
        airSpeed.text = "Airspeed: " + (int)rigidbody.velocity.magnitude + " kts";
    }
}
