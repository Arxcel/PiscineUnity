using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisions : MonoBehaviour
{
    public CharMovement Player;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("OnParticleCollision");
        if (other.CompareTag("Player"))
        {
            Player._isVisible = false;
        }
    }
}
