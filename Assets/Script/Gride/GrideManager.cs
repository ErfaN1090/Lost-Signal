using UnityEngine;

public class GrideManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public Transform tileParent;

    public int width;
    public int height;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.transform.SetParent(tileParent);
            }
        }
        Camera.main.transform.position = new Vector3((width - 1) / 2f, (height - 1) / 2f, -10);
    }
}
