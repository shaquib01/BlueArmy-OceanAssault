using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField, Min(1)] int value = 1;
    [SerializeField] float spinSpeed = 180f;
    [SerializeField] float bobHeight = 0.12f;
    [SerializeField] float bobFrequency = 2.4f;
    bool collected;
    Vector3 basePosition;

    void Awake() { basePosition = transform.position; }

    void Update()
    {
        if (Time.timeScale <= 0f) return;
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime, Space.Self);
        Vector3 p = basePosition;
        p.y += Mathf.Sin(Time.time * bobFrequency * Mathf.PI * 2f) * bobHeight;
        transform.position = p;
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
