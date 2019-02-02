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
    private GameObject _upgradeMenu;
    
    private bool _isLevelFinished;
    private bool _isPaused;
    private bool _isContextMenuOn;
    private string[] _rankes = { "A", "B", "C", "D", "S", "SS"};


    private GameObject _upgrade;

    private GameObject _downgrade;
    public towerScript[] Towers;

    private GameObject _old;
    // Start is called before the first frame update
    void Start()
    {
        _playerHp = transform.Find("PlayerHp").gameObject.GetComponent<Text>();
        _playerEnergy = transform.Find("PlayerEnergy").gameObject.GetComponent<Text>();
        _pauseMenu = transform.Find("PauseMenu").gameObject;
        _confirmMenu = transform.Find("ConfirmMenu").gameObject;
        _victoryMenu = transform.Find("VictoryMenu").gameObject;
        _lossMenu = transform.Find("LossMenu").gameObject;
        _upgradeMenu = transform.Find("UpMenu").gameObject;
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

        if (Input.GetMouseButtonDown(1) && !_isContextMenuOn)
        {
            ShowUpgradeMenu();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            var hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
            if (hit && hit.collider.transform.CompareTag("empty")) {
                gameManager.gm.playerEnergy -= Towers[0].energy;
                Instantiate (Towers[0], hit.collider.gameObject.transform.position, Quaternion.identity);			
            }
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            var hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
            if (hit && hit.collider.transform.CompareTag("empty")) {
                gameManager.gm.playerEnergy -= Towers[1].energy;
                Instantiate (Towers[1], hit.collider.gameObject.transform.position, Quaternion.identity);			
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
            if (hit && hit.collider.transform.CompareTag("empty")) {
                gameManager.gm.playerEnergy -= Towers[2].energy;
                Instantiate (Towers[2], hit.collider.gameObject.transform.position, Quaternion.identity);			
            }
        } 
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


    public void ShowUpgradeMenu()
    {
        var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.transform.CompareTag("tower"))
        {
            var up = hit.transform.gameObject.GetComponent<towerScript>().upgrade;
            var down = hit.transform.gameObject.GetComponent<towerScript>().downgrade;
            if (up)
            {
                _upgrade = up;
                _old = hit.transform.gameObject;
                _upgradeMenu.transform.Find("Up/UpCost").GetComponent<Text>().text = up.gameObject.GetComponent<towerScript>().energy + "";
            }
            else
            {
                _upgradeMenu.transform.Find("Up/UpCost").GetComponent<Text>().text = "";
            }

            if (down)
            {
                _downgrade = down;
                _old = hit.transform.gameObject;
                _upgradeMenu.transform.Find("Down/DownCost").GetComponent<Text>().text = down.gameObject.GetComponent<towerScript>().energy / 2  + "";
            }
            else
            {
                _old = hit.transform.gameObject;
                _upgradeMenu.transform.Find("Down/DownCost").GetComponent<Text>().text = _old.GetComponent<towerScript>().energy / 2 + "";
            }
            _isContextMenuOn = true;
            _upgradeMenu.transform.position = hit.transform.position;
            _upgradeMenu.SetActive(_isContextMenuOn);
        }

    }

    public void UpgradeTower()
    {
        if (_upgrade && _upgrade.GetComponent<towerScript>().energy <= gameManager.gm.playerEnergy)
        {
            gameManager.gm.playerEnergy -= _upgrade.GetComponent<towerScript>().energy;
            Instantiate(_upgrade, _old.transform.position, Quaternion.identity);
            Destroy(_old);
            _isContextMenuOn = false;
            _upgradeMenu.SetActive(_isContextMenuOn);
        }
    }

    public void ContextMenuCancel()
    {
        _isContextMenuOn = false;
        _upgradeMenu.SetActive(_isContextMenuOn);
    }
    
    public void DowngradeTower()
    {
        if (_downgrade)
        {
            gameManager.gm.playerEnergy += _downgrade.GetComponent<towerScript>().energy / 2;
            Instantiate(_downgrade, _old.transform.position, Quaternion.identity);
            Destroy(_old);
            _isContextMenuOn = false;
            _upgradeMenu.SetActive(_isContextMenuOn);
        } 
        else if (_old)
        {
            gameManager.gm.playerEnergy += _old.GetComponent<towerScript>().energy / 2;
            Destroy(_old);
            _upgradeMenu.SetActive(_isContextMenuOn);
            _isContextMenuOn = false;
            _upgradeMenu.SetActive(_isContextMenuOn);
        }

    }
}
