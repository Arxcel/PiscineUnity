using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	private UnitManager _manager;
	public float Speed;
	public AudioClip[] SelectSounds;
	public AudioClip[] WalkSounds;
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
    private Vector3 _endPoint;

	private bool _canSelect;
	
    private CharacterDirection _direction = CharacterDirection.Stay;

    // Start is called before the first frame update
    private void Start()
    {
	    _manager = GameObject.Find("UnitManager").GetComponent<UnitManager>();
	    _manager.AddUnit(this);
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

	    if (_direction != CharacterDirection.Stay) {

		    transform.position = Vector3.MoveTowards(transform.position, _endPoint, Speed * Time.deltaTime);
	    }
	    
	    if (transform.position == _endPoint && _direction != CharacterDirection.Stay) {
		    ActivateTrigger("stay");
		    ChangeDirection(CharacterDirection.Stay);
	    }

    }
    
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.CompareTag("environment") || other.transform.CompareTag("ork") )
			_endPoint = transform.position;
	}
	
	private void OnCollisionStay2D(Collision2D other)
	{
		if (other.transform.CompareTag("environment") || other.transform.CompareTag("ork"))
			_endPoint = transform.position;
	}
	
	private void ChangeDirection(CharacterDirection direction) {
		_direction = direction;
	}
	
	public void ChangeEndpoint(Vector3 endpoint) {
		GetComponent<AudioSource>().clip = WalkSounds[Random.Range(0, WalkSounds.Length)];
		GetComponent<AudioSource>().Play();
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
		_isSelected = isSelected;
		var selection = transform.GetChild(0);
		if(selection)
			selection.gameObject.SetActive(_isSelected);
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
}
