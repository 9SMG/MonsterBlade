using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "ItemSO", menuName = "Create_ItemData", order = 0)] // CreateAssetMenu(defult 파일명, 메뉴 이름, 순번)
public abstract class ItemData : ScriptableObject // 추상 클래스
{
	// 아이템 타입
	public enum ItemType
	{
		None = 0,
		Equipment,		// 장비 아이템
		Consumable,		// 소비 아이템
		Others,			// 기타 아이템
		//Gold,			// 재화: 골드
	}

	[Header("아이템")]

	[Tooltip("아이템 번호")]
	public int itemNum;

	[Space(20)]

	[Tooltip("아이템 타입")]
	public ItemType itemType;


	[Tooltip("아이템 스프라이트")]
	public Sprite itemSpr;

	//[Tooltip("아이템 3D 모델")]
	//public GameObject itemModel;


	[Tooltip("아이템 이름")]
	public string itemName;

	[Tooltip("아이템 설명")] [Multiline(5)]
	public string description;


	[Space(20)]
	[Tooltip("스택형 여부")]
	public bool isStackable;


	public virtual void Use() { }
}
