using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 Target;
    public float Speed;

    private bool _IsDestroyed;
    private void Start()
    {
        var direction = Target - transform.position;
        direction.Normalize();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x , direction.y) * Speed); 
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _IsDestroyed = true;
    }

    private void LateUpdate()
    {
        if (_IsDestroyed)
            Destroy(transform.gameObject);
    }
}
