using UnityEngine;

[CreateAssetMenu(fileName = "Prefab Database", menuName = "Lost Signal/ Prefab Database")]
public class PrefabDataBase : ScriptableObject
{
    public GameObject sourcePrefab;
    public GameObject receiverPrefab;
    public GameObject StraightPrefab;
    public GameObject cornerPrefab;
    public GameObject threeWayPrefab;
    public GameObject fourWayPrefab;
    public GameObject portalPrefab;
    public GameObject converterPrefab; 
}
