using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject Target;
    public Bullet Ammo;
    public float FireRate;   
    private float _timer;

    // Update is called once per frame
    void Update()
    {
        if (_timer >= FireRate)
        {
            _timer = 0;
            var newPos = transform.position;
            Ammo.Target = Target.transform.position;
            Instantiate (Ammo, newPos, Quaternion.identity);
            
        }
        _timer += Time.deltaTime;
    }
}
