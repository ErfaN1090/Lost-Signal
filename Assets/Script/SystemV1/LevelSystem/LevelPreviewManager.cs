using UnityEngine;
using UnityEditor;
public class LevelPreviewManager
{
    private GameObject PreviewParent;

    public void Initialize()
    {
        PreviewParent = GameObject.Find("Level Editor Preview");

        if(PreviewParent == null)
        {
            PreviewParent = new GameObject("Level Editor Preview");
            PreviewParent.hideFlags = HideFlags.DontSave;
        }
    }
}
