using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;


public class MobSpawner : MonoBehaviour
{
    public Color gizmoColor = new Color(0, 0, 1, 0.3f); // 연두색, 반투명
    public int sizeX = 4; // X(가로) 방향 길이
    public int sizeZ = 4; // Z(세로) 방향 길이


    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        // 내부 반투명 채우기
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.1f);
        Gizmos.DrawWireCube(transform.position, new Vector3(sizeX, 0.01f, sizeZ));
    }
}
