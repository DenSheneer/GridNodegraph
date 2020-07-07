using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraphGenerator : MonoBehaviour
{
    public Vector2 leftBottom = new Vector2(-5,-5);
    public Vector2 topRight = new Vector2(5,5);
    public float gridSize = 1;

    [SerializeField]
    private LayerMask layerMask;

    private Vector2?[,] nodes;

    void Start()
    {
        GameObject floor = GameObject.Find("floor");
        floor.layer = LayerMask.NameToLayer("Ignore Raycast");

        int nodeColumns = (int)(Mathf.Floor((topRight.x - leftBottom.x) / gridSize));
        int nodeRows = (int)(Mathf.Floor((topRight.y - leftBottom.y) / gridSize));

        nodes = new Vector2?[nodeColumns, nodeRows];

        for (int x = 0; x < nodeColumns; x++)
        {
            for (int y = 0; y < nodeRows; y++)
            {
                Vector2 node = leftBottom + Vector2.one * gridSize * 0.5f + new Vector2(x, y) * gridSize;
                Vector3 worldSpace = new Vector3(node.x, 100, node.y);

                Ray ray = new Ray(worldSpace, Vector3.down);
                //if we hit something, there is something in the way
                if (!Physics.Raycast(ray, Mathf.Infinity, layerMask))
                {
                    nodes[x, y] = node;
                }else
                {
                    nodes[x, y] = new Vector2(-1,-1);
                }
            }
        }
    }

    public Vector2?[,] GetNodes() {
        return nodes;
    }


    private void OnDrawGizmos()
    {
        if (nodes == null) return;

        int nodeColumns = nodes.GetLength(0);
        int nodeRows = nodes.GetLength(1);

        for (int x = 0; x < nodeColumns; x++)
        {
            for (int y = 0; y < nodeRows; y++)
            {
                if (nodes[x, y] == null) continue;

                Vector2 node = (Vector2)nodes[x, y];
                Vector3 worldSpace = new Vector3(node.x, 1, node.y);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(worldSpace, 0.1f);

                //if there is a right node draw a line
                if (x < nodeColumns - 1 && nodes[x + 1, y] != null)
                {
                    Vector2 rightNode = (Vector2)nodes[x+1, y];
                    Vector3 rightWorldSpace = new Vector3(rightNode.x, 1, rightNode.y);
                    Gizmos.DrawLine(worldSpace, rightWorldSpace);

                }

                if (y < nodeRows - 1 && nodes[x, y+1] != null)
                {
                    Vector2 downNode = (Vector2)nodes[x, y+1];
                    Vector3 downWorldSpace = new Vector3(downNode.x, 1, downNode.y);
                    Gizmos.DrawLine(worldSpace, downWorldSpace);
                }
            }
        }
    }
}
