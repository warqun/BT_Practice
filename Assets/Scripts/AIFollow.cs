using System;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    public Transform playerPos; // 추적할 플레이어
    private GameObject player;
    public float moveSpeed = 3f; // 이동 속도
    public float stoppingDistance = 1f; // 플레이어와의 최소 거리

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }


    void Update()
    {
        if (player == null)
            return;
        playerPos = player.transform;

        float distance = Vector3.Distance(transform.position, playerPos.position);


        if (distance > stoppingDistance)
        {
            Vector3 direction = (playerPos.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
