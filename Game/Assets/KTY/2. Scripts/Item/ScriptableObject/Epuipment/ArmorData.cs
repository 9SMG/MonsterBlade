using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create_ItemData/Armor", order = 1)] // CreateAssetMenu(defult 파일명, 메뉴 이름, 순번)
public class ArmorData : EquipmentData
{
	public enum ArmorType // 얜 필요한가?
	{
		Cloth = 0,		// 천
		Leather,        // 가죽
		Plate,          // 판금
	}

	[Header("방어구")]
	[Space(20)]

	[Tooltip("방어구타입")]
	public ArmorType armorType;

	[Tooltip("방어력")]
	public float Def;

	[Tooltip("최대 체력 증가량")]
	public float MaxHpPlus;

	public override void Use()
	{
		base.Use();
		Debug.Log("방어력: " + Def + "\n체력+: " + MaxHpPlus);
	}
}
