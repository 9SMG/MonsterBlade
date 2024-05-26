using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance; // 싱글톤 일 때

	[SerializeField] GameManager gManager; // 플레이어 스탯 관련 클래스가 오게 될 것.
	[SerializeField] StatManager statManager; // 플레이어 관련 스탯 연결을 위함.

    enum Consumable { HPPotion = 2001, MPPotion, SPPotion, CLPotion, Mochi, }
    Consumable csNum;

    bool isActive;

    private void Awake()
	{
        instance = this;  // 싱글톤 일 때

		gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
		statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();

    }

	/// <summary>
	/// 장비 장착
	/// </summary>
	/// <param name="_eq"></param>
	public void Equip(EquipmentData _eq)
	{
		switch (_eq.equipmentType)
		{
            case EquipmentData.EquipmentType.Weapon:
                gManager.Eq1 = (WeaponData)_eq;
                break;
            case EquipmentData.EquipmentType.Armor:
                gManager.Eq2 = (ArmorData)_eq;
                break;
		}
	}

    /// <summary>
    /// 소모품 사용
    /// </summary>
    /// <param name="_cs"></param>
    public void Consume(ConsumableData _cs)
	{
        csNum = (Consumable)_cs.itemNum;


		switch (csNum)
		{
            case Consumable.HPPotion:
                statManager.statInfo._curHP += 500;
                if(statManager.statInfo._curHP > statManager.statInfo.hpMax)
                {
                    statManager.statInfo._curHP = statManager.statInfo.hpMax;
                }
                break;

            case Consumable.MPPotion:
                statManager.statInfo._curMP += 500;
                if (statManager.statInfo._curMP > statManager.statInfo.mpMax)
                {
                    statManager.statInfo._curMP = statManager.statInfo.mpMax;
                }
                break;

            case Consumable.SPPotion:
                statManager.statInfo._curStamina += 100;
                if (statManager.statInfo._curStamina > statManager.statInfo.staminaMax)
                {
                    statManager.statInfo._curStamina = statManager.statInfo.staminaMax;
                }
                break;

            case Consumable.CLPotion:
                gManager.isSick = false;
                break;

            case Consumable.Mochi:
                float mochiTime = Time.time; // 먹은 시간 기록
                StartCoroutine(MochiBuff(mochiTime));
                break;

            default:
                return;
        }
    }

    public bool BuffCoolTime(float coolTime, bool isBuff)
	{
        isActive = isBuff;
        Debug.Log("함수 실행 됬음");
        StartCoroutine(CoolTime(coolTime));
        return isBuff = isActive;
	}

    // 버프 디버프는 어떻게 해결하지?
    IEnumerator CoolTime(float coolTime)
	{
        Debug.Log("코루틴 실행 됬음");
        isActive = true;
        yield return new WaitForSeconds(coolTime);
        Debug.Log("쿨타임 실행 됬음");
        isActive = false;
	}

    /// <summary>
    /// 찹쌀떡 기능
    /// </summary>
    /// <param name="_time"></param>
    /// <returns></returns>
    IEnumerator MochiBuff(float _time)
	{
		while (Time.time - _time <= 5.0f)
		{
            statManager.statInfo._curHP += 100;
            if (statManager.statInfo._curHP > statManager.statInfo.hpMax)
            {
                statManager.statInfo._curHP = statManager.statInfo.hpMax;
            }
            yield return new WaitForSeconds(1.0f);
		}
        yield return null;
	}
}
