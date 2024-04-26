using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : ItemData
{
	public enum EquipmentType
	{
		Weapon = 0,     // 무기
		Armor,			// 방어구
	}

	[Header("장비")]
	[Space(20)]

	public EquipmentType equipmentType;

	[Space(20)]

	[Tooltip("착용 여부")]
	public bool isWearable;

	public override void Use()
	{
		Debug.Log(this.name + " 장착");
	}
}
