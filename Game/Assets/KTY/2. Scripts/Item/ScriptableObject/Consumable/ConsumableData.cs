using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create_ItemData/Consumable", order = 12)] // CreateAssetMenu(defult 파일명, 메뉴 이름, 순번)
public class ConsumableData : ItemData
{
	public enum ConsumableType // 좀 더 생각하기
	{
		Active = 0,		// 액티브?
		Potion,			// 물약
		Ammo,			// 탄약
		Food,			// 음식
	}

	[Header("소모품")]
	[Space(20)]

	[Tooltip("소모품 타입")]
	public ConsumableType consumableType;

	[Space(20)]

	[Tooltip("소모가능 여부")]
	public bool isEatable;

	public float coolTime;

	public override void Use()
	{
		Debug.Log(this.name + " 사용");
	}
}
