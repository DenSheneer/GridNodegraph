using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class NodeToText
{
    static string path = "Assets/Resources/nodes.json";

    public static void NodeToTextFile(Vector2 node)
    {
        if (node.x >= 0 && node.y >= 0)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            StreamWriter writer = new StreamWriter(path, true);

            writer.WriteLine("Node [");
            writer.WriteLine("x: " + (node.x - 0.5f));
            writer.WriteLine("y: " + (node.y - 0.5f));
            writer.WriteLine("];");
            writer.WriteLine("");
            writer.Close();

            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("nodes");
        }
        else
            return;
    }
}
