using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Level", menuName ="Lost Signal/ Level Data")]
public class LevelData : ScriptableObject
{
    public int width = 6;
    public int hight = 6;

    public List<LevelObjectData> objects = new();
}
