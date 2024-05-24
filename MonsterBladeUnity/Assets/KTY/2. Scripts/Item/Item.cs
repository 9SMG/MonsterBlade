using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	// 아이템 데이타
	public ItemData itemData;
	// 수량
	public int amount = 1;

	// 획득 시 슬롯 정보가 될 아이템 정보 객체 생성
	public SlotInfo _itemInfo = new SlotInfo();

	private void Start()
	{
		// 정보 값 할당
		_itemInfo.item = itemData;
		_itemInfo.amount = amount;
	}

	private void OnTriggerEnter(Collider col)
	{
		// 플레이어와 닿았고 && (인벤토리가 꽉 차지 않았거나 or (인벤 꽉찼는데 && 스택형이고 && 딕셔너리에 있다면))
		if (col.CompareTag("Player") && (!InventoryManager.instance.fullInven || (InventoryManager.instance.fullInven && itemData.isStackable && ItemDictionary.CheckData(itemData.itemNum))))
		{
			// 아이템 고정(컬리더 중복 작동으로 인한 수치 오류 방지를 위함)
			Collider coll = GetComponent<Collider>();
			coll.enabled = false;
			Rigidbody rig = GetComponent<Rigidbody>();
			rig.isKinematic = true;

			SoundManager.Instance.PlaySound2D("BellishAccept6", 0f, false, SoundType.EFFECT);

			InventoryManager.instance.AddItem(_itemInfo);
			Destroy(gameObject);
		}
	}
}
