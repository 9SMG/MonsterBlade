using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance; // 싱글톤 일 때

    [SerializeField] GameManager gManager; // 플레이어 스탯 관련 클래스가 오게 될 것.

    enum Consumable { HPPotion = 2001, MPPotion, SPPotion, CLPotion, Mochi, }
    Consumable csNum;

    bool isActive;

    private void Awake()
	{
        instance = this;  // 싱글톤 일 때

        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
                gManager.hp += 500; // 하드 코딩 말고 따로 직접 조작 가능한 변수로 뺄 수도.
                break;

            case Consumable.MPPotion:
                gManager.mana += 500;
                break;

            case Consumable.SPPotion:
                gManager.stamina += 100;
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
            gManager.hp += 100;
            yield return new WaitForSeconds(1.0f);
		}
        yield return null;
	}
}
