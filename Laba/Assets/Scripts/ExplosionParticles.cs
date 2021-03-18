using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParticles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameManager gameManager;
    private bool isFlying;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Plane") && isFlying)
        {
            explosion.SetActive(true);
            gameManager.CrashPlane();
        }
    }

    public void ChangeisFlying()
    {
        isFlying = true;
    }
}
