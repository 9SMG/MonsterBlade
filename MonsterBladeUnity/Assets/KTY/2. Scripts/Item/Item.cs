using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	// ������ ����Ÿ
	public ItemData itemData;
	// ����
	public int amount = 1;

	// ȹ�� �� ���� ������ �� ������ ���� ��ü ����
	public SlotInfo _itemInfo = new SlotInfo();

	private void Start()
	{
		// ���� �� �Ҵ�
		_itemInfo.item = itemData;
		_itemInfo.amount = amount;
	}

	private void OnTriggerEnter(Collider col)
	{
		// �÷��̾�� ��Ұ� && (�κ��丮�� �� ���� �ʾҰų� or (�κ� ��á�µ� && �������̰� && ��ųʸ��� �ִٸ�))
		if (col.CompareTag("Player") && (!InventoryManager.instance.fullInven || (InventoryManager.instance.fullInven && itemData.isStackable && ItemDictionary.CheckData(itemData.itemNum))))
		{
			// ������ ����(�ø��� �ߺ� �۵����� ���� ��ġ ���� ������ ����)
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
