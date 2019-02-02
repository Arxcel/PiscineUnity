using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public int MoveSpeed;
    public int RotationSpeed;

    
    private float _rotY;
    
    private float _rotX;
    [HideInInspector] public bool IsFlyMode;
    [HideInInspector] public bool IsSnipeMode;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void FixedUpdate()
    {
        if (IsFlyMode)
        {
            _rotY += RotationSpeed * Input.GetAxis("Mouse X");
            _rotX -= RotationSpeed * Input.GetAxis("Mouse Y");
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * MoveSpeed * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * MoveSpeed * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * MoveSpeed * Time.fixedDeltaTime);
            }        
            else if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * MoveSpeed * Time.fixedDeltaTime);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.up * MoveSpeed * Time.fixedDeltaTime);
            }        
            else if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.down * MoveSpeed * Time.fixedDeltaTime);
            }
            transform.eulerAngles = new Vector3(_rotX, _rotY, 0.0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
        }
        else if (IsSnipeMode)
        {
            if (Input.GetKey(KeyCode.D))
            {
                _rotY += RotationSpeed / 2.0f;
            }        
            else if (Input.GetKey(KeyCode.A))
            {
                _rotY -= RotationSpeed / 2.0f;
            }
            transform.eulerAngles = new Vector3(_rotX, _rotY, 0.0f);
            GetComponent<Rigidbody>().velocity = new Vector3(0f,0f,0f);
        }
    }

    private void LookAtTarget(Vector3 target)
    {
        transform.LookAt(target, Vector3.up);
        _rotX = 0;
        _rotY = transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(_rotX, _rotY, 0.0f);
    }

    public void MoveToBall(Vector3 ballPos, Vector3 currentHolePosition)
    {
        transform.position = new Vector3(ballPos.x, ballPos.y, ballPos.z);
        LookAtTarget(currentHolePosition);
        transform.Translate(Vector3.back.x * 2, transform.position.y, Vector3.back.z * 2);
    }
}
