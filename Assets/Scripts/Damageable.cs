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
    public int Health => health;
    public int MaxHealth => maxHealth;
    public bool IsBoss => boss;

    void Awake() { health = Mathf.Max(1, maxHealth); onHealthChanged?.Invoke(health, maxHealth); }
    public void TakeDamage(int amount)
    {
        if (defeated || amount <= 0) return;
        health = Mathf.Max(0, health - amount);
        onHealthChanged?.Invoke(health, maxHealth);
        if (boss && GameManager.Instance) GameManager.Instance.SetBossHealth(health, maxHealth);
        if (health == 0) Defeat();
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
