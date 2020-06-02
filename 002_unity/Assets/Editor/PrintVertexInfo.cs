using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class PrintVertexInfo : Editor {

	[MenuItem("ThirdPerson/Print vertex info")]
	private static void printVertexInfo()
	{
		GameObject go = Selection.activeGameObject;
		if (go == null)
		{
			Debug.Log("Select an object first");
			return;
		}

		MeshFilter mesh = go.GetComponent<MeshFilter>();
		if (mesh == null)
		{
			Debug.Log("No mesh found.");
			return;
		}

		Debug.Log(mesh.name + ":" + mesh.sharedMesh.vertexCount + "  vertices found");

		for (int i = 0; i < mesh.sharedMesh.vertexCount; i++)
		{
			Debug.Log("Vertex "+i+":" + mesh.sharedMesh.vertices[i]);
		}

	}

	[MenuItem("ThirdPerson/Quad demo")]
	private static void quadDemo()
	{
		Vector3 a = new Vector3(1, 1, 1);
		Vector3 b = new Vector3(-1, 1, 1);

		Debug.Log (Quaternion.LookRotation(a, Vector3.up));
		Debug.Log (Quaternion.LookRotation(b, Vector3.up));

	}

}
