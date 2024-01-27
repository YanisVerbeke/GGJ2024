using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public void PlayButtonClicked()
    {
        // JOUER AU JEU VIDEO
    }

    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
