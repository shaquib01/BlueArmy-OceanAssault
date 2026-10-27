using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField, Min(1)] int value = 1;
    [SerializeField] float spinSpeed = 180f;
    bool collected;

    void Update()
    {
        if (Time.timeScale > 0f)
            transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected || !other.GetComponentInParent<CrowdController>()) return;
        if (!GameManager.Instance || GameManager.Instance.HasEnded) return;

        collected = true;
        GameManager.Instance.AddCoins(value);
        Destroy(gameObject);
    }
}
