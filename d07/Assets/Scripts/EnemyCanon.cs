using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyCanon : MonoBehaviour
{

    private float _lookSensitivity = 1;
    
    public ParticleSystem GunShoot;
    public ParticleSystem MissileShoot;

    public AudioSource Gun;
    public AudioSource Missile;
    
    private int _numberMissiles = 10;

    private GameObject _target;

    public EnemyMovement body;

    private bool _enemySeen;
    private float shotTime;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_target != null)
        {
            var direction = (_target.transform.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            lookRotation.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime *_lookSensitivity);
        }
        
        RaycastHit _hit;
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out _hit, Mathf.Infinity))
        {
            if (_hit.transform.CompareTag("enemy") || _hit.transform.CompareTag("player"))
            {
                _enemySeen = true;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * _hit.distance, Color.red);
            }
            else
            {
                _enemySeen = false;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * _hit.distance, Color.white);
            }
        }
        
        if (shotTime >= 5 && _target != null)
        {
            if (_enemySeen)
            {
                var a = Random.Range(0, 4);
                Debug.Log("Enemy try shot");
                if (a == 0)
                    return;
                Debug.Log("Enemy shot");
                if (_numberMissiles > 0)
                    ShotCanon();
                else
                    ShotMachineGun();
                shotTime = 0;
                body.GetComponent<NavMeshAgent>().destination = transform.position;
            }
            else
            {
                body.GetComponent<NavMeshAgent>().destination = _target.transform.position;
                _target = null;
            }     
        }
        shotTime += Time.deltaTime;
    }
    
    private void ShotMachineGun()
    {
        if(!Gun.isPlaying)
            Gun.Play();
        GunShoot.Play();
        var enemy = _target.transform.GetComponent<EnemyMovement>();
        var player = _target.transform.GetComponent<TankMovement>();    
        if (enemy)
            enemy.GetHit(0);
        else if (player)
            player.GetHit(0);            
    }

    private void ShotCanon()
    {
        if (_numberMissiles <= 0)
            return;
        if (!Missile.isPlaying)
            Missile.Play();
        _numberMissiles--;
        MissileShoot.Play();
        var enemy = _target.transform.GetComponent<EnemyMovement>();
        var player = _target.transform.GetComponent<TankMovement>();
        if (enemy)
            enemy.GetHit(1);
        else if (player)
            player.GetHit(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("enemy") || other.CompareTag("player")) && _target == null)
            _target = other.gameObject;
        else if ((other.CompareTag("enemy") || other.CompareTag("player")) && _target != null)
            body.GetComponent<NavMeshAgent>().destination = transform.position;
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("enemy") || other.CompareTag("player")) && _target == null)
            _target = other.gameObject;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("enemy") || other.CompareTag("player") && _target != null )
        {
            body.GetComponent<NavMeshAgent>().destination = _target.transform.position;
            _target = null;
        }

    }
}
