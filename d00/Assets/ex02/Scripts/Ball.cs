using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isGameEnded = false;
    public bool isBallMoving = false;
    public float deceleration;

    public float speed;
    public Vector3 direction;
    public  int  score  =  - 15 ;
    
    // Start is called before the first frame update
    void Start()
    {
 
        direction = Vector3.up;
    }

    // Update is called once per frame
    void  Update () {
        if (speed > 0) {
            isBallMoving = true;
            speed -= deceleration;
        }
        else {
            if (isBallMoving && !isGameEnded) {
                score += 5;
                Debug.Log("Score " + score);
            }
            isBallMoving = false;
            speed = 0;
        }
        // Move ball
        transform.Translate(direction * speed * Time.deltaTime);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.2f, 3.2f), Mathf.Clamp(transform.position.y, -4.75f, 4.75f), transform.position.z);
       
        // End game
       if (speed <= 2 && transform.position.y >= 2.9F && transform.position.y <= 3.1F && transform.position.x == 0f)
       {
            isGameEnded = true;
            if (score > 0)
                Debug.Log("Your score is " + score + ". You loose :(");
            else
                Debug.Log("Your score is " + score + ". You win !");
            Destroy(transform.gameObject);
       }  
       Bounce();
    }

    private void Bounce()
    {
        if (transform.position.y >= 4.7 || transform.position.y <= -4.7)
            direction = new Vector3(direction.x, -direction.y, direction.z);
        else if (transform.position.x >= 3.2 || transform.position.x <= -3.2)
            direction = new Vector3(-direction.x, direction.y, direction.z);
    }
    
}

