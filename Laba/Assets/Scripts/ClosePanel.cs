using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void CloseCurrentPanel()
    {
        panel.SetActive(false);
    }
    
    
}
