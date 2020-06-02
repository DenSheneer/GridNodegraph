using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeGraph))]
public class NodePlacer : Editor
{
    NodeGraph nodeGraph;
    Editmode currentEditmode = Editmode.PlaceObstacles;
    Vector3Int selectorTile = Vector3Int.zero;
    TileIndicator tileIndicator;
    NodeGraphGenerator nodeGraphGenerator;

    enum Editmode
    {
        PlaceObstacles = 0
    }

    void OnEnable()
    {
        nodeGraph = target as NodeGraph;
        tileIndicator = nodeGraph.tileIndicator;
        nodeGraphGenerator = nodeGraph.nodeGraphGenerator;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Remove all obstacles"))
        {
            removeAllObstacles();
        }
    }

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        if (guiEvent.Equals(Event.KeyboardEvent("space")))
        {
            nodeGraphGenerator.GenerateNodes();
        }

        updateSelector(guiEvent);
        placeObstacle(guiEvent);

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
            Transform objectHit = hit.transform;
            tileIndicator.UpdatePosition(hit.point);
            selectorTile = tileIndicator.GetSelectedTile();
        }
    }

    void placeObstacle(Event guiEvent)
    {
        if (leftMouseDown(guiEvent))
        {
            GameObject obstacle = Instantiate(nodeGraph.obstaclePrefab);
            obstacle.tag = "obstacle";
            obstacle.transform.position = selectorTile + NodeGraph.Offset;
            obstacle.transform.localScale = new Vector3(1, 1, 1);
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
        nodeGraph.Clear();
    }

    bool leftMouseDown(Event guiEvent)
    {
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
