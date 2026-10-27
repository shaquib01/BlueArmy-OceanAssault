using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField, Min(1)] int value = 1;
    bool collected;

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.GetComponentInParent<CrowdController>()) return;
        if (!GameManager.Instance || GameManager.Instance.HasEnded) return;

        collected = true;
        GameManager.Instance.AddCoins(value);
        Destroy(gameObject);
    }
}
