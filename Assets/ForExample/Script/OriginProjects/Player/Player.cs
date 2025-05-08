using UnityEngine;

public class Player : AliveObject
{
    [HideInInspector]
    public Rigidbody rigidbody = null;
    
    private void Awake()
    {
        // ���߿� DB�� ���� ���� �ý����� ���� �������� �ٸ� ������ ����ϰ� ����.
        SetStatusValue(ObjectDataType.AliveObjectStatus.HP,100);
        SetStatusValue(ObjectDataType.AliveObjectStatus.DP,1);
        SetStatusValue(ObjectDataType.AliveObjectStatus.Speed,3);
        SetStatusValue(ObjectDataType.AliveObjectStatus.BasicDamage,3);

        resDamageTick = 5;
        type = ObjectDataType.AliveObjectType.Player;
        rigidbody = GetComponent<Rigidbody>();

    }
    protected override void Start()
    {
        base.Start();
    }

    public override float DamageReqEvnet()
    {
        float attackPoint = 0;
        attackPoint += GetStatusValue(ObjectDataType.AliveObjectStatus.BasicDamage);
        Debug.LogFormat("[Player][Damage][REQ] {0} - Damage:{1}", gameObject.name, attackPoint);
        float fullDamage = attackPoint * GetBuffValue(ObjectDataType.AliveObjectStatus.BasicDamage);
        return fullDamage;
    }
    public override void DamageResEvnet(float damage)
    {
        float hp = GetStatusValue(ObjectDataType.AliveObjectStatus.HP);
        float dp = GetStatusValue(ObjectDataType.AliveObjectStatus.DP);

        hp = damage - dp * GetBuffValue(ObjectDataType.AliveObjectStatus.DP);
        SetStatusValue(ObjectDataType.AliveObjectStatus.HP, hp);
        if (hp <= 0)
            isAlive = false;
        Debug.LogFormat("[Player][Damage][RES] {0} - Damage:{1}, RemainHP:{2}", gameObject.name, damage, hp);
    }
    /// Ʈ���ſ� �浹������

    private void OnTriggerEnter(Collider other)
    {
        // ���̽����� ���⿡ ���� ����Ÿ�̸� ������.
        {
            /// ���⿡ ���� ������
            /// Ư�� ��쿡 ��Ʈ ������ ���;��ϴ� ���.
            WeaponBase triggerWeapon = other.GetComponent<WeaponBase>();

            if (triggerWeapon != null)
            {
                if (!weaponCycle.ContainsKey(triggerWeapon))
                {
                    if (triggerWeapon.master == null)
                        return;
                    // �׾��ִ� �����϶� ������ ���� ����.
                    if (triggerWeapon.master.isAlive == false)
                        return;
                    // ���� ���ο� ���� ������ 
                    switch (triggerWeapon.master.type)
                    {
                        case ObjectDataType.AliveObjectType.Mob:
                            // ���⿡ ���� �������� ����� �߰�.
                            ObjectDamage damageReport = new ObjectDamage(triggerWeapon.DamageReqEvnet, DamageResEvnet);
                            damageReport.AddEvnet(DamageEvnet);
                            damageReport.AddEvnet(triggerWeapon.DamageEvnet);
                            GameBase.gameBase.AddDamageEvent(damageReport);
                            break;
                    }

                    // ���� �߿� ������ ���.
                    float weaponTimer = triggerWeapon.WeaponDamageTypeTick();
                    if (weaponTimer > 0)
                    {
                        weaponCycle[triggerWeapon] = triggerWeapon.WeaponDamageTypeTick();
                        Debug.Log("[Player][WEAPON][STAY][ENTER] CHEKCIN ENTER WEAPON");
                    }
                }
            }

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (allowDamage == true)
        {
            {
                // ���̷�Ʈ ������ ������.
                // ���̷�Ʈ ������.
                //AliveObject triggerObject = other.GetComponent<AliveObject>();
                //if (triggerObject != null)
                //{
                //    switch (triggerObject.type)
                //    {
                //        case ObjectDataType.AliveObjectType.Mob:
                //            ObjectDamage damageReport = new ObjectDamage(triggerObject.DamageReqEvnet, DamageResEvnet);
                //            damageReport.AddEvnet(DamageEvnet);
                //            damageReport.AddEvnet(triggerObject.DamageEvnet);

                //            // ������ �Ǵ� �߰��Ǵ� ����� �����.

                //            GameBase.gameBase.AddDamageEvent(damageReport);
                //            break;
                //    }
                //}
            }
            // ���� Ȱ��ȭ.
            allowDamage = false;
        }
        // ���̽����� ���⿡ ���� ����Ÿ�̸� ������.
        {
            /// ���⿡ ���� ������
            /// Ư�� ��쿡 ��Ʈ ������ ���;��ϴ� ���.
            WeaponBase triggerWeapon = other.GetComponent<WeaponBase>();
            if (triggerWeapon != null)
            {
                if (triggerWeapon.master == null)
                    return;
                // �׾��ִ� �����϶� ������ ���� ����.
                if (triggerWeapon.master.isAlive == false)
                    return;
                float weaponTime = 0;
                if (weaponCycle.TryGetValue(triggerWeapon, out weaponTime))
                {
                    // 0 ���� ���� ������ Ÿ�̸Ӵ� �۵����� �ʴ´�.
                    if (weaponTime > 0)
                    {
                        weaponTime -= Time.fixedDeltaTime;
                        // Ÿ�̸� �Ҹ�� ���󺹱�
                        bool useTickDamage = false;
                        if (weaponTime < 0)
                        {
                            weaponCycle[triggerWeapon] = triggerWeapon.WeaponDamageTypeTick();
                            useTickDamage = true;
                        }
                        else
                        {
                            weaponCycle[triggerWeapon] = weaponTime;
                            useTickDamage = false;
                        }
                        // Ÿ�̸Ӱ� �Ǿ����� ������ ���.
                        if (useTickDamage)
                        {
                            switch (triggerWeapon.master.type)
                            {
                                case ObjectDataType.AliveObjectType.Player:
                                    // ���⿡ ���� �������� ����� �߰�.
                                    ObjectDamage damageReport = new ObjectDamage(triggerWeapon.DamageReqEvnet, DamageResEvnet);
                                    damageReport.AddEvnet(DamageEvnet);
                                    damageReport.AddEvnet(triggerWeapon.DamageEvnet);
                                    GameBase.gameBase.AddDamageEvent(damageReport);
                                    break;
                            }
                        }
                    }
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {

        WeaponBase triggerWeapon = other.GetComponent<WeaponBase>();
        if (triggerWeapon != null)
        {
            if (weaponCycle.ContainsKey(triggerWeapon))
            {
                weaponCycle.Remove(triggerWeapon);
                Debug.Log("[Player][WEAPON][STAY][EXIT] CHEKCOUT EXIT WEAPON");
            }
        }
    }
    public override void DamageEvnet()
    {
        Debug.Log("[Player][Damage] event ResDamage");
    }
}
