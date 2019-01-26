using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    /***************************************/
    /*************Player stats**************/
    /***************************************/
    [HideInInspector]public float _str = 20;
    [HideInInspector]public float _agi = 20;
    [HideInInspector]public float _con = 20;
    [HideInInspector]public float _armor = 20;
    [HideInInspector]public float MaxHitPoints;
    [HideInInspector]public float HitPoints;
    [HideInInspector]public float OrigMinDmg;
    [HideInInspector]public float OrigMaxDmg;   
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
    [HideInInspector]public float StatPoints;

    public List<Item> _inventory = new List<Item>();

    
    
    /***************************************/
    /*****************UI********************/
    /***************************************/
    public Text HpText;
    public Text XpText;
    public Text LevelText;

    public Text EnemyLvl; 
    public Text STRTEXT; 
    public Text AGITEXT; 
    public Text CONTEXT; 
    public Text StatPointsText; 
    public Text DmgText; 
    public Text ArmorText; 
    public Text ValetsText;
    
    public Image InventoryMenu;
    public Image StatMenu;
    public Image HpAmoumt; 
    public Image XpAmoumt;
    public Image EnemyHP;
    public Image EnemyMaxHP;
    public Sprite DefaultIcon;
    public Button[] Pluses;
    
    private bool _statVisible;
    private bool _inventoryVisible;

    private Item EquippedItem;
    
    /***************************************/
    /*************Player Logic**************/
    /***************************************/
    private NavMeshAgent _agent;
    private RaycastHit _hit;
    private Camera _cam;
    private GameObject _target;
    private GameObject _selection;
    private Animator _animator;
    private float _dmgTimer;
    public GameObject WayCone;
    public ParticleSystem LevelUpPartickle;

    public GameObject Hand;
    public ParticleSystem GetHitParticle;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _cam = Camera.main;
        InitPlayer();
        WayCone.SetActive(false);
    }

    private void Update()
    {
        if (IsAlive)
        {
            if (Input.GetKeyDown("mouse 0"))
            {
                PlayerGetTarget();
            }
            PlayerMovementAnimation();
            PlayerHotkeys();
        }
        OnHoverEnemy();
        UpdateUI();
    }

    private void PlayerMovementAnimation()
    {
        if (_agent.velocity.magnitude < 0.1f)
        {
            _animator.SetBool("walk", false);
            WayCone.SetActive(false);
            _agent.updateRotation = false;
        }
        else
        {
            _agent.updateRotation = true;
            _animator.SetBool("walk", true);
            WayCone.SetActive(_target == null);
        } 
    }

    private void PlayerGetTarget()
    {
        if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out _hit) && !IsPointerOverUIObject())
        {
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
            else if(_hit.transform.CompareTag("Ground"))
            {
                WayCone.transform.position = new Vector3(_hit.point.x, _hit.point.y + 0.7f, _hit.point.z);
                _agent.destination = _hit.point;
                _target = null;
            }
            _statVisible = false;
        }
    }

    private void PlayerHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _statVisible = !_statVisible;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HitPoints += 10;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventoryVisible = !_inventoryVisible;
        }
    }

    private void OnHoverEnemy()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse X")) >= 0.01 || Mathf.Abs(Input.GetAxis("Mouse Y")) >= 0.01)
        {
            if (_target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out hit) && !IsPointerOverUIObject())
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
        InventoryUI();
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
        EnemyHpBarUI();
    }

    private void InventoryUI()
    {
        InventoryMenu.gameObject.SetActive(_inventoryVisible);
        int i = 0;
        foreach (Transform child in InventoryMenu.transform)
        {
            var btn = child.gameObject.GetComponent<Button>();
            if (btn != null)
            {
                if (i < _inventory.Count)
                {
                    btn.interactable = true;
                    var image = child.GetComponent<Image>();
                    var currItem = _inventory[i++];
                    if (image)
                        image.sprite = currItem.Icon;
                }
                else
                {
                    var image = child.GetComponent<Image>();
                    btn.interactable = false;
                    if (image)
                        image.sprite = DefaultIcon;
                }
            }
        }    
    }

    private void EnemyHpBarUI()
    {
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
        
        while (true)
        {
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
        while (Input.GetKey("mouse 0") && _target != null)
        {
            if (!IsAlive)
            {
                _agent.isStopped = true;
                yield break;
            }
            var targetPosition = _target.transform.position;
            _agent.destination = targetPosition;
            var dist = Vector3.Distance(targetPosition, transform.position);
            while (dist > AttackRange)
            {
                if (!IsAlive)
                {
                    _agent.isStopped = true;
                    yield break;
                }
                _agent.destination = targetPosition;
                dist = Vector3.Distance(targetPosition, transform.position);
                yield return null;
            }
            if (!IsAlive)
            {
                _agent.isStopped = true;
                yield break;
            }
            var enemy = _target.GetComponent<EnemyLogic>();
            _agent.destination = transform.position;
            transform.LookAt(targetPosition);
            _animator.SetTrigger("meelee");
            if(!IsAlive)
                yield break;
            if (!enemy.TakeDamage(CalculateDamage()))
            {
                Xp += enemy.XPHolds;
                if (Xp >= NextLevel)
                    LevelUp();
                _target = null;
                yield break;
            }
            yield return new WaitForSeconds(10f / AttackSpeed); 
        }
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
        if (!IsAlive)
        {
            _agent.isStopped = true;
            yield break;
        }
        var enemy = _target.GetComponent<EnemyLogic>();
        _agent.destination = transform.position;
        transform.LookAt(_target.transform.position);
        _animator.SetTrigger("meelee");
        if (!enemy.TakeDamage(CalculateDamage()))
        {
            Xp += enemy.XPHolds;
            Money += enemy.MoneyHolds;
            if (Xp >= NextLevel)
                LevelUp();
            _target = null;
            yield break;
        }
        yield return new WaitForSeconds(10f / AttackSpeed);
        StartCoroutine(Attack());
    }
 
    public bool TakeDamage(float damage)
    {
        if (!IsAlive)
            return IsAlive;
        HitPoints -= damage;
        if (damage > 0)
            GetHitParticle.Play();
        if (HitPoints <= 0)
        {
            HitPoints = 0;
            IsAlive = false;
            StopAllCoroutines();
            Die();
        }
        return IsAlive;
    }
    
    private float CalculateDamage()
    {                
        var baseDamage = Random.Range(MinDmg, MaxDmg);
        if (_target != null)
        {
            var enemy = _target.GetComponent<EnemyLogic>();
            if (enemy != null)
            {
                baseDamage = baseDamage * (1 - enemy._armor / 200f);
                var chance = 75 + _agi - enemy._agi;
                if (Random.Range(0f, 100f) >= chance)
                    baseDamage *= 0;
            }
        }
        return baseDamage;
    }

    private void LevelUp()
    {
        Xp = 0;
        NextLevel *= 1.5f;
        Level++;
        StatPoints += 5;
        HitPoints = MaxHitPoints;
        if (_armor < 195)
            _armor += 2;
        LevelUpPartickle.Play();
    }

    private void Die()
    {
        StartCoroutine(Death());
    }
    
    private void InitPlayer()
    {
        MaxHitPoints = _con * 5;
        HitPoints = MaxHitPoints;
        OrigMinDmg = _str / 2;
        OrigMaxDmg = MinDmg + 4;
        MinDmg = OrigMinDmg;
        MaxDmg = OrigMaxDmg;
        AttackSpeed = 10f;
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
        
        OrigMinDmg = _str / 2;
        OrigMaxDmg = MinDmg + 4;
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
       
    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        var eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var item = other.transform.gameObject.GetComponent<Item>();
            if (item)
            {
                if (_inventory.Count < 16)
                {
                    var pickingItem = item.Model.GetComponent<Item>();
                    if(pickingItem)
                        _inventory.Add(pickingItem);
                    Destroy(item.transform.gameObject);
                }
            }
        }
    }

    private void UseItem(Item i)
    {
        if (i.Type == "HpPotion")
        {
            var amount = MaxHitPoints * 0.3f;
            if (HitPoints + amount > MaxHitPoints)
                HitPoints = MaxHitPoints;
            else
                HitPoints += amount;
        } else if (i.Type == "Weapon")
        {
            foreach (Transform item in Hand.transform)
            {
                var it = item.gameObject.GetComponent<Item>();
                if (it)
                {
                    _inventory.Add(it.Model.GetComponent<Item>());
                }
                Destroy(item.gameObject);
            }
            var newWeapon = Instantiate(i, transform);
            EquippedItem = newWeapon.GetComponent<Item>();
            MinDmg = OrigMinDmg * EquippedItem.Multiplier;
            MaxDmg = OrigMaxDmg * EquippedItem.Multiplier;
            AttackSpeed = EquippedItem.AttackSpped * 10f;
            newWeapon.transform.parent = Hand.transform;
            newWeapon.transform.localPosition = EquippedItem.PickingPosition;
            newWeapon.transform.localRotation = Quaternion.Euler(EquippedItem.PickingRotation);
            newWeapon.transform.localScale = EquippedItem.PickingScale;
        }
    }

    public void UseInventoryItem1()
    {
        var item = _inventory[0];
        UseItem(item);
        Debug.Log("slot 0");
        _inventory.RemoveAt(0);
    }
    public void UseInventoryItem2()
    {
        var item = _inventory[1];
        UseItem(item);
        Debug.Log("slot 1");
        _inventory.RemoveAt(1);
    }
    public void UseInventoryItem3()
    {
        var item = _inventory[2];
        UseItem(item);
        _inventory.RemoveAt(2);
    }
    public void UseInventoryItem4()
    {
        var item = _inventory[3];
        UseItem(item);
        _inventory.RemoveAt(3);
    }
    public void UseInventoryItem5()
    {
        var item = _inventory[4];
        UseItem(item);
        _inventory.RemoveAt(4);
    }
    public void UseInventoryItem6()
    {
        var item = _inventory[5];
        UseItem(item);
        _inventory.RemoveAt(5);
    }
    public void UseInventoryItem7()
    {
        var item = _inventory[6];
        UseItem(item);
        _inventory.RemoveAt(6);
    }
    public void UseInventoryItem8()
    {
        var item = _inventory[7];
        UseItem(item);
        _inventory.RemoveAt(7);
    }
    public void UseInventoryItem9()
    {
        var item = _inventory[8];
        UseItem(item);
        _inventory.RemoveAt(8);
    }
    public void UseInventoryItem10()
    {
        var item = _inventory[9];
        UseItem(item);
        _inventory.RemoveAt(9);
    }
    public void UseInventoryItem11()
    {
        var item = _inventory[10];
        UseItem(item);
        _inventory.RemoveAt(10);
    }
    public void UseInventoryItem12()
    {
        var item = _inventory[11];
        UseItem(item);
        _inventory.RemoveAt(11);
    }
    public void UseInventoryItem13()
    {
        var item = _inventory[12];
        UseItem(item);
        _inventory.RemoveAt(12);
    }
    public void UseInventoryItem14()
    {
        var item = _inventory[13];
        UseItem(item);
        _inventory.RemoveAt(13);
    }
    public void UseInventoryItem15()
    {
        var item = _inventory[14];
        UseItem(item);
        _inventory.RemoveAt(14);
    }
    public void UseInventoryItem16()
    {
        var item = _inventory[15];
        UseItem(item);
        _inventory.RemoveAt(15);
    }
}
