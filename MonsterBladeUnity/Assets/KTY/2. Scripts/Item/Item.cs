using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public ItemData itemData;
	public int amount = 1;

	public SlotInfo _itemInfo = new SlotInfo();

	private void Start()
	{
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


			InventoryManager.instance.AddItem(_itemInfo);
			Destroy(gameObject);
		}
	}
}
