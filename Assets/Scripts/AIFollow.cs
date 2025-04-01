using System;
using UnityEngine;

public class AIFollow : MonoBehaviour
{
    public Transform playerPos; // ??? ????
    private GameObject player;
    public float moveSpeed = 3f; // ?? ??
    public float stoppingDistance = 1f; // ?????? ?? ??

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
