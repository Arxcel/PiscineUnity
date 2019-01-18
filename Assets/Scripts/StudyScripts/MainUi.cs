using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUi : MonoBehaviour
{
    private Text _playerHp;
    private Text _playerEnergy;

    private float _currentSpeed;

    private GameObject _pauseMenu;
    private GameObject _confirmMenu;
    private GameObject _victoryMenu;
    private GameObject _lossMenu;

    private bool _isLevelFinished;
    private bool _isPaused;

    private string[] _rankes = { "A", "B", "C", "D", "S", "SS"};
    // Start is called before the first frame update
    void Start()
    {
        _playerHp = transform.Find("PlayerHp").gameObject.GetComponent<Text>();
        _playerEnergy = transform.Find("PlayerEnergy").gameObject.GetComponent<Text>();
        _pauseMenu = transform.Find("PauseMenu").gameObject;
        _confirmMenu = transform.Find("ConfirmMenu").gameObject;
        _victoryMenu = transform.Find("VictoryMenu").gameObject;
        _lossMenu = transform.Find("LossMenu").gameObject;
        gameManager.gm.changeSpeed(_currentSpeed);
    }

    // Update is called once per frame
    private void Update()
    {
        _playerHp.text = gameManager.gm.playerHp + "";
        _playerEnergy.text = gameManager.gm.playerEnergy + "";
        
        if (!_isPaused && Input.GetKeyDown(KeyCode.Escape) || _isLevelFinished)
            pause();
        _isLevelFinished = LevelFinished() || gameManager.gm.playerHp <= 0;
        if (Input.GetKeyDown(KeyCode.N))
            NextLevel();
    }
    
    public void accelerate0()
    {
        _currentSpeed = 1f;
        gameManager.gm.changeSpeed(_currentSpeed);
    }

    public void accelerate1() {
        _currentSpeed = 2.5f;
        gameManager.gm.changeSpeed(_currentSpeed);    }

    public void accelerate2() {
        _currentSpeed = 5f;
        gameManager.gm.changeSpeed(_currentSpeed);    }

    public void pause() {
        if (_isLevelFinished && gameManager.gm.playerHp > 0)
        {
            _victoryMenu.transform.Find("Score").GetComponent<Text>().text = "Score: " + gameManager.gm.score;
            _victoryMenu.transform.Find("Rank").GetComponent<Text>().text = _rankes[(gameManager.gm.score + gameManager.gm.playerHp) % 4];
            _victoryMenu.SetActive(true);
            
        }
        else if (_isLevelFinished && gameManager.gm.playerHp <= 0)
        {
            _lossMenu.transform.Find("Score").GetComponent<Text>().text = "Score: " + gameManager.gm.score;
            _lossMenu.SetActive(true);
        }
        else
            _pauseMenu.SetActive(true);
        gameManager.gm.pause(true);
        _isPaused = true;
    }
    
    public void resume() {
        _pauseMenu.SetActive(false);
        gameManager.gm.pause(false);
        _isPaused = false;
        gameManager.gm.changeSpeed(_currentSpeed);
    }

    public void confirmExit()
    {
        _pauseMenu.SetActive(false);
        _confirmMenu.SetActive(true);
    }

    public void confirmOk()
    {
        Application.Quit();
    }
    
    public void confirmCancel()
    {
        _confirmMenu.SetActive(false);
        _pauseMenu.SetActive(true);
    }
     
    private bool LevelFinished() {
        if (gameManager.gm.lastWave) {
           var spawners = GameObject.FindGameObjectsWithTag("spawner");
            foreach (var spawner in spawners) {
                if (spawner.GetComponent<ennemySpawner>().isEmpty == false || spawner.transform.childCount > 1) {
                    return false;
                }
            }

            return true;
        }
        return false;
    }

    public void NextLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    public void Retry()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
