using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    private float _factor;
    private float _breath = 1.0f;
    private string _breathString = "Breath left: ";
    private string _timeString = "Time: ";
    private float _elapsedTime = 0.0f;
    private float _startTime = 0.0f;
    private bool _messageShowed = false;
    
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 30), _breathString); 
        GUI.Label(new Rect(10, 20, 1000, 30), _timeString); 
    }

    void Update()
    {
        float x = transform.localScale.x;
        if (x < 6.0f && x > 0.1f)
        {
            if (Input.GetKeyDown("space"))
            {
                if (_breath >= 0.1)
                {
                    _breath -= 0.1f;
                    _factor = 0.1f;
                }
            }
            else
            {
                _factor = -0.005f;
            }

            _breathString = "Breath left: " + _breath.ToString("0.00");
            _elapsedTime = Time.realtimeSinceStartup - _startTime;
            _timeString = "Balloon life time: " + Mathf.RoundToInt(_elapsedTime) + "s";
            transform.localScale += new Vector3(_factor, _factor, 0);
            _breath += 0.0005f;
        }
        else
        {
            if (!_messageShowed)
            {
                Debug.Log("Balloon life time:" + _elapsedTime + "s");
                _messageShowed = true;
            }

            if (Input.GetKeyDown("r"))
            {
                _messageShowed = false;
                _startTime = Time.realtimeSinceStartup;
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                _breath = 1.0f;
            }
        }
    }
}
