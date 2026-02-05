using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    GameObject Nuke;
    ParticleSystem nukeParticleSystem;
    public float triggerCooldown = 1f;
    private bool canTrigger = true;

    void Start()
    {
        Nuke = GameObject.Find("NukeExplosion");
        // Cache ParticleSystem để tránh GetComponent mỗi lần trigger (tối ưu cho WebGL)
        if (Nuke != null)
        {
            nukeParticleSystem = Nuke.GetComponent<ParticleSystem>();
        }
    }


    public void OnTriggerEnter(Collider collision)
    {
        if (!GameManager.instance.GameEnd && canTrigger)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Stick"))
            {

                Nuke.transform.position = collision.gameObject.transform.position;
                if (nukeParticleSystem != null)
                {
                    nukeParticleSystem.Play();
                }
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
