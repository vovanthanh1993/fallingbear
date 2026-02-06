using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    GameObject Nuke;
    public float triggerCooldown = 1f;
    private bool canTrigger = true;

    void Start()
    {
        Nuke = GameObject.Find("NukeExplosion");

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnTriggerEnter(Collider collision)
    {
        if (!GameManager.instance.GameEnd && canTrigger)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stick"))
            {

                Nuke.transform.position = collision.gameObject.transform.position;
                Nuke.GetComponent<ParticleSystem>().Play();
                StartCoroutine(TriggerCooldown());

                GameManager.instance.PlayerTakeDmg();
            }
        }

    }
    
    private IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }
}
