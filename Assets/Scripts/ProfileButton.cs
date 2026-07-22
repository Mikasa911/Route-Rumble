using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButton : MonoBehaviour
{
    Button myButton;
    CanvasManager canvasManager;
    void Start()
    {
        myButton = GetComponent<Button>();
        canvasManager=FindAnyObjectByType<CanvasManager>();
        myButton.onClick.AddListener(selected);          
    }
    void selected()
    {
        canvasManager.OnSelectingProfile(myButton.GetComponentInChildren<TextMeshProUGUI>().text);
      
    }

    
}
