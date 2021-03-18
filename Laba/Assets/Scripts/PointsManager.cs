using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private int maxPoints;
    [SerializeField] private GameObject terrain;
    [SerializeField] private Text points;
    [SerializeField] private Text timerText;
    [SerializeField] private Text currentScore;
    [SerializeField] private Text record;
    [SerializeField] private GameObject panelVictory;
    
    
    private float timer;
    private bool isStarted = false;
    public int currentPoints = 0;
    private GameObject[] rings;
    
    private void Start()
    {
        rings = new GameObject[transform.childCount];
        
        for (int i = 0; i < rings.Length; i++)
        {
            rings[i] = transform.GetChild(i).gameObject;
            if (rings[i].GetComponent<Ring>() != null)
            {
                rings[i].GetComponent<Ring>().SetNumber(i);
            }
        }
    }

    private void Update()
    {
        if (isStarted)
        {
            timer += Time.deltaTime;
            
        }

        
        
        if (timer % 60 < 10)
        {
            timerText.text = "Time  " + (int) timer / 60 + ":" + "0" +(int) timer % 60;
        }
        else
        {
            timerText.text = "Time  " + (int) timer / 60 + ":" + (int) timer % 60;
        }

    }

    public void AddPoint(int number)
    {
        if (currentPoints == 0)
        {
            currentPoints++;
            Destroy(rings[0]);
            rings[2].SetActive(true);
        }
        else if (rings[number - 1] == null)
        {
            currentPoints++;
            Destroy(rings[number]);
            if (currentPoints < rings.Length - 2)
            {
                rings[number + 1].SetActive(true);
                rings[number + 2].SetActive(true);
            }

            if (number == 22)
            {
                rings[23].SetActive(true); 
            }
            Debug.Log(currentPoints);
        }

        if (currentPoints == 3)
        {
            isStarted = true;
        }

        if (currentPoints == 4)
        {
            terrain.GetComponent<ExplosionParticles>().ChangeisFlying();
        }

        points.text = currentPoints + "/" + maxPoints;
        if (currentPoints == maxPoints)
        {
            isStarted = false;
            if (PlayerPrefs.HasKey("RecordTime"))
            {
                if (PlayerPrefs.GetFloat("RecordTime") < timer)
                {
                    PlayerPrefs.SetFloat("RecordTime", timer);
                }
            }
            else
            {
                PlayerPrefs.SetFloat("RecordTime", timer);
            }

            if (timer % 60 < 10)
            {
                currentScore.text = "Ваше время: " + (int) timer / 60 + ":" + "0" +(int) timer % 60;
            }
            else
            {
                currentScore.text = "Ваше время: " + (int) timer / 60 + ":" + (int) timer % 60;
            }

            
            float score = PlayerPrefs.GetFloat("RecordTime");
            if (score % 60 < 10)
            {
                record.text = "Рекорд: " + (int) score / 60 + ":" + "0" + (int) score % 60;
            }
            else
            {
                record.text = "Рекорд: " + (int) score / 60 + ":" + (int) score % 60;
            }

            panelVictory.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    
    
    
}
