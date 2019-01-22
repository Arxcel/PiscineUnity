using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FanLogic : MonoBehaviour
{
    public ParticleSystem Smoke;
    public bool _canActivate;
    public Text Tooltip;
    private Color tmp ;

    public GameObject Door;
    
    public CharMovement pl;
    // Start is called before the first frame update
    private void Start()
    {
        if (Smoke)
            Smoke.Stop();
        tmp = Tooltip.color;
        tmp.a = 0;
        Tooltip.color = tmp;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_canActivate && Input.GetKeyDown(KeyCode.E))
        {
            if (Smoke)
            {
                Debug.Log("Activate fan");
                if(Smoke.isPlaying)
                    Smoke.Stop();
                else
                    Smoke.Play();
            }
            if (CompareTag("deactivator"))
            {
                    if (pl.hasDeactivator)
                    {
                        Destroy(Door.transform.gameObject);
                        pl.hasDeactivator = false;
                    }
            }
            else if (CompareTag("doorLock"))
            {
                if (pl.hasCard)
                {
                    pl.hasCard = false;
                    Destroy(Door.transform.gameObject);
                }
            }
            else if (CompareTag("finish"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if (_canActivate)
        {
            if(tmp.a < 1)
                tmp.a += 0.05f;
        }
        else
        {
            if(tmp.a > 0)
                tmp.a -= 0.05f;
        }
        Tooltip.color = tmp;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        Debug.Log("Collision " + tag);
        if (CompareTag("deactivator"))
        {
            if (!pl.hasDeactivator)
            {
                Tooltip.text = "Need Deactivator to do open";
            }
            else
            {
                Tooltip.text = "Press E to Deactivate laser";
            }
        }
        else if (CompareTag("doorLock"))
        {
            if (!pl.hasCard)
            {
                Tooltip.text = "Need Card to open";
            }
            else
            {
                Tooltip.text = "Press E to open the door";
            }
        }
        else if (CompareTag("finish"))
        {
             Tooltip.text = "Press E to take Documents"; 
        }
        else
        {
            Tooltip.text = "Press E to activate";
        }
        
        if (other.transform.CompareTag("Player"))
            _canActivate = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Player"))
            _canActivate = false;
    }
}
