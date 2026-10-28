using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float forwardSpeed = 7f;
    [SerializeField] float horizontalSensitivity = 0.012f;
    [SerializeField] float horizontalLimit = 3.2f;
    [SerializeField] float smoothTime = 0.08f;
    [Header("Formation")]
    [SerializeField] GameObject soldierPrefab;
    [SerializeField] int startingSoldiers = 12;
    [SerializeField] float spacing = 0.85f;
    [SerializeField] int maxSoldiers = 250;

    readonly List<Transform> soldiers = new();
    float targetX, velocityX;
    bool dragging;
    bool initialized;
    Vector2 lastPointer;
    public int Count => soldiers.Count;
    public float ForwardSpeed => forwardSpeed;

    void Start()
    {
        initialized = true;
        SetCount(startingSoldiers);
    }

    void Update()
    {
        if (GameManager.Instance && Time.timeScale == 0f) return;
        ReadPointer();
        targetX = Mathf.Clamp(targetX, -horizontalLimit, horizontalLimit);
        Vector3 p = transform.position;
        p.x = Mathf.SmoothDamp(p.x, targetX, ref velocityX, smoothTime);
        p.z += forwardSpeed * Time.deltaTime;
        transform.position = p;
        ArrangeFormation();
    }

    void ReadPointer()
    {
        // Android can emulate mouse input for the same touch. Process touch OR mouse,
        // never both in one frame, to avoid doubled steering on some devices.
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                dragging = true;
                lastPointer = t.position;
            }
            else if (t.phase == TouchPhase.Moved && dragging)
            {
                targetX += (t.position.x - lastPointer.x) * horizontalSensitivity;
                lastPointer = t.position;
            }
            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                dragging = false;
            }
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            lastPointer = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) dragging = false;
        if (Input.GetMouseButton(0) && dragging)
        {
            Vector2 now = Input.mousePosition;
            targetX += (now.x - lastPointer.x) * horizontalSensitivity;
            lastPointer = now;
        }
    }

    public void AddSoldiers(int amount) => SetCount(Mathf.Min(maxSoldiers, Count + Mathf.Max(0, amount)));
    public void MultiplySoldiers(int factor)
    {
        if (factor > 0) SetCount((int)Mathf.Min(maxSoldiers, (long)Count * factor));
    }
    public void RemoveSoldiers(int amount) => SetCount(Mathf.Max(0, Count - Mathf.Max(0, amount)));

    public void SetCount(int count)
    {
        if (!soldierPrefab) { Debug.LogWarning("CrowdController needs a soldier prefab.", this); return; }
        count = Mathf.Clamp(count, 0, maxSoldiers);
        while (soldiers.Count < count)
            soldiers.Add(Instantiate(soldierPrefab, transform).transform);
        while (soldiers.Count > count)
        {
            int i = soldiers.Count - 1;
            if (soldiers[i]) Destroy(soldiers[i].gameObject);
            soldiers.RemoveAt(i);
        }
        ArrangeFormation();
        if (GameManager.Instance)
        {
            GameManager.Instance.SetCrowdCount(Count);
            if (initialized && Count == 0)
                GameManager.Instance.EndRun(false);
        }
    }

    void ArrangeFormation()
    {
        int columns = Mathf.Max(1, Mathf.CeilToInt(Mathf.Sqrt(Mathf.Max(1, soldiers.Count))));
        for (int i = 0; i < soldiers.Count; i++)
        {
            if (!soldiers[i]) continue;
            int row = i / columns, col = i % columns;
            Vector3 local = new Vector3((col - (columns - 1) * 0.5f) * spacing, 0.25f, -row * spacing);
            soldiers[i].localPosition = Vector3.Lerp(soldiers[i].localPosition, local, Time.deltaTime * 10f);
            soldiers[i].localRotation = Quaternion.identity;
        }
    }
}
