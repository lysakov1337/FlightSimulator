﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour
{
    public void GoMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
    }
}
