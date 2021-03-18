using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject planeTab;
    public void CrashPlane()
    {
        Invoke("ChangeTimeScale", 0.3f);
        planeTab.SetActive(true);
    }

    private void ChangeTimeScale()
    {
        Time.timeScale = 0f;
    }
}
