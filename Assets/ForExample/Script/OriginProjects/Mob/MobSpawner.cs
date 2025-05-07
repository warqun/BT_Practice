using System.Collections.Generic;
using Unity.Hierarchy;
using UnityEngine;


public class MobSpawner : MonoBehaviour
{
    public Color gizmoColor = new Color(0, 0, 1, 0.3f); // ���λ�, ������
    public int sizeX = 4; // X(����) ���� ����
    public int sizeZ = 4; // Z(����) ���� ����


    [ExecuteAlways]
    private void OnDrawGizmos()
    {
        // ���� ������ ä���
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.1f);
        Gizmos.DrawWireCube(transform.position, new Vector3(sizeX, 0.01f, sizeZ));
    }
}
