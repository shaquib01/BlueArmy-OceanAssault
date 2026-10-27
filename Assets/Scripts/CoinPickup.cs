using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int value = 1;
    bool collected;
    void OnTriggerEnter(Collider other)
    {
        if (collected && !other.GetComponentInParent<CrowdController>()) return;
        if (!other.GetComponentInParent<CrowdController>()) return;
        collected = true;
        // Reward via a defeated target is the core loop for now; pickup reward can be wired here later.
        Destroy(gameObject);
    }
}
