using System;
using UnityEngine;

public class DetectingArea : MonoBehaviour
{
    private Transform currentTarget;
    public event Action<Transform> TargetAggro;
    public string Tag;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tag))
        {
            currentTarget = other.transform;
            TargetAggro?.Invoke(currentTarget);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tag) && currentTarget == other.transform)
        {
            currentTarget = null;
            TargetAggro?.Invoke(currentTarget);
        }
    }
}
