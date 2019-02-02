using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PongBall : MonoBehaviour
{
    public int Speed = 5;
    public Player[] Players;

    private Vector3 _velocity = new Vector3(1,1);
    private void Start()
    {
        _velocity.Normalize();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckCollision();
        Move();
    }
    
    private void Move()
    {
        transform.Translate(_velocity * Time.deltaTime * Speed);
        if (transform.position.y >= 3.8f || transform.position.y <= -3.8f)
        {
            _velocity = new Vector3(_velocity.x, -_velocity.y);
            _velocity.Normalize();
        }
        else if (transform.position.x >= 6f || transform.position.x <= -6f)
        {
            if (transform.position.x >= 6f)
                Players[1].AddScore();
            else
                Players[0].AddScore();
            _velocity = new Vector3(
                Random.Range(0.5f, 1f) * (Random.Range(-1f, 1f) > 0 ? -1f: 1f) ,
                Random.Range(0.5f, 1f) * (Random.Range(-1f, 1f) > 0 ? -1f: 1f));
            _velocity.Normalize();
            transform.position = new Vector3(0f, 0f);
            Debug.Log("Player 1: " + Players[0].GetScore() + " | Player 2: " + Players[0].GetScore());
        }
    }
    
    private void CheckCollision()
    {
        var ballX = transform.position.x - transform.localScale.x / 2f;
        var ballY = transform.position.y - transform.localScale.y / 2f;
        foreach (var player in Players)
        {
            var playerX = player.transform.position.x - player.transform.localScale.x / 2f;
            var playerY = player.transform.position.y - player.transform.localScale.y / 2f;
            var collisionX =
                playerX + player.transform.localScale.x >= ballX &&
                ballX + transform.localScale.x >= playerX;
            var collisionY =
                playerY + player.transform.localScale.y >= ballY &&
                ballY + transform.localScale.y >= playerY;
            if (!(collisionX && collisionY))
                continue;
            var distance = transform.position.y - player.transform.position.y;
            var percentage = distance / (player.transform.localScale.y / 2);
            transform.position = new Vector3(transform.position.x > 0 ? 5.5f : -5.5f, transform.position.y);
            _velocity = new Vector3(-_velocity.x, _velocity.y + percentage);
            _velocity.Normalize();
            break;
        }
    }
}

