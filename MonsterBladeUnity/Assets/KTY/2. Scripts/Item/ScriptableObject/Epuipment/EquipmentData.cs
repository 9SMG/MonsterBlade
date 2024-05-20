using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentData : ItemData
{
	public enum EquipmentType
	{
		Weapon = 0,     // ¹«±â
		Armor,			// ¹æ¾î±¸
	}

	[Header("Àåºñ")]
	[Space(20)]

	public EquipmentType equipmentType;

	[Space(20)]

	[Tooltip("Âø¿ë ¿©ºÎ")]
	public bool isWearable;

	public override void Use()
	{
		//Debug.Log(this.name + " ÀåÂø");
		ItemManager.instance.Equip(this); // ½Ì±ÛÅæ ÀÏ ¶§
		//itemManager.Equip(equipmentData); // ½Ì±ÛÅæÀÌ ¾Æ´Ò ¶§

	}
}
