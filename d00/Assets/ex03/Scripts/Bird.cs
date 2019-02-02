using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public int Speed = 1;
    private bool _messageShowed = false;
    public Pipe Pipe;
    private int _score = 0;
    private string _scoreString = "Score: ";
    private string _timeString = "Time: ";
    private float _elapsedTime = 0.0f;
    private float _startTime = 0.0f;
    private bool _isGameRunning = true;
    private bool _isPipePassed = false;
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 1000, 30), _scoreString); 
        GUI.Label(new Rect(10, 20, 1000, 30), _timeString); 
    }

    // Update is called once per frame
    private void Update()
    {
        if (_isGameRunning)
        {
            var isUp = -1;
            if (Input.GetKeyDown("space"))
                isUp = 30;
            transform.localRotation = Quaternion.Euler(0, 0, transform.position.y * 30f);
            transform.Translate(new Vector3(0, Time.deltaTime * Speed * isUp), 0);
            CheckCollisions();
            UpdateMessages();
        }
        else
        { 
            Pipe.Stop();
            if (!_messageShowed)
            {
                Debug.Log(_scoreString + "\n" + _timeString);
                _messageShowed = true;
            }
        }
    }

    private void UpdateMessages()
    {
        _scoreString = "Score: " + _score;
        _elapsedTime = Time.realtimeSinceStartup - _startTime;
        _timeString = "Time: " + Mathf.RoundToInt(_elapsedTime) + "s";
    }

    private void CheckCollisions()
    {
        var pipePosition = Pipe.transform.position.x;
        var birdPosition = transform.position.y;
        if (transform.position.y < -3f)
            _isGameRunning = false;
        if (pipePosition <= 1.4f && pipePosition >= -1.3f)
        {
            if (!_isPipePassed)
            {
                _score += 5;
                Pipe.Speed++;
                _isPipePassed = true;
            }
            if (birdPosition >= 1.3f || birdPosition <= -1.7f)
                _isGameRunning = false;
        }
        else
        {
            _isPipePassed = false;
        }
    }
}
