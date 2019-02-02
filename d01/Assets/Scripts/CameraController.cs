using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public PlayerScriptEx00[] Players;

    private int _activePlayer;
    private Vector3 _offset;

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
            SceneManager.LoadScene("ex00");
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
