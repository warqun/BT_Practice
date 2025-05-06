using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AliveObject : MonoBehaviour
{
    public ObjectDataType.AliveObjectType type = ObjectDataType.AliveObjectType.None;
    public bool isAlive = true;

    // ObjectDataType.AliveObjectStatus 관련된 상태 데이터 배열
    float[] statusValue = new float[(int)ObjectDataType.AliveObjectStatus.MAX];
    public float GetStatusValue(ObjectDataType.AliveObjectStatus statusType)
    {
        return statusValue[(int)statusType];
    }
    public void SetStatusValue(ObjectDataType.AliveObjectStatus statusType, float value)
    {
        statusValue[(int)statusType] = value;
    }

    protected float resDamageTick=0.5f;
    protected bool allowDamage = true;
    public bool AllowDamage { get { return allowDamage; } set { allowDamage = value; } }
    /// <summary>
    /// 무기 사이클에 대한 딕셔너리
    /// </summary>
    protected Dictionary<WeaponBase, float> weaponCycle = new Dictionary<WeaponBase, float>();

    /// <summary>
    /// 현재 AliveObject가 가진 모든 버프
    /// </summary>
    protected List<BuffBase> buffList = new List<BuffBase>();

    protected virtual void Start()
    {
        isAlive = true;
        Debug.LogFormat("[Alive][State] {0} State - HP:{1}, Speed:{2}, AP:{3}, DP:{4}, RP:{5}, Type:{6}",
            gameObject.name, 
            GetStatusValue(ObjectDataType.AliveObjectStatus.HP), 
            GetStatusValue(ObjectDataType.AliveObjectStatus.Speed), 
            GetStatusValue(ObjectDataType.AliveObjectStatus.BasicDamage), 
            GetStatusValue(ObjectDataType.AliveObjectStatus.DP), 
            GetStatusValue(ObjectDataType.AliveObjectStatus.HPRegen), 
            type);

        // 모든 자식 오브젝트에서 WeaponBase 찾기
        WeaponBase[] weapons = GetComponentsInChildren<WeaponBase>(true); // 비활성화된 오브젝트도 포함

        if (weapons.Length > 0)
        {
            foreach (var weapon in weapons)
            {
                weapon.master = this;
                Debug.LogFormat("[Alive][Weapon][Find] {0}에서 무기 {1}를 찾았습니다.", gameObject.name, weapon.name);
            }
        }
        else
        {
            Debug.LogWarningFormat("[Alive][Weapon][Find] {0}에서 WeaponBase를 가진 자식 오브젝트를 찾을 수 없습니다. 무기가 필요하지 않은 경우 이 경고를 무시해도 됩니다.", gameObject.name);
        }
    }

    // 무기 사이클에 대한 이벤트 함수
    public virtual float DamageReqEvnet()
    {
        float attackPoint = 0;
        attackPoint = GetStatusValue(ObjectDataType.AliveObjectStatus.BasicDamage);
        Debug.LogFormat("[Alive][Damage][REQ] {0} - Damage:{1}",gameObject.name, GetStatusValue(ObjectDataType.AliveObjectStatus.BasicDamage));
        /// TODO
        /// 무기 사이클에 대한 추가 처리 필요
        /// 무기 사이클에 대한 상태 처리 필요 
        
        return attackPoint;
    }
    // 무기 사이클에 대한 결과 이벤트 함수
    public virtual void DamageResEvnet(float damage)
    {
        /// 무기 사이클에 대한 추가 처리 필요
        /// 무기 사이클에 대한 상태 처리 필요 
        Debug.LogFormat("[Alive][Damage][RES] {0} - Damage:{1}, RemainHP:{2}", gameObject.name, damage, GetStatusValue(ObjectDataType.AliveObjectStatus.HP));
    }
    // 무기 사이클에 대한 이벤트 함수
    public virtual void DamageEvnet()
    {
        Debug.LogFormat("[Alive][Damage][Event] Name:{0}, Type:{1}", gameObject.name, type);
    }

    public float GetBuffValue(ObjectDataType.AliveObjectStatus status)
    {
        float value = 1;
        for(int i = 0; i<buffList.Count; i++)
        {
            value += buffList[i].GetBuffValue(status);
        }
        return value+1;
    }

    float damageTimer = 0;
    protected virtual void Update()
    {
        {
            // 무기 사이클에 대한 타이머 처리
            if (allowDamage == false)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer > resDamageTick)
                {
                    damageTimer = 0;
                    allowDamage = true;
                }
            }
        }
    }
}