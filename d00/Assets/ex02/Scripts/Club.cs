using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
    private float strength;
    private Ball ball;
    private bool isSpacePressed = false;
    public Vector3 holePosition;
    private int isUp = 1;
    
    private Vector3 prevPosition;
    void  Start ()
    {
        GameObject ballObject = GameObject.Find("ball");
        ball = ballObject.GetComponent<Ball>();
        GameObject holeObject = GameObject.Find("hole");
        holePosition = holeObject.transform.position;
    }

    // Update is called once per frame
    void  Update ()
    {
       if (!ball.isGameEnded)
       {
           
           
            if (ball.speed == 0 && !isSpacePressed)
            {
                Vector3 diff = holePosition - ball.transform.position;
                diff.Normalize();

                isUp = diff.y > 0 ? 1 : -1;
                float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
                transform.position = new Vector3(ball.transform.position.x - 0.2f * isUp, ball.transform.position.y + 0.3f * isUp, ball.transform.position.z);
            }

            if (Input.GetKey("space") && ball.speed == 0)
            {
                if (!isSpacePressed)
                    prevPosition = transform.position;
                if (strength < 5f)
                {
                    strength += 0.1f;
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f * isUp, transform.position.z);
                }
                isSpacePressed = true;
            } 
            else if (strength != 0 && isSpacePressed)
            {
                isSpacePressed = false;
                transform.position = prevPosition;
                ball.speed = strength;
                strength = 0;
                ball.direction = isUp > 0 ? Vector3.up : Vector3.down;
            }
        }
       else
       {
           Destroy(transform.gameObject);
       }
    }
}

