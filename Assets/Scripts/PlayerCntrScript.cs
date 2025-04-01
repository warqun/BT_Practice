using UnityEngine;

public class PlayerCntrScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 moveDirection = transform.TransformDirection(move);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
    }
}

