using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;

[ExecuteAlways]
public class MapTail : MonoBehaviour
{
    public int sizeX = 4; // X(가로) 방향 길이
    public int sizeZ = 4; // Z(세로) 방향 길이
    public Color gizmoColor = new Color(0, 1, 0, 0.3f); // 연두색, 반투명

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Vector3 origin = transform.position;
        Vector3 p0 = origin;
        Vector3 p1 = origin + new Vector3(sizeX, 0, 0);
        Vector3 p2 = origin + new Vector3(sizeX, 0, sizeZ);
        Vector3 p3 = origin + new Vector3(0, 0, sizeZ);

        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);

        // 내부 반투명 채우기
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.1f);
        Gizmos.DrawCube(origin + new Vector3(sizeX / 2f, 0, sizeZ / 2f), new Vector3(sizeX, 0.01f, sizeZ));
    }
}
