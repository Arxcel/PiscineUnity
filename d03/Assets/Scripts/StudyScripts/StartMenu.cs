using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void QuitApplication() {
        Application.Quit();
    }

    public void OnPlay()
    {
        SceneManager.LoadScene("ex01");
    }
}
