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

    enum Editmode
    {
        SelectTile = 5,
        selectNorthConnection = 1,
        selectEastConnection = 2,
        selectSouthConnection = 3,
        selectWestConnection = 4,
        PlaceObstacles = 0
    }

    void OnEnable()
    {
        nodeGraph = target as NodeGraph;
        tileIndicator = nodeGraph.tileIndicator;
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Remove all nodes"))
        {
            removeAllNodes();
        }
        if (GUILayout.Button("Remove all obstacles"))
        {
            removeAllObstacles();
        }
    }

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        updateSelector(guiEvent);
        skipModeCheck(guiEvent);
        toggleObstacleMode(guiEvent);
        tileIndicator.UpdateConnectionIndicator((int)currentEditmode);

        switch (currentEditmode)
        {
            case Editmode.SelectTile:
                placeNode(guiEvent);
                break;

            case Editmode.selectNorthConnection:
                linkNorthSide(guiEvent);
                break;

            case Editmode.selectEastConnection:
                linkEastSide(guiEvent);
                break;

            case Editmode.selectSouthConnection:
                linkSouthSide(guiEvent);
                break;

            case Editmode.selectWestConnection:
                linkWestSide(guiEvent);
                break;

            case Editmode.PlaceObstacles:
                placeObstacle(guiEvent);
                break;
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
            Transform objectHit = hit.transform;
            tileIndicator.UpdatePosition(hit.point);
            selectorTile = tileIndicator.GetSelectedTile();
        }
    }

    void placeNode(Event guiEvent)
    {
        if (leftMouseDown(guiEvent))
        {
            if (nodeGraph.FindNodeOnTile(selectorTile) == null)
            {
                Node node = new Node(nodeGraph.NrOfNodes, selectorTile, -1, -1, -1, -1);
                nodeGraph.AddNode(node);

                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Plane);
                tile.tag = "node";
                tile.transform.position = selectorTile + NodeGraph.Offset;

                currentEditmode = Editmode.selectNorthConnection;
            }
            else
            {
                Debug.Log("there is already a node on this tile");
            }
        }
    }

    void linkNorthSide(Event guiEvent)
    {
        Vector3Int tile = Vector3Int.zero;
        tile = selectorTile + new Vector3Int(0, 0, 10);

        if (nodeGraph.FindNodeOnTile(tile) != null)
        {
            if (leftMouseDown(guiEvent))
            {
                Debug.Log("north linked");
                nodeGraph.FindNodeOnTile(selectorTile).NorthNeighbour = nodeGraph.FindNodeOnTile(tile).Nr;
                nodeGraph.FindNodeOnTile(tile).SouthNeighbour = nodeGraph.FindNodeOnTile(selectorTile).Nr;
                currentEditmode++;
            }
        }
        else
        {
            currentEditmode++;
        }
    }
    void linkEastSide(Event guiEvent)
    {
        Vector3Int tile = Vector3Int.zero;
        tile = selectorTile + new Vector3Int(10, 0, 0);

        if (nodeGraph.FindNodeOnTile(tile) != null)
        {
            if (leftMouseDown(guiEvent))
            {
                Debug.Log("east linked");
                nodeGraph.FindNodeOnTile(selectorTile).EastNeighbour = nodeGraph.FindNodeOnTile(tile).Nr;
                nodeGraph.FindNodeOnTile(tile).WestNeighbour = nodeGraph.FindNodeOnTile(selectorTile).Nr;
                currentEditmode++;
            }
        }
        else
        {
            currentEditmode++;
        }
    }
    void linkSouthSide(Event guiEvent)
    {
        Vector3Int tile = Vector3Int.zero;
        tile = selectorTile + new Vector3Int(0, 0, -10);

        if (nodeGraph.FindNodeOnTile(tile) != null)
        {
            if (leftMouseDown(guiEvent))
            {
                Debug.Log("south linked");
                nodeGraph.FindNodeOnTile(selectorTile).SouthNeighbour = nodeGraph.FindNodeOnTile(tile).Nr;
                nodeGraph.FindNodeOnTile(tile).NorthNeighbour = nodeGraph.FindNodeOnTile(selectorTile).Nr;
                currentEditmode++;
            }
        }
        else
        {
            currentEditmode++;
        }
    }
    void linkWestSide(Event guiEvent)
    {
        Vector3Int tile = Vector3Int.zero;
        tile = selectorTile + new Vector3Int(-10, 0, 0);

        if (nodeGraph.FindNodeOnTile(tile) != null)
        {
            if (leftMouseDown(guiEvent))
            {
                Debug.Log("west linked");
                nodeGraph.FindNodeOnTile(selectorTile).WestNeighbour = nodeGraph.FindNodeOnTile(tile).Nr;
                nodeGraph.FindNodeOnTile(tile).EastNeighbour = nodeGraph.FindNodeOnTile(selectorTile).Nr;
                currentEditmode = 0;
            }
        }
        else
        {
            currentEditmode++;
        }
    }
    void placeObstacle(Event guiEvent)
    {
        if (leftMouseDown(guiEvent))
        {
            GameObject obstacle = Instantiate(nodeGraph.obstaclePrefab);
            obstacle.tag = "Wall";
            obstacle.transform.position = selectorTile + NodeGraph.Offset;
            obstacle.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void removeAllNodes()
    {
        nodeGraph.Clear();
        currentEditmode = Editmode.PlaceObstacles;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("node");
        Debug.Log("destroying " + gos.Length + " nodes...");
        foreach (GameObject go in gos)
        {
            DestroyImmediate(go);
        }
    }
    void removeAllObstacles()
    {
        currentEditmode = Editmode.PlaceObstacles;
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Wall");
        Debug.Log("destroying " + gos.Length + " obstacles...");
        foreach (GameObject go in gos)
        {
            DestroyImmediate(go);
        }
    }

    void skipModeCheck(Event guiEvent)
    {
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
            if (currentEditmode != 0)
            {
                if (currentEditmode != Editmode.selectWestConnection)
                {
                    currentEditmode++;
                }
                else
                {
                    currentEditmode = Editmode.SelectTile;
                }
                Debug.Log("skipped");
            }
    }
    void toggleObstacleMode(Event guiEvent)
    {
        if (guiEvent.Equals(Event.KeyboardEvent("space")))
        {
            if (currentEditmode == Editmode.SelectTile)
            {
                currentEditmode = Editmode.PlaceObstacles;
            }
            else
            {
                if (currentEditmode == Editmode.PlaceObstacles)
                {
                    currentEditmode = Editmode.SelectTile;
                }
            }
        }

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
