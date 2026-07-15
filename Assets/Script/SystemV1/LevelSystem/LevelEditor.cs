using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    public LevelObjectType selectedTool = LevelObjectType.Straight;

}
