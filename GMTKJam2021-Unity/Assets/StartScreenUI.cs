using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenUI : MonoBehaviour
{

    public void StartGame()
    {
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
