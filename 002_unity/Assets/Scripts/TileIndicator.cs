using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TileIndicator : MonoBehaviour
{
    Vector3Int oldPosition = new Vector3Int();
    Vector3Int position;

    public void UpdatePosition(Vector3 raycastPoint)
    {
        position = CordsToTile(raycastPoint);

        if (position != oldPosition)
        {
            oldPosition = position;
            transform.position = position + NodeGraph.Offset;
            //Debug.Log("X: " + position.x + " Y: " + position.z);
        }
    }

    public static Vector3Int CordsToTile(Vector3 position)
    {
        return new Vector3Int((int)position.x, 0, (int)position.z);
    }

    public Vector3Int GetSelectedTile()
    {
        return new Vector3Int(position.x, 0, position.z);
    }
}
