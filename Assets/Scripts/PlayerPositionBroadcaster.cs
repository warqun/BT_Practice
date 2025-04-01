using Unity.Mathematics;
using UnityEngine;

public class PlayerPositionBroadcaster : MonoBehaviour
{
    public static float3 Position;

    void Update()
    {
        Position = transform.position;
    }
}