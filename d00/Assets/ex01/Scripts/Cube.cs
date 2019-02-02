using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private float _speed;
    public int Type;
    
    
    void Start () {
        _speed = Random.Range(3f, 6f);
    }
		
    void Update ()
    {
        float precision = transform.position.y + 2;
        transform.Translate(Vector3.down * Time.deltaTime * _speed);
        
        if (transform.position.y < -5.5) {
            CubeSpawner.DeleteCube(Type);
            Destroy (transform.gameObject);
        } else if (transform.position.x == -1.41f && Input.GetKeyDown("a") && transform.position.y > -2.75f) {
            Debug.Log("Precision: " + precision);
            CubeSpawner.DeleteCube(0);
            Destroy (transform.gameObject); 
        } else if (transform.position.x == 0 && Input.GetKeyDown("s") && transform.position.y > -2.75f) {
            Debug.Log("Precision: " + precision);
            CubeSpawner.DeleteCube(1);
            Destroy (transform.gameObject);
        } else if (transform.position.x == 1.41f && Input.GetKeyDown("d") && transform.position.y > -2.75f) {
            Debug.Log("Precision: " + precision);
            CubeSpawner.DeleteCube(2);
            Destroy (transform.gameObject);
        }
    }
}
