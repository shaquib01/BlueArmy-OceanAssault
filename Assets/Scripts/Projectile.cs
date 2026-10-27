using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float lifetime = 3f;
    [SerializeField] GameObject impactEffect;
    float speed = 24f;
    int damage = 1;
    Transform owner;
    bool spent;

    public void Initialize(float moveSpeed, int hitDamage, Transform shooterRoot)
    {
        speed = moveSpeed; damage = Mathf.Max(1, hitDamage); owner = shooterRoot;
        Destroy(gameObject, lifetime);
    }

    void Update() => transform.position += transform.forward * speed * Time.deltaTime;

    void OnTriggerEnter(Collider other)
    {
        if (spent || (owner && other.transform.root == owner)) return;
        Damageable target = other.GetComponentInParent<Damageable>();
        if (!target) return;
        spent = true;
        target.TakeDamage(damage);
        if (impactEffect) Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
