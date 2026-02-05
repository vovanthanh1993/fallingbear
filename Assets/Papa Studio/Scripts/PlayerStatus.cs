using System;
using System.Collections;
using GamePolygon;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public Action onPlayerDied;
    [SerializeField] private float currentHealth;
    [SerializeField] private float gravityMultiplier;

    [SerializeField] private bool isImmortal = false;

    [SerializeField] Transform HealthUI;

    public void SetCurrentCharacterStatus(Character characterData)
    {
        currentHealth = characterData.characterHealth;
        gravityMultiplier = characterData.gravityMultiplier;
        GameUI.instance.UpdateHealthUI(currentHealth);
    }

    public void PlayerTakeDamage(int amount)
    {
        if (isImmortal) return;
        SoundManager.Instance.PlaySound(SoundManager.Instance.GameOverFx);
        currentHealth -= amount;
        GameUI.instance.UpdateHealthUI(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            onPlayerDied?.Invoke();
        }
    }

    public void PlayerKillZone()
    {
        currentHealth = 0;
        GameUI.instance.UpdateHealthUI(currentHealth);
        onPlayerDied?.Invoke();
    }

    public void PlayerGetImmortal(float timeBuff)
    {
        StartCoroutine(ImmortalBuff(timeBuff));
    }

    IEnumerator ImmortalBuff(float time)
    {
        isImmortal = true;
        yield return new WaitForSecondsRealtime(time);
        isImmortal = false;
    }
}
