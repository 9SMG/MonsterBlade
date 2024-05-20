using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create_ItemData/Weapon", order = 0)] // CreateAssetMenu(defult 파일명, 메뉴 이름, 순번)
public class WeaponData : EquipmentData
{
	public enum WeaponType
	{
		Shield = 0,		// 방패
		Sword,			// 검
		Bow,			// 활
	}

	[Header("무기")]
	[Space(20)]

	[Tooltip("무기타입")]
	public WeaponType weaponType;

	[Tooltip("공격력")]
	public float Atk;

	public override void Use()
	{
		base.Use();
		Debug.Log("공격력: " + Atk);

		//switch (weaponType)
		//{
		//	case WeaponType.Shield:
		//		Debug.Log("공격력: " + Atk);
		//		break;

		//	case WeaponType.Sword:
		//		Debug.Log("공격력: " + Atk);
		//		break;

		//	case WeaponType.Bow:
		//		Debug.Log("공격력: " + Atk);
		//		break;
		//}
	}
}
