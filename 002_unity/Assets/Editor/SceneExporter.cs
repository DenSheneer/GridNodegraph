using System.Xml;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExporter : Editor
{

    [MenuItem("MGE/ExportScene")]
    private static void ExportScene()
    {
        XmlDocument sceneXml = new XmlDocument();
        XmlElement rootElement = sceneXml.CreateElement("root");
        sceneXml.AppendChild(rootElement);

        GameObject[] nodeGraphArray = GameObject.FindGameObjectsWithTag("NodeGraph");
        NodeGraph nodeGraph = nodeGraphArray[0].GetComponent<NodeGraph>();
        processNodeGraph(nodeGraph, sceneXml, rootElement);

        Scene scene = SceneManager.GetActiveScene();
        GameObject[] gameObjects = scene.GetRootGameObjects();
        foreach (GameObject go in gameObjects) _process(go.transform, rootElement, sceneXml);

        string path = Application.dataPath + "/" + scene.name + "_export.xml";
        sceneXml.Save(path);
        Debug.Log("File exported to :" + path);
    }

    private static void processNodeGraph(NodeGraph pNodeGraph, XmlDocument pScene, XmlElement root)
    {
        if (pNodeGraph != null)
        {
            Debug.Log("processing nodes...");
            XmlElement parent;

            parent = pScene.CreateElement("NodeGraph");
            root.AppendChild(parent);
            parent.SetAttribute("width", (pNodeGraph.GetWidthHeigth().x).ToString());
            parent.SetAttribute("heigth", (pNodeGraph.GetWidthHeigth().y).ToString());

            int nodeNr = 1;

            foreach (Vector2? node in pNodeGraph.GetNodes())
            {
                if (node != null)
                {
                    Vector2 actualNode = (Vector2)node;
                    XmlElement child;
                    child = pScene.CreateElement("Node");
                    parent.AppendChild(child);
                    child.SetAttribute("number", nodeNr.ToString()); ;
                    child.SetAttribute("x", (actualNode.x - 0.5f).ToString());
                    child.SetAttribute("y", (actualNode.y - 0.5f).ToString());
                    nodeNr++;
                }else
                {
                    Debug.Log("this node is null");
                }
            }
            Debug.Log("done");
        }else
        {
            Debug.Log("nodegraph is null");
        }
    }

    private static void _process(Transform pTransform, XmlElement pParentElement, XmlDocument pScene)
    {
        XmlElement convertedElement = _convert(pTransform, pScene);
        pParentElement.AppendChild(convertedElement);

        foreach (Transform child in pTransform)
        {
            _process(child, convertedElement, pScene);
        }
    }

    private static XmlElement _convert(Transform pTransform, XmlDocument pScene)
    {
        XmlElement node;

        if (pTransform.GetComponent<Light>() != null)
            node = pScene.CreateElement("Light");
        else if (pTransform.GetComponent<Camera>() != null)
            node = pScene.CreateElement("Camera");
        else
            node = pScene.CreateElement("GameObject");


        node.SetAttribute("name", pTransform.name);

        Vector3 position = pTransform.localPosition;
        position.x *= -1;
        node.SetAttribute("position", VectorToString(position));

        Quaternion rotation = pTransform.localRotation;
        rotation.y *= -1;
        rotation.z *= -1;
        node.SetAttribute("rotation", rotation.ToString());

        node.SetAttribute("scale", VectorToString(pTransform.localScale));

        MeshFilter mesh = pTransform.GetComponent<MeshFilter>();
        if (mesh != null)
        {
            node.SetAttribute("mesh", pTransform.name + ".obj");
        }

        MeshRenderer renderer = pTransform.GetComponent<MeshRenderer>();
        if (renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.mainTexture != null)
        {
            node.SetAttribute("material", renderer.sharedMaterial.mainTexture.name);
        }

        ObjectBehaviours behaviours = pTransform.GetComponent<ObjectBehaviours>();
        if (behaviours != null)
        {
            string[] list = behaviours.getBehaviours();
            string behaviourString = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (i == list.Length - 1)
                    behaviourString += list[i];
                else
                    behaviourString += (list[i] + "-");
            }
            node.SetAttribute("behaviours", behaviourString);
        }

        Light light = pTransform.GetComponent<Light>();
        if (light != null)
        {
            node.SetAttribute("type", light.type.ToString());
            node.SetAttribute("ambient", VectorToString(new Vector3(light.intensity, light.intensity, light.intensity)));
            node.SetAttribute("diffuse", VectorToString(new Vector3(light.color.r, light.color.g, light.color.b)));
            node.SetAttribute("cutOff", light.innerSpotAngle.ToString());
            node.SetAttribute("outerCutOff", light.spotAngle.ToString());
            node.SetAttribute("range", light.range.ToString());
        }

        Camera camera = pTransform.GetComponent<Camera>();
        if (camera != null)
        {
            node.SetAttribute("FOV", camera.fieldOfView.ToString());
        }

        return node;
    }


    private static string VectorToString(Vector3 vec)
    {
        string result = "(";

        result += vec.x.ToString() + ", ";
        result += vec.y.ToString() + ", ";
        result += vec.z.ToString();

        return result += ")";
    }

    // (0.0, 0.0, 0.0)
}
