using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public enum EditorTool 
{
    Place,
    select,
    Erase
}

public class LevelEditorWindow : EditorWindow
{
    [MenuItem("Tools/Lost Signal/Level Editor")]

    public static void open()
    {
        GetWindow<LevelEditorWindow>("Level Editor");
    }
    [SerializeField] private PrefabDataBase prefabDataBase;
    private LevelData currentLevel;
    private LevelObjectType selectedTool;
    private PreviewObject Selectedpreview;
    private LevelObjectData selectedData;
    private void OnGUI()
    {
        GUILayout.Label("Lost Signal Level Editor", EditorStyles.boldLabel);

        currentTool = (EditorTool)GUILayout.Toolbar((int)currentTool, new string[] { "Place", "Select", "Erase" });

        currentLevel = (LevelData)EditorGUILayout.ObjectField("Level", currentLevel, typeof(LevelData), false);

        selectedTool = (LevelObjectType)EditorGUILayout.EnumPopup("Tool", selectedTool);

        prefabDataBase = (PrefabDataBase)EditorGUILayout.ObjectField("Prefab DataBase", prefabDataBase, typeof(PrefabDataBase), false);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Preview", GUILayout.Height(30)))
        {
            RefreshPreview();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear Preview", GUILayout.Height(30)))
        {
            clear();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(15);
        if (selectedData != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Selected Object", EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Type", selectedData.objectType.ToString());

                EditorGUI.BeginChangeCheck();

                selectedData.rotation =
                    EditorGUILayout.IntSlider("Rotation", selectedData.rotation, 0, 3);

                switch (selectedData.objectType)
                {
                    case LevelObjectType.Source:

                        selectedData.outputColor =
                            (SignalColor)EditorGUILayout.EnumPopup(
                                "Output Color",
                                selectedData.outputColor);

                        break;

                    case LevelObjectType.Receiver:

                        selectedData.inputColor =
                            (SignalColor)EditorGUILayout.EnumPopup(
                                "Input Color",
                                selectedData.inputColor);

                        break;

                    case LevelObjectType.Converter:

                        selectedData.outputColor =
                            (SignalColor)EditorGUILayout.EnumPopup(
                                "Converted Color",
                                selectedData.outputColor);

                        break;

                    case LevelObjectType.Portal:

                        selectedData.portalID =
                            EditorGUILayout.IntField(
                                "Portal ID",
                                selectedData.portalID);

                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(currentLevel);
                    AssetDatabase.SaveAssets();

                    RefreshPreview();
                }
            }
       
    }
    private GameObject PreviewParent;
    private EditorTool currentTool = EditorTool.Place;
    private GameObject GetPrefab(LevelObjectType type)
    {
        switch (type)
        {
            case LevelObjectType.Empty:
                return null;
            case LevelObjectType.Source:
                return prefabDataBase.sourcePrefab;
            case LevelObjectType.Receiver:
                return prefabDataBase.receiverPrefab;
            case LevelObjectType.Straight:
                return prefabDataBase.StraightPrefab;
            case LevelObjectType.Corner:
                return prefabDataBase.cornerPrefab;
            case LevelObjectType.ThreeWay:
                return prefabDataBase.threeWayPrefab;
            case LevelObjectType.FourWay:
                return prefabDataBase.fourWayPrefab;
            case LevelObjectType.Portal:
                return prefabDataBase.portalPrefab;
            case LevelObjectType.Converter:
                return prefabDataBase.converterPrefab;
            default: return null;
                
        }
    }
    private Sprite GetSprite(LevelObjectType type)
    {
        GameObject prefab = GetPrefab(type);

        if (prefab == null)
        {
            return null;
        }

        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();

        if (sr == null)
        {
            return null;
        }

        return sr.sprite;
    }

    //private void DrawObjects()
    //{
    //    if (currentLevel == null)
    //        return;

    //    foreach (LevelObjectData obj in currentLevel.objects)
    //    {
    //        Sprite sprite = GetSprite(obj.objectType);

    //        if (sprite == null)
    //            continue;

    //        Handles.BeginGUI();

    //        Vector3 worldPos = new Vector3(obj.position.x + 0.5f, obj.position.y + 0.5f);

    //        Vector2 guiPos = HandleUtility.WorldToGUIPoint(worldPos);

    //        Rect rect = new Rect(guiPos.x - 16, guiPos.y - 16, 32, 32);

    //        GUI.DrawTexture(rect, sprite.texture);
    //        Handles.EndGUI();
    //    }
    //}
    private void DrawGrid()
    {
        if (currentLevel == null)
            return;
        Handles.color = Color.red;

        for (int x = 0; x <= currentLevel.width; x++)
        {
            Handles.DrawLine(new Vector3(x, 0, 0), new Vector3(x, currentLevel.hight, 0));
        }
        for (int y = 0; y <= currentLevel.hight; y++)
        {
            Handles.DrawLine(new Vector3(0, y, 0), new Vector3(currentLevel.width, y, 0));
        }
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        DrawGrid();
        //DrawObjects();
        Event e = Event.current;

        if (currentTool == EditorTool.select && e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                PreviewObject preview = hit.collider.GetComponent<PreviewObject>();

                if (preview != null)
                {
                    Selectedpreview = preview;
                    selectedData = preview.data;

                    Debug.Log("Selected: " + selectedData.objectType);
                    Repaint();
                    e.Use();
                }
            }

        }
        if (currentTool == EditorTool.Place && e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            Plane plane = new Plane(Vector3.forward, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);

                int x = Mathf.FloorToInt(worldPos.x);
                int y = Mathf.FloorToInt(worldPos.y);

                if (currentLevel == null)
                {
                    return;
                }
                LevelObjectData existing = currentLevel.objects.Find(obj => obj.position == new Vector2Int(x, y));

                if (existing != null)
                {
                    currentLevel.objects.Remove(existing);
                }
                LevelObjectData data = new LevelObjectData();

                data.objectType = selectedTool;
                data.position = new Vector2Int(x, y);

                currentLevel.objects.Add(data);

                EditorUtility.SetDirty(currentLevel);
                AssetDatabase.SaveAssets();

                RefreshPreview();

                Debug.Log($"Grid: {x}, {y}");
            }

            if (Physics2D.GetRayIntersection(ray, Mathf.Infinity))
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

                Tale tile = hit.collider.GetComponent<Tale>();

                if(tile != null)
                {
                    Debug.Log(tile.gridPos);
                }
            }
        }
        if (currentTool == EditorTool.Erase && e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                PreviewObject preview = hit.collider.GetComponent<PreviewObject>();

                if (preview != null)
                {
                    currentLevel.objects.Remove(preview.data);
                    RefreshPreview();
                    EditorUtility.SetDirty(currentLevel);
                    AssetDatabase.SaveAssets();
                    e.Use();
                }
            }
        }
    }
    private void OnEnable()
    {
        CreatePreviewParent();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        clear();
    }

    private void clear()
    {
        if (PreviewParent != null)
        {
            Object.DestroyImmediate(PreviewParent);
        }
    }
    private void CreatePreviewParent()
    {
        if (PreviewParent != null)
            return;

        PreviewParent = GameObject.Find("Level Editor Preview");
        if (PreviewParent == null)
        {
            PreviewParent = new GameObject("Level Editor Preview");
        }

        PreviewParent.hideFlags = HideFlags.DontSave;
    }

    private void ClearPreview()
    {
        if (PreviewParent == null)
            return;

        while (PreviewParent.transform.childCount > 0)
        {
            DestroyImmediate(PreviewParent.transform.GetChild(0).gameObject);
        }
    }

    private void RefreshPreview()
    {
        Debug.Log("Refresh Started");
        CreatePreviewParent();
        ClearPreview();

        foreach (LevelObjectData data in currentLevel.objects)
        {
            Debug.Log(data);
            GameObject prefab = GetPrefab(data.objectType);
            if (prefab == null)
                continue;
            Debug.Log(prefab);
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            Debug.Log(obj);
            PreviewObject preview = obj.GetComponent<PreviewObject>();
            if (preview == null)
            {
                preview = obj.AddComponent<PreviewObject>();
            }
            preview.data = data;
            obj.transform.SetParent(PreviewParent.transform);
            Node node = obj.GetComponent<Node>();
            if (node != null)
            {
                for (int i = 0; i < data.rotation; i++)
                {
                    node.RotateDirection();
                }
            }
            obj.transform.rotation = Quaternion.Euler(0, 0, -90 * data.rotation);

            obj.transform.position = new Vector3(data.position.x + 0.5f, data.position.y + 0.5f, 0);
            obj.hideFlags = HideFlags.DontSave;
        }

        SceneView.RepaintAll();
    }
}
