using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLift : MonoBehaviour
{
    public float TopEdge;
    public int Speed;
    private Vector3 _direction = Vector3.up;
    private float _basicHeight;
    
    private void Start()
    {
        _basicHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * Time.deltaTime * Speed);
        if (transform.position.y > TopEdge || transform.position.y < _basicHeight)
            _direction = -_direction;
    }
}
