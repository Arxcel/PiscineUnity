using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharMovement : MonoBehaviour
{
    
    public AudioClip Calm;
    public AudioClip Panic;
    public AudioClip Alarm;
    public AudioClip MoveSound;

    
    private float _yRotation;
    private float _xRotation;
    private float _lookSensitivity = 2;

    public float MoveSpeed = 6.0F;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;
    private float _gravity = 200f;
    private float _alarm;
    public Image AlarmBar;
    public bool _isVisible;

    public Text LaserDeacivator;
    public Text Card;
    
    [HideInInspector] public bool hasCard;
    [HideInInspector] public bool hasDeactivator;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();
    }
 
    private void FixedUpdate()
    {
        Card.gameObject.SetActive(false);
        LaserDeacivator.gameObject.SetActive(false);
        var speed = MoveSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
            speed += 20;
        _yRotation += Input.GetAxis("Mouse X") * _lookSensitivity;
        _xRotation -= Input.GetAxis("Mouse Y") * _lookSensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -60, 60);
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        if (_controller.isGrounded)
        {
            _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            _moveDirection = transform.TransformDirection(_moveDirection);
            _moveDirection *= speed;
        }

        _moveDirection.y -= _gravity * Time.fixedDeltaTime;
        _controller.Move(_moveDirection * Time.fixedDeltaTime);
        if (_isVisible)
            _alarm += 0.01f;
        else if (_alarm > 0.01f)
            _alarm -= 0.0005f;
        
        if (_alarm >= .75f)
        {
            var tmp = AlarmBar.color;
            tmp.r = 1;
            tmp.b = 0;
            tmp.g = 0;
            AlarmBar.color = tmp;
            
        }
        else
        {
            var tmp = AlarmBar.color;
            tmp.r = 1;
            tmp.g = 1;
            tmp.b = 1;
            AlarmBar.color = tmp;
        }

        if (_alarm >= 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        AlarmBar.fillAmount = _alarm;
        if(hasCard)
            Card.gameObject.SetActive(true);
        if(hasDeactivator)
            LaserDeacivator.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEntered");
        if (other.CompareTag("camView"))
        {
            _isVisible = true;
            Debug.Log("Visible!");
        }

        if (other.CompareTag("laser"))
            _alarm += 0.75f;

        if (other.CompareTag("card"))
        {
            hasCard = true;
            Destroy(other.transform.gameObject); 
        }
        if (other.CompareTag("object"))
        {
            hasDeactivator = true;
            Destroy(other.transform.gameObject); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("TriggerLeft");
        if (other.CompareTag("camView"))
            _isVisible = false;
    } 
}
