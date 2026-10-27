using UnityEngine;

public class MultiplierGate : MonoBehaviour
{
    public enum Operation { Add, Multiply }
    [SerializeField] Operation operation = Operation.Multiply;
    [SerializeField] int value = 2;
    bool used;

    void OnTriggerEnter(Collider other)
    {
        if (used) return;
        CrowdController crowd = other.GetComponentInParent<CrowdController>();
        if (!crowd) return;
        used = true;
        if (operation == Operation.Add) crowd.AddSoldiers(value);
        else crowd.MultiplySoldiers(value);
        // Visual gate is left in the scene to show the chosen route.
    }
}
