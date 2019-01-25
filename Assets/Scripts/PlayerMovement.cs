using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Analytics;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector]public float _str = 20;
    [HideInInspector]public float _agi = 20;
    [HideInInspector]public float _con = 20;
    [HideInInspector]public float _armor = 20;
    
    [HideInInspector]public float MaxHitPoints;
    [HideInInspector]public float HitPoints;
    [HideInInspector]public float MinDmg;
    [HideInInspector]public float MaxDmg;
    [HideInInspector]public float Level;
    [HideInInspector]public float Xp;
    [HideInInspector]public float NextLevel;
    [HideInInspector]public float Money;
    [HideInInspector]public bool IsAlive = true; 
    [HideInInspector]public float AttackSpeed = 10f;
    [HideInInspector]public float AttackRange = 3;
    [HideInInspector]public float Armor = 20;
    [HideInInspector]public float StatPoints = 0;

    public Text HpText;
    public Image HpAmoumt;
    
    
    public Text XpText;
    public Image XpAmoumt;
    
    public Text LevelText;

    public Image EnemyHP;
    public Image EnemyMaxHP;
    public Text EnemyLvl; 
    
    public Text STRTEXT; 
    public Text AGITEXT; 
    public Text CONTEXT; 
    public Text StatPointsText; 

    public Text DmgText; 
    public Text ArmorText; 
    public Text ValetsText; 

    
    public Image StatMenu;
    private bool _statVisible;
    
    private NavMeshAgent _agent;
    // Start is called before the first frame update
    private RaycastHit _hit;
    private Camera _cam;
    private GameObject _target;
    private GameObject _selection;
    private Animator _animator;
    private float _dmgTimer;

    public Button[] Pluses;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _cam = Camera.main;
        _agent.updateRotation = false;
        InitPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!IsAlive)
            return;
        if (Input.GetKeyDown("mouse 0"))
        {
            if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out _hit))
            {
                Debug.Log(_hit.transform.tag);
                if (_hit.transform.CompareTag("Enemy"))
                {
                    _target = _hit.transform.gameObject;
                    var enemy = _target.GetComponent<EnemyLogic>();
                    if (!enemy.IsAlive)
                    {
                        _target = null;
                        _agent.destination = _hit.point;
                    }
                    else
                    {
                        StartCoroutine(PrepareAttack());
                    }
                }
                else
                {
                    _agent.destination = _hit.point;
                }
            }
        }

        if(_agent.velocity.magnitude < 0.1f)
            _animator.SetBool("walk", false);
        else
        {
            transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized);
            _animator.SetBool("walk", true);
        }

        if (Mathf.Abs(Input.GetAxis("Mouse X")) >= 0.01 || Mathf.Abs(Input.GetAxis("Mouse Y")) >= 0.01)
        {
            if (_target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        _selection = hit.transform.gameObject;
                    }
                    else
                    {
                        _selection = null;
                    }
                }
                else
                {
                    _selection = null;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _statVisible = !_statVisible;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {

        foreach (var btn in Pluses)
        {
            btn.gameObject.SetActive(StatPoints > 0);
        }
        
        EnemyHP.gameObject.SetActive(false);
        EnemyMaxHP.gameObject.SetActive(false);
        EnemyLvl.gameObject.SetActive(false);
        StatMenu.gameObject.SetActive(_statVisible);
        
        HpText.text = "" + HitPoints.ToString("0") + "/" + MaxHitPoints.ToString("0");
        HpAmoumt.fillAmount = 1 - (MaxHitPoints - HitPoints) / MaxHitPoints;
        XpText.text = "" + Xp.ToString("0") + "/" + NextLevel.ToString("0");
        XpAmoumt.fillAmount = 1 - (NextLevel - Xp) / NextLevel;
        LevelText.text = "Lvl: " + Level.ToString("0");
        
        STRTEXT.text = _str.ToString("0");
        AGITEXT.text = _agi.ToString("0");
        CONTEXT.text = _con.ToString("0");
        StatPointsText.text = StatPoints.ToString("0");
        DmgText.text = "Damage: " + MinDmg.ToString("0") + "-" + MaxDmg.ToString("0");
        ArmorText.text = "Armor: " + _armor.ToString("0");
        ValetsText.text = "Valets: " + Money.ToString("0");
        if (_target != null)
        {
            var enemy = _target.GetComponent<EnemyLogic>();
            if (enemy)
            {
                EnemyHP.gameObject.SetActive(true);
                EnemyMaxHP.gameObject.SetActive(true);
                EnemyLvl.gameObject.SetActive(true);
                    
                EnemyHP.fillAmount = 1 - (enemy.MaxHitPoints - enemy.HitPoints) / enemy.MaxHitPoints;
                EnemyLvl.text = "Lvl: " + enemy.Level.ToString("0") + "   " + "" + enemy.HitPoints.ToString("0") + "/" + enemy.MaxHitPoints.ToString("0");
            }
        }
        else if (_selection != null)
        {
            var enemy = _selection.GetComponent<EnemyLogic>();
            if (enemy)
            {
                EnemyHP.gameObject.SetActive(true);
                EnemyMaxHP.gameObject.SetActive(true);
                EnemyLvl.gameObject.SetActive(true);
                
                EnemyHP.fillAmount = 1 - (enemy.MaxHitPoints - enemy.HitPoints) / enemy.MaxHitPoints;
                EnemyLvl.text = "Lvl: " + enemy.Level.ToString("0") + "   " + "" + enemy.HitPoints.ToString("0") + "/" + enemy.MaxHitPoints.ToString("0");
            } 
        }
    }  
    private IEnumerator MoveDown()
    {
        
        for(;;) {
            if (_agent.baseOffset <= -0.05)
                yield break;
            _agent.baseOffset -= 0.0001f;
            yield return null;
        }
    }

    private IEnumerator Death()
    {
        _animator.SetTrigger("death");
        yield return new WaitForSeconds(5f);
        StartCoroutine(MoveDown());
    }
    
    private IEnumerator Attack()
    {
        
        while (Input.GetKey("mouse 0"))        
       {
           if (!IsAlive)
               yield break;
            if (_target)
            {
                var enemy = _target.GetComponent<EnemyLogic>();
                if (enemy)
                {
                    transform.LookAt(_target.transform.position);
                    _animator.SetTrigger("meelee");
                    if (!_target.GetComponent<EnemyLogic>().TakeDamage(GetDamage()))
                    {
                        Xp += enemy.XPHolds;
                        if (Xp >= NextLevel)
                            LevelUp();
                        _target = null;
                        yield break;
                    }
                    yield return new WaitForSeconds(10f / AttackSpeed); 
                }
                else
                {
                    yield break;
                }
            }
            else
            {
                yield break;
            }
            yield return null;
        }
        yield return null;
    }

    private IEnumerator PrepareAttack()
    {
        _agent.destination = _target.transform.position;
        var dist = Vector3.Distance(_target.transform.position, transform.position);
        while (dist > AttackRange)
        {
            if (_target != null)
                dist = Vector3.Distance(_target.transform.position, transform.position);
            else
                yield break;
            yield return null;
        }
        var enemy = _target.GetComponent<EnemyLogic>();
        transform.LookAt(_target.transform.position);
        _agent.destination = transform.position;
        _animator.SetTrigger("meelee");
        if (!_target.GetComponent<EnemyLogic>().TakeDamage(GetDamage()))
        {
            Xp += enemy.XPHolds;
            if (Xp >= NextLevel)
                LevelUp();
            _target = null;
            yield break;
        }
        yield return new WaitForSeconds(10f / AttackSpeed);
        StartCoroutine(Attack());
    }

    
    public void TakeDamage(float damage)
    {
        Debug.Log("PlayerTakeDamage. Hp: " + HitPoints);
        if (!IsAlive)
            return;
        HitPoints -= damage;
        if (HitPoints <= 0)
        {
            HitPoints = 0;
            IsAlive = false;
            Die();
        }
    }
    
    private float GetDamage()
    {                
        var baseDamage = Random.Range(MinDmg, MaxDmg);
        if (_target != null)
        {
            var enemy = _target.GetComponent<EnemyLogic>();
            if (enemy != null)
            {
                baseDamage = baseDamage * (1 - enemy._armor / 200f);
                var chance = 75 + _agi - enemy._agi;
                if (Random.Range(0f, 100f) < chance)
                    baseDamage *= 0;
            }
        }
        return baseDamage;
    }

    private void LevelUp()
    {
        Xp = 0;
        NextLevel += 10;
        Level++;
        StatPoints += 5;
        HitPoints = MaxHitPoints;
        if (_armor < 190)
            _armor += 10;
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
        Xp = 0;
        NextLevel = 100;
    }

    public void UpSTR()
    {
        if (StatPoints <= 0)
            return;
        StatPoints--;
        _str++;
        MinDmg = _str / 2;
        MaxDmg = MinDmg + 4;
    }
    public void UpAGI()
    {
        if (StatPoints <= 0)
            return;
        StatPoints--;
        _agi++;
    }
    public void UpCON()
    {
        if (StatPoints <= 0)
            return;
        StatPoints--;
        _con++;
        MaxHitPoints = _con * 5;
    }
    
    public void Plus()
    {
        _statVisible = true;
    }
}
