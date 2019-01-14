using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public int Speed = 2;

    private bool isGameRunning = true;

    // Update is called once per frame
    private void Update()
    {
        if (!isGameRunning)
            return; 
        transform.Translate(new Vector3(Speed * Time.deltaTime * -1f, 0));
        if (transform.position.x < -4.5)
            transform.position = new Vector3(4.5f, 0);
    }

    public void Stop()
    {
        isGameRunning = false;
    }
    
}
