using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControllerEx01 : MonoBehaviour
{
    public PlayerScriptEx01[] Players;

    private int _activePlayer;
    private Vector3 _offset;
    private static int _currentScene = 0;
    
    private void Start ()
    {
        ChangePlayer(0);
        _offset = transform.position - Players[_activePlayer].transform.position;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangePlayer(0);
        else  if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangePlayer(1);
        else  if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangePlayer(2);

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(_currentScene);
        var isFinished = true;
        foreach (var player in Players)
        {
           isFinished &= player.IsFinished();
            if (!player.IsAlive())
            {
                SceneManager.LoadScene(_currentScene);
                return;   
            }
        }

        if (isFinished)
        {
            if (_currentScene < 4)
                _currentScene++;
            SceneManager.LoadScene(_currentScene);
        }

    }
    // LateUpdate is called after Update each frame
    private void LateUpdate () 
    {
        transform.position = Players[_activePlayer].transform.position + _offset;
    }

    private void ChangePlayer(int index)
    {
        if (index >= Players.Length)
            return;
        _activePlayer = index;
        foreach (var player in Players)
        {
            player.Deactivate();
        }
        Players[_activePlayer].Activate();
    }
}
