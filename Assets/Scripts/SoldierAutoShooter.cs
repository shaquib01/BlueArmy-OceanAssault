using System;
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
    RaycastHit[] hitBuffer = new RaycastHit[32];

    void Update()
    {
        if (!projectilePrefab || Time.timeScale == 0f || Time.time < nextShot) return;

        Transform origin = muzzle ? muzzle : transform;
        int hitCount = Physics.RaycastNonAlloc(
            origin.position, Vector3.forward, hitBuffer, targetRange,
            targetLayers, QueryTriggerInteraction.Ignore);

        Damageable target = null;
        float nearestDistance = float.MaxValue;
        Transform ownRoot = transform.root;
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit hit = hitBuffer[i];
            if (!hit.collider || hit.collider.transform.root == ownRoot) continue;
            Damageable candidate = hit.collider.GetComponentInParent<Damageable>();
            if (!candidate || hit.distance >= nearestDistance) continue;
            target = candidate;
            nearestDistance = hit.distance;
        }

        Array.Clear(hitBuffer, 0, hitCount);
        if (!target) return;

        nextShot = Time.time + 1f / Mathf.Max(0.1f, fireRate);
        Projectile p = Instantiate(projectilePrefab, origin.position, Quaternion.LookRotation(Vector3.forward));
        p.Initialize(projectileSpeed, damage, ownRoot);
    }

    public void Configure(Projectile projectile, float rate, int hitDamage)
    {
        projectilePrefab = projectile;
        fireRate = rate;
        damage = hitDamage;
    }
}
