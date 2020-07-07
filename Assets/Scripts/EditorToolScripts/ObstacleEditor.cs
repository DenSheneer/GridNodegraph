using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeGraph))]
public class ObstacleEditor : Editor
{
    Editmode currentEditmode = Editmode.PlaceObstacles;

    Vector3Int selectorTile = Vector3Int.zero;

    NodeGraph nodeGraph;
    TileIndicator tileIndicator;

    bool placementAllowed = false;

    enum Editmode
    {
        DeleteObstacles = 0,
        PlaceObstacles = 1
    }

    void OnEnable()
    {
        nodeGraph = target as NodeGraph;
        tileIndicator = FindObjectOfType<TileIndicator>();
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Remove all obstacles"))
            removeAllObstacles();
    }

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        updateSelector(guiEvent);
        updateEditmode(guiEvent);

        if (leftMouseGUI(guiEvent))
        {
            switch (currentEditmode)
            {
                case Editmode.PlaceObstacles:
                    placeObstacle(guiEvent);
                    break;

                case Editmode.DeleteObstacles:
                    removeObstacle(guiEvent);
                    break;
            }
        }

        if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }

    void updateSelector(Event guiEvent)
    {
        RaycastHit hit;
        Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag != "obstacle")
                placementAllowed = true;

            tileIndicator.UpdatePosition(hit.point);
            selectorTile = tileIndicator.GetSelectedTile();
            return;
        }
        placementAllowed = false;
    }
    void placeObstacle(Event guiEvent)
    {
        if (placementAllowed)
        {
            GameObject obstacleGO = Resources.Load<GameObject>("Obstacle");
            Instantiate(obstacleGO, selectorTile + NodeGraph.Offset, Quaternion.identity);
            placementAllowed = false;
        }
    }
    void removeObstacle(Event guiEvent)
    {
        if (leftMouseGUI(guiEvent))
        {
            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "obstacle")
                    DestroyImmediate(hit.collider.gameObject);
            }
        }
    }
    void removeAllObstacles()
    {
        currentEditmode = Editmode.PlaceObstacles;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("obstacle");
        Debug.Log("destroying " + gos.Length + " obstacles...");
        foreach (GameObject go in gos)
        {
            DestroyImmediate(go);
        }
    }
    void updateEditmode(Event guiEvent)
    {
        if (guiEvent.shift)
            currentEditmode = Editmode.DeleteObstacles;
        else
            currentEditmode = Editmode.PlaceObstacles;
    }
    bool leftMouseGUI(Event guiEvent)
    {
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 ||
            guiEvent.type == EventType.MouseDrag && guiEvent.button == 0)
            return true;

        return false;
    }
}
