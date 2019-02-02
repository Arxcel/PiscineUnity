using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	public Building Town;
	private UnitManager _manager;
	public float Speed;
	public float Damage = 3;
	public float HitPoints = 9;
	private float _currentHitPoints;
	public AudioClip[] SelectSounds;
	public AudioClip[] WalkSounds;
	public bool Alliance;
	private enum CharacterDirection {
		Up,
		Down,
		Right,
		Left,
		UpRight,
		UpLeft,
		DownRight,
		DownLeft,
		Stay
	};
    private bool _isSelected;
    private bool _isAttacking;
	private bool _isMoving;
    private Vector3 _endPoint;

	private bool _canSelect;
	
    private CharacterDirection _direction = CharacterDirection.Stay;
	private GameObject _currentEnemyAttacked;
	private float _nextAttack;
	private bool _canAttack;

	private bool _isAlive = true;
    // Start is called before the first frame update
    private void Start()
    {
	    _manager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
	    _manager.AddUnit(this);
	    _currentHitPoints = HitPoints;
	    if (transform.CompareTag("ork"))
		    ChangeEndpoint(new Vector3(-6, 0, 0));
    }

	private void OnEnable()
	{
		if (Town)
			Town.OnMainBuildingAttacked += AttackListener;
	}

	private void OnDisable()
	{
		if (Town)
			Town.OnMainBuildingAttacked += AttackListener;
	}

	private void AttackListener()
	{
		if (this && transform.CompareTag("ork"))
			ChangeEndpoint(Town.transform.position);
	}

	private void OnMouseEnter()
	{
		_canSelect = true;

	}

	private void OnMouseExit()
	{
		_canSelect = false;
	}

	// Update is called once per frame
    private void Update()
    {
	    if (Time.time > _nextAttack)
		    _canAttack = true;

	    if (_currentEnemyAttacked && _canAttack)
	    {
		    AttackEnemy(_currentEnemyAttacked);
	    }

	    
	    if (_direction != CharacterDirection.Stay )
		    transform.position = Vector3.MoveTowards(transform.position, _endPoint, Speed * Time.deltaTime);
	    
	    if (transform.position == _endPoint && _direction != CharacterDirection.Stay) {
		    ActivateTrigger("stay");
		    ChangeDirection(CharacterDirection.Stay);
	    }

    }
    
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("environment"))
			_endPoint = transform.position;
		
		if (other.transform.CompareTag("ork") && Alliance || other.transform.CompareTag("aliance") && !Alliance)
		{
			if (!_isAttacking)
				_currentEnemyAttacked = other.transform.gameObject;
			_isAttacking = true;
			ActivateTrigger("stay");
			ActivateTrigger("attack");
			ChangeEndpoint (other.transform.position);
			_endPoint = transform.position;
		}	
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (transform.CompareTag("ork"))
			ChangeEndpoint(new Vector3(-6, 0, 0));
		_isAttacking = false;
	}
	
	private void ChangeDirection(CharacterDirection direction) {
		_direction = direction;
	}
	
	public void ChangeEndpoint(Vector3 endpoint) {
		if (WalkSounds.Length > 0)
		{
			GetComponent<AudioSource>().clip = WalkSounds[Random.Range(0, WalkSounds.Length)];
			GetComponent<AudioSource>().Play();
		}
		_endPoint = endpoint;
		var newDirection = new Vector3 (endpoint.x - transform.position.x, endpoint.y - transform.position.y, 0);
		newDirection.Normalize();
		if (newDirection.x >= -0.25 && newDirection.x <= 0.25 && newDirection.y > 0)
		{
			ActivateTrigger ("walk_up");
			ChangeDirection (CharacterDirection.Up);
		} else if (newDirection.x >= -0.25 && newDirection.x <= 0.25 && newDirection.y < 0)
		{
			ActivateTrigger ("walk_down");
			ChangeDirection (CharacterDirection.Down);
		} else if (newDirection.x < 0 && newDirection.y >= -0.25 && newDirection.y <= 0.25)
		{
			ActivateTrigger ("walk_left");
			ChangeDirection (CharacterDirection.Left);
		} else if (newDirection.x > 0 && newDirection.y >= -0.25 && newDirection.y <= 0.25)
		{
			ActivateTrigger ("walk_right");					
			ChangeDirection (CharacterDirection.Right);
		} else if (newDirection.x < 0 && newDirection.y > 0)
		{
			ActivateTrigger ("walk_up_left");
			ChangeDirection (CharacterDirection.UpLeft);
		} else if (newDirection.x > 0 && newDirection.y > 0)
		{
			ActivateTrigger ("walk_up_right");
			ChangeDirection (CharacterDirection.UpRight);
		} else if (newDirection.x < 0 && newDirection.y < 0)
		{
			ActivateTrigger ("walk_down_left");
			ChangeDirection (CharacterDirection.DownLeft);
		} else if (newDirection.x > 0 && newDirection.y < 0)
		{
			ActivateTrigger ("walk_down_right");
			ChangeDirection (CharacterDirection.DownRight);
		}
	}

	public void ActivateTrigger(string trigger) {
		GetComponent<Animator>().SetTrigger(trigger);
	}

	public bool IsSelected()
	{
		return _isSelected;
	}

	public void SetSelection(bool isSelected)
	{
		if (transform.CompareTag("ork"))
			return;
		_isSelected = isSelected;
		if (transform.childCount > 0)
		{
			var selection = transform.GetChild(0);
			if(selection)
				selection.gameObject.SetActive(_isSelected);
		}
		if (isSelected)
		{
			GetComponent<AudioSource>().clip = SelectSounds[Random.Range(0, SelectSounds.Length)];
			GetComponent<AudioSource>().Play();
		}
	}

	public bool CanSelect()
	{
		return _canSelect;
	}

	private void AttackEnemy(GameObject enemy)
	{
		if (_isAttacking)
		{
			var unit = enemy.gameObject.GetComponent<Unit>();
			var building = enemy.gameObject.GetComponent<Building>();
			if (unit)
			{
				Debug.Log ((Alliance ? "Orc" : "Alliance") + " unit [" + unit.GetCurrentHP() + "/" + unit.GetHP() + "]HP has been attacked.");
				unit.TakeDamage(Damage);
				if (!unit.IsAlive())
				{
					_currentEnemyAttacked = null;
					_isAttacking = false;
					ActivateTrigger ("stay");
				}
			}
			else if (building)
			{
				Debug.Log ((Alliance ? "Orc" : "Alliance") + " building [" + building.GetCurrentHP() + "/" + building.GetHP() + "]HP has been attacked.");
				building.TakeDamage(Damage);
				if (!building.IsAlive())
				{
					_currentEnemyAttacked = null;
					_isAttacking = false;
					ActivateTrigger ("stay");
				}
			}
			else
			{
				_currentEnemyAttacked = null;
				_isAttacking = false;
				ActivateTrigger ("stay");
			}
		}
		else if (enemy.CompareTag("aliance"))
			ChangeEndpoint (enemy.transform.position);
		_isAttacking = true;
		_canAttack = false;
		_nextAttack = Time.time + 2f;
	}

	public void TakeDamage(float damage)
	{
		_currentHitPoints -= damage;
		if(_currentHitPoints <= 0)
			Die();
	}

	private void Die()
	{
		_isAlive = false;
	}

	private bool IsAlive()
	{
		return _isAlive;
	}

	private void LateUpdate()
	{
		if (!_isAlive)
		{
			OnDisable();
			Destroy(transform.gameObject);
		}

	}

	private float GetHP()
	{
		return HitPoints;
	}

	private float GetCurrentHP()
	{
		return _currentHitPoints;
	}
}
