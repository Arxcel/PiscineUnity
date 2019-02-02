using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public Text NumberHPText;

    private int _hp = 100;

    public ParticleSystem GH;

    private void Update ()
    {
        NumberHPText.text = _hp.ToString();
    }

    public void GetHit(int type)
    {
        if (type == 1 && !GH.isPlaying)
        {
            _hp -= 10;
            GH.Play();
        }
        else
        {
            _hp -= 1;
        }
        if (_hp <= 0)
            Destroy(transform.gameObject);
    }
}
