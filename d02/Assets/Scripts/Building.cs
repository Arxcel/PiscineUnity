using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Building : MonoBehaviour
{
    public float HitPoints;
    private float _currentHitPoints;
    private bool _isAlive = true;
    public bool IsMain;

    public delegate void BuildingEvent();

    public event BuildingEvent OnMainBuildingAttacked;
    
    private void Start()
    {
        _currentHitPoints = HitPoints;
    }

    public void TakeDamage(float damage)
    {
        if (IsMain)
            OnMainBuildingAttacked();
        _currentHitPoints -= damage;
        if(_currentHitPoints <= 0)
            Die();
    }

    private void Die()
    {
        _isAlive = false;
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    private void LateUpdate()
    {

        if (!_isAlive)
        {

            if (IsMain)
            {
                if (transform.CompareTag("aliance"))
                    Debug.Log("Horde Wins");
                else
                    Debug.Log("Alliance Wins");
                var scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            Destroy(transform.gameObject);
        }
        

    }

    public float GetHP()
    {
        return HitPoints;
    }

    public float GetCurrentHP()
    {
        return _currentHitPoints;
    }

}
