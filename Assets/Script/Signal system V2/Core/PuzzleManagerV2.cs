using UnityEngine;
using System.Collections.Generic;

public class PuzzleManagerV2 : MonoBehaviour
{
    [SerializeField] private LevelLoaderV2 levelLoader;
    [SerializeField] private SignalSystemV2 signalSystem;
    [SerializeField] private GrideManager grideManager;
    [SerializeField] private LevelData levelData;

    private List<NodeV2> nodes;

    private void Start()
    {
        grideManager.initialize(levelData.width, levelData.hight);

        nodes = levelLoader.LoadLevel();

        signalSystem.Initialize(nodes);

        signalSystem.Recalculate();

        //Debug.Log("V2 Started");
    }

    public void RotateFinished()
    {
        signalSystem.Recalculate();
    }
}
