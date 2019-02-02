using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptEx00 : MonoBehaviour
{
    public uint JumpHeight;
    private bool _isJumping;
    private bool _isActive;

    private void FixedUpdate()
    {
        if (!_isActive)
            return;
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKey(KeyCode.D) || Input.GetKey("right"))
            transform.Translate(Vector3.right * Time.fixedDeltaTime);
        else if (Input.GetKey(KeyCode.A) || Input.GetKey("left"))
            transform.Translate(Vector3.left * Time.fixedDeltaTime);			
    }


    private void Jump()
    {
        if(_isJumping)
            return;
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpHeight, ForceMode2D.Impulse);
        _isJumping = true;
    }
    
    private void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            _isJumping = false;
        }
    }

    public void Activate()
    {
        _isActive = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Deactivate()
    {
        _isActive = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
    }
}
