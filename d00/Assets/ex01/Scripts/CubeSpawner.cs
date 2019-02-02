using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject[] Cubes;
    public float SpawnTime;

    private float _timer;
    private float[] _possibleX = new float[] {-1.41f, 0f, 1.41f};
    private static int[] _cubeCounter = new int[]{0,0,0};
    private int _rand;

    void Update ()
    {
        if (_timer >= SpawnTime) {
            var rand = Random.Range (0, 3);
            if (_cubeCounter[rand] == 0) {
                _timer = 0;
                _cubeCounter[rand]++;
                var newPos = new Vector3 (_possibleX[rand], 2f, 1f);
                Instantiate (Cubes[rand], newPos, Quaternion.identity);
            }
        }
        _timer += Time.deltaTime;

    }

    public static void DeleteCube(int index)
    {
        _cubeCounter[index]--;
    }
}