using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] bool boss;
    [SerializeField] int coinReward = 10;
    [SerializeField] GameObject defeatEffect;
    [SerializeField] UnityEvent<int, int> onHealthChanged;
    [SerializeField] UnityEvent onDefeated;
    int health;
    bool defeated;
    Vector3 baseScale;
    Coroutine hitFeedback;
    public int Health => health;
    public int MaxHealth => maxHealth;
    public bool IsBoss => boss;

    void Awake()
    {
        // Normalize invalid Inspector values once so health and UI events always
        // agree on a positive maximum health value.
        maxHealth = Mathf.Max(1, maxHealth);
        health = maxHealth;
        baseScale = transform.localScale;
        onHealthChanged?.Invoke(health, maxHealth);
    }
    public void TakeDamage(int amount)
    {
        if (defeated || amount <= 0) return;
        health = Mathf.Max(0, health - amount);
        if (health > 0) PlayHitFeedback();
        onHealthChanged?.Invoke(health, maxHealth);
        if (boss && GameManager.Instance) GameManager.Instance.SetBossHealth(health, maxHealth);
        if (health == 0) Defeat();
    }
    void PlayHitFeedback()
    {
        if (!isActiveAndEnabled) return;
        if (hitFeedback != null) StopCoroutine(hitFeedback);
        hitFeedback = StartCoroutine(HitPulse());
    }

    IEnumerator HitPulse()
    {
        const float duration = 0.14f;
        const float intensity = 0.09f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float pulse = Mathf.Sin(Mathf.Clamp01(elapsed / duration) * Mathf.PI);
            transform.localScale = baseScale * (1f + intensity * pulse);
            yield return null;
        }
        transform.localScale = baseScale;
        hitFeedback = null;
    }

    void Defeat()
    {
        if (defeated) return;
        defeated = true;
        if (defeatEffect) Instantiate(defeatEffect, transform.position, Quaternion.identity);
        if (GameManager.Instance) GameManager.Instance.TargetDefeated(this, coinReward, boss);
        onDefeated?.Invoke();
        Destroy(gameObject, 0.1f);
    }
}
