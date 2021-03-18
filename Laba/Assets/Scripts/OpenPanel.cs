using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void OpenCurrentPanel()
    {
        panel.SetActive(true);
    }
}
