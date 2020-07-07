using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class NodeGraph : MonoBehaviour
{
    public static readonly Vector3 Offset = new Vector3(.5f, 0, .5f);
    private static string path;

    public TileIndicator tileIndicator;

    [SerializeField]
    NodeGraphGenerator nodeGraphGenerator;

    public Vector2?[,] GetNodes()
    {
        Vector2?[,] nodeArray = nodeGraphGenerator.GetNodes();
        return nodeArray;
    }
    private void Start()
    {
        path = Application.dataPath + "/Resources/nodes.json";
    }

    public void PrintToFile()
    {
        File.WriteAllText(path, string.Empty);

        foreach (Vector2 node in GetNodes())
        {
            NodeToText.NodeToTextFile(node);
        }
        Debug.Log("File 'nodes.json' created at: " + path);
    }
}
