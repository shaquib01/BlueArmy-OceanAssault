using UnityEngine;
public class PrototypeCameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 8, -11);
    void LateUpdate()
    {
        if (!target) return;
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(24, 0, 0);
    }
}
