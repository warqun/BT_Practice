using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.StandaloneInputModule;

public class PlayerController : MonoBehaviour
{
    [Header("# Camera")]
    public Camera mainCamera; // 주 카메라

    AliveObject state = null;
    float playerMoveVerctorX = 0;
    float playerMoveVerctorY = 0;
    [Header("#SPEED PAR")]   
    float speed = 0f;

    private int SkillIndex = 0; // Index for the current skill

    [SerializeField]
    public GameObject[] Skills; // Prefab for the projectile
    public GameObject skillPos; // 발사 위치

    [Header("#DASH PAR")]
    /// 마우스 위치로 대쉬
    /// 대쉬 상태에서 이동입력가능함.
    /// 
    bool isDash = false;
    Vector3 dashVector = Vector3.zero;
    public float dashSpeed = 10f;
    float dashCollTime = 0.3f;
    float curDashCollTime = 0f;

    private Animator Animator;

    Rigidbody rigbd = null;

    private void Start()
    {
        state = null;
        state = GetComponent<AliveObject>();
        if(state != null)
            speed = state.GetStatusValue(ObjectDataType.AliveObjectStatus.Speed);

        rigbd = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        
    }
    /// <summary>
    /// 대쉬 가능 여부
    /// </summary>
    bool IsDashVaild { get { return isDash == false && curDashCollTime <= 0; } }
    private void Update()
    {
        /// Player Move Controller
        {
            playerMoveVerctorX = Input.GetAxisRaw("Horizontal");
            playerMoveVerctorY = Input.GetAxisRaw("Vertical");

            Vector3 move = new Vector3(playerMoveVerctorX, 0f, playerMoveVerctorY).normalized;
            Vector3 moveDirection = transform.TransformDirection(move);

            if (move != Vector3.zero)
            {
                Animator.SetBool("Moving", true);
            }
            else
            {
                Animator.SetBool("Moving", false);
            }
        }

        // Player Skill Controller
        skillSwap();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Animator.SetTrigger("Shooting");
            Instantiate(Skills[SkillIndex], skillPos.transform.position, skillPos.transform.rotation);
        }

        //대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && IsDashVaild == true)
        {
            isDash = true;
            // 현재 속도 초기화
            rigbd.linearVelocity = Vector3.zero;
            
            //대쉬 방향
            dashVector = Vector3.zero;
            
            // 입력 방향으로 대시 방향 설정
            Vector3 inputDirection = new Vector3(playerMoveVerctorX, 0, playerMoveVerctorY).normalized;
            if (inputDirection != Vector3.zero)
            {
                dashVector = inputDirection * dashSpeed;
            }
            else
            {
                // 입력이 없을 경우 정면 방향으로 대시
                dashVector = transform.forward * dashSpeed;
            }

            // AddForce에 현재 방향을 기준으로 하는 힘을 가합니다.
            rigbd.AddForce(dashVector, ForceMode.Impulse);

            curDashCollTime = dashCollTime;
        }
    }

    void skillSwap()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SkillIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) SkillIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) SkillIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) SkillIndex = 3;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || state.isAlive == false)
        {
            return;
        }

        // MoveController
        float speedBuff = state.GetBuffValue(ObjectDataType.AliveObjectStatus.Speed);
        Vector3 moveVec = new Vector3(playerMoveVerctorX, 0, playerMoveVerctorY);
        transform.Translate(moveVec.normalized * (speed * speedBuff) * Time.fixedDeltaTime);

        //대쉬 쿨타임 감소
        if(curDashCollTime > 0)
        {
            curDashCollTime -= Time.fixedDeltaTime;
            
            // 대시 중 y축 속도 제한
            Vector3 currentVelocity = rigbd.linearVelocity;
            currentVelocity.y = 0;
            rigbd.linearVelocity = currentVelocity;
        }
        else if(curDashCollTime <= 0)
        {
            isDash = false;
            // 대시 종료 시 속도 초기화
            rigbd.linearVelocity = Vector3.zero;
        }
    }
}
