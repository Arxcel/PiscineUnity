using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [HideInInspector]public float _str;
    [HideInInspector]public float _agi;
    [HideInInspector]public float _con;
    [HideInInspector]public float _armor;
    
    [HideInInspector]public float HitPoints;
    [HideInInspector]public float MinDmg;
    [HideInInspector]public float MaxDmg;
    [HideInInspector]public float Level;
    
    [HideInInspector]public bool IsAlive = true; 
    [HideInInspector]public float AttackSpeed = 1f;
    [HideInInspector]public float AttackRange = 4;
    [HideInInspector]public float MaxHitPoints;
    [HideInInspector]public float XPHolds;

    
    
    private NavMeshAgent _agent;
    // Start is called before the first frame update
    private RaycastHit _hit;
    public GameObject Target;
    private Animator _animator;
    
    private bool _isMoving;
    // Start is called before the first frame update
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _agent.updateRotation = false;
          _str = Random.Range(10, 20);
          _agi = Random.Range(10, 20);
          _con = Random.Range(10, 20);
          _armor = Random.Range(10, 20);
        InitPlayer();
    }

    private IEnumerator MoveDown()
    {
        for(;;) {
            _agent.baseOffset -= 0.0001f;
            yield return null;
        }
    }

    private IEnumerator DeleteEnemy()
    {
        yield return new WaitForSeconds(7f);
        Destroy(transform.gameObject);
    }

    private IEnumerator Death()
    {
        _animator.SetTrigger("death");
        yield return new WaitForSeconds(5f);
        StartCoroutine(MoveDown());
        StartCoroutine(DeleteEnemy());
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsAlive)
            return;
        if(_agent.velocity.magnitude < 0.1f)
            _animator.SetBool("walk", false);
        else
        {
            transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
            _animator.SetBool("walk", true);
        }
    }

    public bool TakeDamage(float damage)
    {
        Debug.Log("TakeDamage");
        if (!IsAlive)
            return IsAlive;
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            HitPoints = 0;
            IsAlive = false;
            Die();
        }
        return IsAlive;
    }

    private IEnumerator PrepareAttack()
    {
        if (!IsAlive)
            yield break;
        while (Target != null)
        {
            var player = Target.GetComponent<PlayerMovement>();
            if (!player || !player.IsAlive)
                yield break;
            _agent.destination = Target.transform.position;
            var dist = Vector3.Distance(Target.transform.position, transform.position);
            while (dist > AttackRange)
            {
                if (Target == null || !IsAlive)
                        yield break;
                dist = Vector3.Distance(Target.transform.position, transform.position);
                yield return null; 
            }
            transform.LookAt(Target.transform.position);
            _agent.destination = transform.position;
            if (Target == null || !IsAlive)
                yield break;
            _animator.SetTrigger("meelee"); 
            Target.GetComponent<PlayerMovement>().TakeDamage(GetDamage());
            yield return new WaitForSeconds(10f / AttackSpeed); 
        }
    }

    private float GetDamage()
    {                
        var baseDamage = Random.Range(MinDmg, MaxDmg);
        if (Target != null)
        {
            var player = Target.GetComponent<PlayerMovement>();
            if (player != null)
            {
                baseDamage = baseDamage * (1 - player.Armor / 200f);
                var chance = 75 + _agi - player._agi;
                if (Random.Range(0f, 100f) < chance)
                    baseDamage *= 0;
            }
        }
        return baseDamage;
    }

    private void Die()
    {
        StartCoroutine(Death());
    }
    
    private void InitPlayer()
    {
        MaxHitPoints = _con * 5;
        HitPoints = MaxHitPoints;
        MinDmg = _str / 2;
        MaxDmg = MinDmg + 4;
        Level = 1;
        XPHolds = _con * Level;
    }

    public void SetTarget(GameObject target)
    {
        Target = target;
        StartCoroutine(PrepareAttack());
    }
}


