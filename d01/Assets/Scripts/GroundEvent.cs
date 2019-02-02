using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEvent : MonoBehaviour
{
    public GameObject Door;
    public bool Horisontal;
    public bool isReversed;
    
    private Vector3 _basicPosition;
    private Vector3 _openPosition;
    
    
    private void Start()
    {
        _basicPosition = Door.transform.position;
        if (Horisontal)
            _openPosition = new Vector3(Door.transform.position.x - 2f, Door.transform.position.y);
        else
            _openPosition = new Vector3(Door.transform.position.x, Door.transform.position.y + 0.4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Door.transform.position = _openPosition;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isReversed)
            Door.transform.position = _basicPosition;
    }
}
