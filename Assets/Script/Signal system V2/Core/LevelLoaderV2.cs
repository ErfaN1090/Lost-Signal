using UnityEngine;
using System.Collections.Generic;

public class LevelLoaderV2 : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private PrefabDataBase prefabDatabase;

    public List<NodeV2> LoadLevel()
    {
        List<NodeV2> spawnedNodes = new();

        foreach (LevelObjectData data in levelData.objects)
        {
            GameObject prefab = GetPrefab(data.objectType);

            if (prefab == null)
                continue;

            GameObject obj = Instantiate(
                prefab,
                new Vector3(data.position.x, data.position.y, 0),
                Quaternion.Euler(0, 0, -90 * data.rotation)
            );

            NodeV2 node = obj.GetComponent<NodeV2>();

            node.GridPosition = data.position;

            node.SourceColor = data.outputColor;
            node.ReceiverColor = data.inputColor;
            node.ConvertedColor = data.outputColor;
            node.PortalID = data.portalID;

            for (int i = 0; i < data.rotation; i++)
                RotateConnections(node);

            spawnedNodes.Add(node);
            //Debug.Log($"{node.name} Grid={node.GridPosition} World={node.transform.position}");
        }

        return spawnedNodes;
    }

    private void RotateConnections(NodeV2 node)
    {
        for (int i = 0; i < node.Connections.Count; i++)
        {
            switch (node.Connections[i])
            {
                case Direction.Up:
                    node.Connections[i] = Direction.Right;
                    break;

                case Direction.Right:
                    node.Connections[i] = Direction.Down;
                    break;

                case Direction.Down:
                    node.Connections[i] = Direction.Left;
                    break;

                case Direction.Left:
                    node.Connections[i] = Direction.Up;
                    break;
            }
        }
    }
    private GameObject GetPrefab(LevelObjectType type)
    {
        switch (type)
        {
            case LevelObjectType.Source: return prefabDatabase.sourcePrefab;
            case LevelObjectType.Receiver: return prefabDatabase.receiverPrefab;
            case LevelObjectType.Straight: return prefabDatabase.StraightPrefab;
            case LevelObjectType.Corner: return prefabDatabase.cornerPrefab;
            case LevelObjectType.ThreeWay: return prefabDatabase.threeWayPrefab;
            case LevelObjectType.FourWay: return prefabDatabase.fourWayPrefab;
            case LevelObjectType.Portal: return prefabDatabase.portalPrefab;
            case LevelObjectType.Converter: return prefabDatabase.converterPrefab;
        }

        return null;
    }

}
