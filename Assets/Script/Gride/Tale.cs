using UnityEngine;

public class Tale : MonoBehaviour
{
    public Vector2Int gridPos;
    private void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
    }
}
