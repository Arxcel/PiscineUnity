using UnityEngine;
using UnityEngine.UI;

public class CanonMovement : MonoBehaviour
{

    private float _yRotation;
    private float _lookSensitivity = 1;
    
    public ParticleSystem GunShoot;
    public ParticleSystem MissileShoot;

    public AudioSource Gun;
    public AudioSource Missile;
    
    private int _numberMissiles = 10;

    public Camera cam;
    public Text NumberMissilesText;
    public Image C;

    private bool _enemySeen;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _yRotation += Input.GetAxis("Mouse X") * _lookSensitivity * Time.deltaTime;
        transform.Rotate(0, _yRotation, 0);

        if (Input.GetMouseButton(0))
            ShotMachineGun();
        if (Input.GetMouseButtonDown(1))
            ShotCanon();
        NumberMissilesText.text = _numberMissiles.ToString();
         
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 70f))
        {
            if (hit.transform.CompareTag("enemy"))
            {
                _enemySeen = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            }
            else
            {
                _enemySeen = false;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.white);
            }
        }

        if (_enemySeen)
        {
            C.color = Color.red;
        }
        else
        {
            C.color = Color.grey;
        }

    }
    
    private void ShotMachineGun()
    {
        if(!Gun.isPlaying)
            Gun.Play();
        if (!GunShoot.isPlaying)
        {
            GunShoot.Play();
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 70f))
            {
                if (hit.transform.CompareTag("enemy"))
                {
                    hit.transform.GetComponent<EnemyMovement>().GetHit(0);
                }
            }
        }
    }
    
    private void ShotCanon()
    {
        if (_numberMissiles <= 0)
            return;
        if(!Missile.isPlaying)
            Missile.Play();
        if (!MissileShoot.isPlaying)
        {
            _numberMissiles--;
            MissileShoot.Play();
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 70f))
            {
                if (hit.transform.CompareTag("enemy"))
                {
                    hit.transform.GetComponent<EnemyMovement>().GetHit(1);
                }
            }
        }
    }
}
