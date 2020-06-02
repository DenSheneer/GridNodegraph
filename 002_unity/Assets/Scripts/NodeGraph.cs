using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{
    Dictionary<Vector3Int, Node> nodes =
        new Dictionary<Vector3Int, Node>();

    public static readonly Vector3 Offset = new Vector3(.5f, 0, .5f);

    public TileIndicator tileIndicator;
    public GameObject obstaclePrefab;
    public NodeGraphGenerator nodeGraphGenerator;


    public Vector2?[,] GetNodes()
    {
        Vector2?[,] nodeArray = nodeGraphGenerator.GetNodes();
        return nodeArray;
    }

    public void PrintToFile()
    {
        Debug.Log(nodeGraphGenerator.GetNodes().Length);
        foreach (Vector2 node in GetNodes())
        {
            NodeToText.NodeToTextFile(node);
        }
    }

    public Vector2Int GetWidthHeigth()
    {
        return nodeGraphGenerator.GetWidthHeigth();
    }


    public void RemoveNode(Node node)
    {
        if (nodes.ContainsValue(node))
        {
            FindNodeByNumber(node.NorthNeighbour).SouthNeighbour = -1;
            FindNodeByNumber(node.EastNeighbour).WestNeighbour = -1;
            FindNodeByNumber(node.SouthNeighbour).NorthNeighbour = -1;
            FindNodeByNumber(node.WestNeighbour).EastNeighbour = -1;

            nodes.Remove(node.Position);
        }
    }

    public void Clear()
    {
        nodeGraphGenerator.Clear();
    }

    public Node FindNodeByNumber(int nodeNr)
    {
        Node indexedNode = null;

        foreach (Node node in nodes.Values)
        {
            if (node.Nr == nodeNr)
            {
                indexedNode = node;
            }
        }
        return indexedNode;
    }

    public Node FindNodeOnTile(Vector3Int tilePosition)
    {
        Node node = null;
        if (nodes.ContainsKey(tilePosition))
        {
            node = nodes[tilePosition];
            return node;
        }
        return null;
    }

    public int NrOfNodes
    {
        get { return nodes.Count; }
    }

    public void ConnectNorthSouth(Node northNode, Node southNode)
    {
        northNode.SouthNeighbour = southNode.Nr;
        southNode.NorthNeighbour = northNode.Nr;
    }
    public void ConnectEastWest(Node eastNode, Node westNode)
    {
        eastNode.WestNeighbour = westNode.Nr;
        westNode.EastNeighbour = eastNode.Nr;
    }
}
