using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankMovement : MonoBehaviour
{
    public float m_Speed;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.

    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.

    public Text NumberHPText;
    public ParticleSystem GH;

    private int _hp = 100;
    
    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_Rigidbody.centerOfMass = Vector3.down;
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start ()
    {
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";
    }


    private void Update ()
    {
        m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            m_Speed = 8f;
        }
        else
        {
            m_Speed = 4f;
        }
        NumberHPText.text = _hp.ToString();
    }

    private void FixedUpdate ()
    {
        Move ();
        Turn ();
    }


    private void Move ()
    {
        var movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        movement.y = 0;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        var turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
        var turnRotation = Quaternion.Euler (0f, turn, 0f);
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }

    public void GetHit(int type)
    {
        if (type == 1 && !GH.isPlaying)
        {
            _hp -= 10;
            GH.Play();
        }
        else
        {
            _hp -= 1;
        }
        if (_hp <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
