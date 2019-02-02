using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public uint PlayerNumber;

    public int Speed = 1;

    private int _score = 0;
    // Update is called once per frame
    private void Update()
    {
        if (PlayerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W) && transform.position.y <= 3f)
            {
                transform.Translate(new Vector3(0, Time.deltaTime * Speed), 0);
            }
            else if (Input.GetKey(KeyCode.S) && transform.position.y >= -3f)
            {
                transform.Translate(new Vector3(0, Time.deltaTime * -Speed), 0);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow) && transform.position.y <= 3f)
            {
                transform.Translate(new Vector3(0, Time.deltaTime * Speed), 0);
            }
            else if (Input.GetKey(KeyCode.DownArrow) && transform.position.y >= -3f)
            {
                transform.Translate(new Vector3(0, Time.deltaTime * -Speed), 0);
            }
        }
    }

    public void AddScore()
    {
        _score++;
    }
    
    public int GetScore()
    {
        return _score;
    }
}
