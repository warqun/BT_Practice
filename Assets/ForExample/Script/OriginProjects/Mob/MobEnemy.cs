using UnityEngine;
using UnityEngine.AI;

public class MobEnemy : MobRoot
{
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;
    private IEnemyState currentState;

    [Header("Health Set")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Attack Set")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0;

    [Header("Detection Set")]
    public float detectionRange = 10f;
    public float attackDetectionRange = 2f;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        // 플레이어의 transform을 static으로 참조
        if (PlayerController.playerTransform == null)
        {
            Debug.LogWarning("[MobEnemy] Player transform not assigned.");
        }
        
        // 초기 상태를 대기 상태로 설정
        ChangeState(new ChaseState(this));
    }

    protected override void FrameMobMove()
    {
        currentState?.Update();
    }

    public void ChangeState(IEnemyState newState)
    {
        Debug.Log($"[MobEnemy] State changed to: {newState.GetType().Name}");
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("Hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        agent.ResetPath();
        enabled = false;
        // TODO: 사망 처리 (파괴 또는 비활성화)
    }

    public float GetDistanceToTarget()
    {
        if (target == null) return float.MaxValue;
        return Vector3.Distance(transform.position, target.position);
    }

    public void MoveToTarget()
    {
        target = PlayerController.playerTransform;
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    public void StopMoving()
    {
        agent.ResetPath();
    }

    public void TryAttack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        animator.SetTrigger("Attack");
        float damage = DamageReqEvnet();
        GameBase.gameBase.player.DamageResEvnet(damage);
    }
}

// 상태 패턴 인터페이스
public interface IEnemyState
{
    void Enter();
    void Update();
    void Exit();
}

// 대기 상태
public class IdleState : IEnemyState
{
    private MobEnemy enemy;

    public IdleState(MobEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.StopMoving();
    }

    public void Update()
    {
        if (enemy.GetDistanceToTarget() <= enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }

    public void Exit() { }
}

// 추격 상태
public class ChaseState : IEnemyState
{
    private MobEnemy enemy;

    public ChaseState(MobEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.MoveToTarget();
    }

    public void Update()
    {
        float distance = enemy.GetDistanceToTarget();
        
        if (distance > enemy.detectionRange)
        {
            enemy.ChangeState(new IdleState(enemy));
        }
        else if (distance <= enemy.attackDetectionRange)
        {
            enemy.ChangeState(new AttackState(enemy));
        }
        else
        {
            enemy.MoveToTarget();
        }
    }

    public void Exit() { }
}

// 공격 상태
public class AttackState : IEnemyState
{
    private MobEnemy enemy;

    public AttackState(MobEnemy enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        enemy.StopMoving();
    }

    public void Update()
    {
        float distance = enemy.GetDistanceToTarget();
        
        if (distance > enemy.attackDetectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
        else
        {
            enemy.TryAttack();
        }
    }

    public void Exit() { }
}