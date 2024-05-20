using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance; // �̱��� �� ��

    [SerializeField] GameManager gManager; // �÷��̾� ���� ���� Ŭ������ ���� �� ��.

    enum Consumable { HPPotion = 2001, MPPotion, SPPotion, CLPotion, Mochi, }
    Consumable csNum;

    bool isActive;

    private void Awake()
	{
        instance = this;  // �̱��� �� ��

        gManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	}

	/// <summary>
	/// ��� ����
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
    /// �Ҹ�ǰ ���
    /// </summary>
    /// <param name="_cs"></param>
    public void Consume(ConsumableData _cs)
	{
        csNum = (Consumable)_cs.itemNum;


		switch (csNum)
		{
            case Consumable.HPPotion:
                gManager.hp += 500; // �ϵ� �ڵ� ���� ���� ���� ���� ������ ������ �� ����.
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
                float mochiTime = Time.time; // ���� �ð� ���
                StartCoroutine(MochiBuff(mochiTime));
                break;

            default:
                return;
        }
    }

    public bool BuffCoolTime(float coolTime, bool isBuff)
	{
        isActive = isBuff;
        Debug.Log("�Լ� ���� ����");
        StartCoroutine(CoolTime(coolTime));
        return isBuff = isActive;
	}

    // ���� ������� ��� �ذ�����?
    IEnumerator CoolTime(float coolTime)
	{
        Debug.Log("�ڷ�ƾ ���� ����");
        isActive = true;
        yield return new WaitForSeconds(coolTime);
        Debug.Log("��Ÿ�� ���� ����");
        isActive = false;
	}

    /// <summary>
    /// ���Ҷ� ���
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
