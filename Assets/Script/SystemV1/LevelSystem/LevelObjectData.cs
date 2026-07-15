using UnityEngine;

public enum LevelObjectType 
{
    Empty,
    Source,
    Receiver,
    Straight,
    Corner,
    ThreeWay,
    FourWay,
    Portal,
    Converter
}
[System.Serializable]
public class LevelObjectData
{
    public LevelObjectType objectType;
    public Vector2Int position;
    public int rotation;
    public int portalID;
    public SignalColor inputColor;
    public SignalColor outputColor;
}
