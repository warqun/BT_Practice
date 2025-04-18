using UnityEngine;

public class Vid_PlayerCntl : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator Animator;
    private int SkillIndex = 0; // Index for the current skill

    [SerializeField]
    public GameObject[] Skills; // Prefab for the projectile
    public GameObject skillPos; // 발사 위치

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        skillSwap();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Animator.SetTrigger("Shooting");
            Instantiate(Skills[SkillIndex], skillPos.transform.position, skillPos.transform.rotation);
        }
    }

    void skillSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SkillIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) SkillIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) SkillIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) SkillIndex = 3;
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        Vector3 moveDirection = transform.TransformDirection(move);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (move != Vector3.zero)
        {
            Animator.SetBool("Moving",true);
        }
        else
        {
            Animator.SetBool("Moving",false);
        }
    }
}
