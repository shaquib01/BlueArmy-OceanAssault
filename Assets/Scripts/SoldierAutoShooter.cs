using UnityEngine;

public class SoldierAutoShooter : MonoBehaviour
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform muzzle;
    [SerializeField] float fireRate = 1.5f;
    [SerializeField] float projectileSpeed = 24f;
    [SerializeField] int damage = 2;
    [SerializeField] float targetRange = 55f;
    [SerializeField] LayerMask targetLayers = ~0;
    float nextShot;

    void Update()
    {
        if (!projectilePrefab || Time.time < nextShot) return;
        Transform origin = muzzle ? muzzle : transform;
        if (!Physics.Raycast(origin.position, Vector3.forward, out RaycastHit hit, targetRange, targetLayers, QueryTriggerInteraction.Ignore))
            return;
        if (!hit.collider.GetComponentInParent<Damageable>()) return;
        nextShot = Time.time + 1f / Mathf.Max(0.1f, fireRate);
        Projectile p = Instantiate(projectilePrefab, origin.position, Quaternion.LookRotation(Vector3.forward));
        p.Initialize(projectileSpeed, damage, transform.root);
    }

    public void Configure(Projectile projectile, float rate, int hitDamage)
    {
        projectilePrefab = projectile;
        fireRate = rate;
        damage = hitDamage;
    }
}
