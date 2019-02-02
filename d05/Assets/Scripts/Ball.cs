using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private bool _isMoving;

    public CameraMovement MainCamera;

    public GameObject[] Holes;
    public GameObject[] BasicPositions;

    private int _currentHole;

    private int _numberHits;
    public Image ProgressBar;
    public Text CurrnetHole;
    public Text NumberHits;
    public Text Club;
    public Text ClubPower;


    public Text[] HoleHits;
    public Text[] HoleNumbers;
    public Image TabScreen;
    public Image WinScreen;

    private bool _isUp;
    private bool _isPrepareHit;
    private bool _isHit;
    private float _force;
    private bool _canHit;

    private bool _isBallMoveMode;
    private bool _isPowerSelectionMode;
    private bool _isDirectionSelectionMode;
    private int[] _holeHits = {0,0,0};
    private Mode _m = Mode.DirectionSelectionMode;
    enum Mode
    {
        BallMoveMode,
        PowerSelectionMode,
        DirectionSelectionMode,
        FlyMode,
        TabMode,
        WinMode
    }


    private int _currentClub;

    private string[] _clubs = {"wooden", "iron", "wedge"};
    private float[] _clubPower = {0.4f, 0.6f, 0.8f};
    
    private void Start()
    {
        foreach (var hole in Holes)
        {
            hole.SetActive(false);
        }
        Holes[_currentHole].SetActive(true);
        transform.position = BasicPositions[_currentHole].transform.position;
        MainCamera.MoveToBall(transform.position, Holes[_currentHole].transform.position);
    }

    private void Update()
    {
        ProgressBar.transform.gameObject.SetActive(false);
        TabScreen.transform.gameObject.SetActive(false);    
        WinScreen.transform.gameObject.SetActive(false);                
        MainCamera.IsFlyMode = false;
        MainCamera.IsSnipeMode = false;
        switch (_m)
        {
            case Mode.BallMoveMode:
                _isMoving = GetComponent<Rigidbody>().velocity != Vector3.zero;
                if (!_isMoving)
                {
                    MainCamera.MoveToBall(transform.position, Holes[_currentHole].transform.position);
                    _m = Mode.DirectionSelectionMode;
                }
                break;
            case Mode.DirectionSelectionMode:
                MainCamera.IsSnipeMode = true;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _m = Mode.PowerSelectionMode;
                }
                else if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    _m = Mode.FlyMode;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if(_currentClub - 1 >= 0)
                        _currentClub--;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if(_currentClub + 1 < 3)
                        _currentClub++;
                }
                
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    _m = Mode.TabMode;
                }
                break;
            case Mode.PowerSelectionMode:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Hit();
                    _m = Mode.BallMoveMode;
                }
                ProgressBar.transform.gameObject.SetActive(true);
                PrepareHit();
                break;
            case Mode.FlyMode:
                MainCamera.IsFlyMode = true;
                if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Space))
                {
                    MainCamera.MoveToBall(transform.position, Holes[_currentHole].transform.position);
                    _m = Mode.DirectionSelectionMode;
                }
                break;
            case Mode.TabMode:
                TabScreen.transform.gameObject.SetActive(true);                
                if (Input.GetKeyUp(KeyCode.LeftAlt))
                {
                    _m = Mode.DirectionSelectionMode;
                }
                break;
            case Mode.WinMode:
                WinScreen.transform.gameObject.SetActive(true);                
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    _m = Mode.DirectionSelectionMode;
                }
                break;
        }

        CurrnetHole.text = "Current Hole: " + (_currentHole + 1);
        NumberHits.text = "Total hits: " + _numberHits;
        Club.text = "Current club: " + _clubs[_currentClub];
        ClubPower.text = "Club Power: " + _clubPower[_currentClub];
        for (var i = 0; i < HoleNumbers.Length; ++i)
        {
            HoleNumbers[i].text = _holeHits[i].ToString();
            HoleHits[i].text = i.ToString();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        FinishRound();
    }


    private void PrepareHit()
    {
        ProgressBar.fillAmount += _isUp ? Time.deltaTime : -Time.deltaTime;
        if (ProgressBar.fillAmount <= 0)
            _isUp = true;
        if (ProgressBar.fillAmount >= 1)
            _isUp = false;
        _force = ProgressBar.fillAmount;
    }

    private void FinishRound()
    {
        if (_currentHole < 2)
            _currentHole++;
        transform.position = BasicPositions[_currentHole].transform.position;
        MainCamera.MoveToBall(transform.position, Holes[_currentHole].transform.position);
        foreach (var hole in Holes)
        {
            hole.SetActive(false);
        }
        Holes[_currentHole].SetActive(true);
        _m = Mode.WinMode;
    }

    private void Hit()
    {
        var dir = MainCamera.transform.forward;
        dir.Normalize();
        dir = new Vector3(dir.x, _clubPower[_currentClub], dir.z);
        dir.Normalize();
        dir *= _force;
        dir *= 100;
        _numberHits++;
        _holeHits[_currentHole]++;
        GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
    }
}
