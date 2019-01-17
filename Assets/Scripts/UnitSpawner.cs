using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public Unit Unit;
    public float SpawnTime;
    public AudioClip[] CreateSounds;
    public Building MainTown;
    public float OffsetX;
    public float OffsetY;
    
    // Update is called once per frame
    private float _timer;
    private void Update()
    {
        if (_timer >= SpawnTime)
        {
            _timer = 0;
            var newPos = new Vector3(transform.position.x + OffsetX, transform.position.y + + OffsetY);
            Unit.Town = MainTown;
            Instantiate (Unit, newPos, Quaternion.identity);
            
            GetComponent<AudioSource>().clip = CreateSounds[Random.Range(0, CreateSounds.Length)];
            GetComponent<AudioSource>().Play();
        }
        _timer += Time.deltaTime;
    }
}
