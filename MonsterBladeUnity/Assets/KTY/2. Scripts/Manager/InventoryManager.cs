using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotInfo
{
	// ������
	public ItemData item;

	// ����
	public int amount;

	// ��ü���⿡�� ��������? �������� �ʴ°ɱ�?
	[HideInInspector] public float remainTime; // ���� ��Ÿ��
}


public class InventoryManager : MonoBehaviour, IComparer<SlotInfo>
{
	// �̱���
	public static InventoryManager instance;

	#region ���� ����
	GameObject inven;
	Transform invenSlots; // �ڵ带 ���̱� ���� ����.

	Transform goldPnl; // ��� �г�
	Text goldTxt; // ��� �ؽ�Ʈ

	int gold = 0; // ���� ���
	public int Gold
	{
		set { gold = value; }
		get { return gold; }
	}

	//List<InvenSlot> slots = new List<InvenSlot>(); // ���� ����Ʈ

	const int SLOTSIZE = 24; // �ִ� ���� ������
	InvenSlot[] slots = new InvenSlot[SLOTSIZE];



	//public List<SlotInfo> slotInfos = new List<SlotInfo>();


	//public List<ItemData> itemDatas = new List<ItemData>(); // ȹ���� ������ ����Ʈ

	//int prevListCount; // ���� ����Ʈ ��

	bool isOpen = false; // �κ��丮 ���� ����

	public bool fullInven = false; // �κ��丮 ���� ����

	#endregion

	/// <summary>
	/// �κ��丮 �� ��� �帣�� �ð�
	/// </summary>
	[SerializeField] float invenTimer; // �ð� üũ ��

	/// <summary>
	/// ��Ȱ��ȭ�� �ð�
	/// </summary>
	[SerializeField] float disableTimer;

	private void Awake()
	{
		#region �̱��� ȭ
		instance = this;
		#endregion

		inven = GameObject.FindGameObjectWithTag("Inventory");
		invenSlots = inven.transform.GetChild(1);

		goldPnl = inven.transform.GetChild(2);
		goldTxt = goldPnl.GetComponentInChildren<Text>();

		// ���� ������ ���� ����
		for (int i = 0; i < SLOTSIZE; i++)
		{
			//slots.Add(invenSlots.GetChild(i).GetComponent<InvenSlot>());
			//slotInfos.Add(invenSlots.GetChild(i).GetComponent<InvenSlot>().slotInfo);

			slots[i] = invenSlots.GetChild(i).GetComponent<InvenSlot>();
		}

	}
	// Start is called before the first frame update
	void Start()
    {
		InventoryInit();
	}

    // Update is called once per frame
    void Update()
    {
		InventoryUpdate();

#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.B))
		{
			OpenInventory();
		}

#elif UNITY_STANDALONE_WIN
		if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.B))
		{
			OpenInventory();
		}
#endif
	}

	// �κ��丮 ����
	void OpenInventory()
	{
		isOpen = !isOpen;
		inven.SetActive(isOpen);
		FollowWindow.instance.CloseWindow();

		// ���� ��Ÿ�� ���
		if (!isOpen)
		{
			DisabledTime(); // â�� ���� �ð� üũ
		}
		else
		{
			EnabledTime(); // â�� ���� ���� �ð� ���
		}
	}

	// �ʱ�ȭ
	void InventoryInit()
	{
		inven.SetActive(isOpen);
	}

	void InventoryUpdate()
	{
		//invenTimer += Time.deltaTime;
		invenTimer = Time.time;
		goldTxt.text = gold.ToString("0 G");
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// ������ ȹ�� �Լ� (������ ���� ����)
	/// </summary>
	/// <param name="_item">������ ������ ����</param>
	public void AddItem(SlotInfo _item)
	{
		ItemData _data = _item.item; // ���� ����ȭ�� ���� ����

		// �ߺ� üũ
		if (!ItemDictionary.CheckData(_data.itemNum)) // �ش� key�� ���ٸ�
		{
			// ��ųʸ��� ��� --
			ItemDictionary.SetData(_data.itemNum, _data);

			FindEmptySlot(_item);
		}
		else
		{
			Debug.Log("�ߺ� ������ ȹ��");

			if (_data.isStackable)
			{
				FindIncreaseSlot(_item);
			}
			else
			{
				FindEmptySlot(_item);
			}
		}

		#region *���� ����Ʈ ���
		///*
		// * <����Ʈ ���>
		// * ������ Ŭ�������� �Լ��� ȣ��ȴ� (������ ����, ȹ���� ����)
		// * ������ ����Ʈ�� �ִ´�. �ش� ������ ������ �����Ѵ�.
		// * 
		// * ������ Ŭ�������� �Լ��� ȣ��ȴ� "
		// * ������ ����Ÿ�� ���� ������ ����Ʈ�� ���Ѵ�.
		// * �ߺ��� ����Ÿ�� �ִٸ� ������ �����Ѵ�.
		// * �ߺ��� ����Ÿ�� ���ٸ� ������ ����Ʈ�� �ִ´�. �ش� ������ ������ �����Ѵ�.
		// */
		//
		//if (itemDatas.Count == 0) // ����Ʈ�� �ƹ��͵� ���� ��
		//{
		//	itemDatas.Add(_itemData);
		//	slots[0].Item = _itemData;
		//	slots[0].ItemImg.sprite = _itemData.itemSpr;
		//	slots[0].ItemAmount += amount;
		//}
		//else
		//{
		//	var checkData = _itemData; // �ߺ� üũ�� ����
		//
		//	if (itemDatas.Contains(checkData) && checkData.isStackable) // ����Ʈ �ȿ� �ߺ��Ǵ� ���� �ְ� �������� �������� ��
		//	{
		//		for (int i = 0; i < itemDatas.Count; i++) // �ߺ��� ã�´�.
		//		{
		//			if (checkData == itemDatas[i]) // �ߺ��� ã����.
		//			{
		//				slots[i].ItemAmount += amount; // �ش� ������ ������ �������Ѷ�
		//				break;
		//			}
		//		}
		//	}
		//	else // �ߺ��Ǵ� ���� ���ų� ��ġ�� �������� �ƴ� ��
		//	{
		//		itemDatas.Add(_itemData); // ��Ҹ� �߰��Ѵ�.
		//
		//		int emptyIdx = 0; // ����ִ� ���Կ� �ֱ� ����.
		//
		//		for (int j = 0; j < slots.Count; j++)
		//		{
		//			if (slots[j].Item == null) // �� ������ �߰��ϸ� �ݺ����� ������
		//			{
		//				emptyIdx = j;
		//				break;
		//			}
		//		}
		//
		//		slots[emptyIdx].Item = _itemData;
		//		slots[emptyIdx].ItemImg.sprite = _itemData.itemSpr;
		//		slots[emptyIdx].ItemAmount += amount; // ������ �����Ѵ�.
		//	}
		//}
		#endregion

		Debug.Log(_data.itemName + ", " + _item.amount + "�� ȹ��");
	}

	/// <summary>
	/// ���� ��ų ���� ã��
	/// </summary>
	/// <param name="_item"></param>
	void FindIncreaseSlot(SlotInfo _item)
	{
		// ������ ���� ���� --
		int findIdx = 0; // ã�� �ε��� üũ����

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != _item.item) // ������ ������ ã�� ������ ���� �ʴٸ�
			{
				findIdx++; // �ε����� ����
			}
			else // ã�Ҵٸ� �ݺ��� Ż��
			{
				Debug.Log("�ߺ� �߰�: " + findIdx);
				break;
			}
		}

		slots[findIdx].slotInfo.amount += _item.amount; // �ش� ������ ���� ����
	}

	/// <summary>
	/// �� ���� ã��
	/// </summary>
	/// <param name="_item"></param>
	void FindEmptySlot(SlotInfo _item)
	{
		// ������ ��� --
		int emptyIdx = 0; // ����ִ� �ε��� üũ����

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != null)
			{
				emptyIdx++; // ������� �ʴٸ� �ε��� ����
			}
			else // �� ĭ �߰� �� �ݺ��� Ż��
			{
				if (emptyIdx == SLOTSIZE - 1)
				{
					fullInven = true;
				}
				else
				{
					fullInven = false;
				}

				break;
			}
		}
		slots[emptyIdx].slotInfo = _item;
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// �� �߰�
	/// </summary>
	/// <param name="_money"></param>
	public void AddGold(int _money)
	{
		gold += _money;
	}

	/// <summary>
	/// �� ����
	/// </summary>
	/// <param name="_money"></param>
	public void RemoveGold(int _money)
	{
		gold -= _money;
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// ��Ȱ��ȭ �� �ð�
	/// </summary>
	void DisabledTime()
	{
		disableTimer = invenTimer; // â�� ���� �ð��� üũ
	}
	/// <summary>
	/// Ȱ��ȭ�� �ð� ���
	/// </summary>
	void EnabledTime()
	{
		// ��Ȱ��ȭ ���� �帥 �ð� = ��ü �ð� - ��Ȱ��ȭ �� �ð�
		float activatedTime = invenTimer - disableTimer;

		disableTimer = 0; // ��Ȱ��ȭ �� �ð� �ʱ�ȭ

		foreach (var slot in slots)
		{
			slot.slotInfo.remainTime -= activatedTime;
		}
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// ������ ����
	/// </summary>
	public void SortItem()
	{
		List<SlotInfo> infoList = new List<SlotInfo>(); // ���� ������ ������ ���� ����Ʈ

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != null) // ������ ������ �ִ� �ֵ鸸 ����Ʈ�� �ֱ�
			{
				infoList.Add(slot.slotInfo);
			}
			else
				continue;
		}

		infoList.Sort(Compare);

		foreach (var info in infoList)
			Debug.Log(info.item.itemNum + info.item.itemName);

		for (int i = 0; i < SLOTSIZE; i++)
		{
			slots[i].Refresh();
		}

		for (int j = 0; j < infoList.Count; j++)
		{
			slots[j].slotInfo = infoList[j];
		}
	}

	/// <summary>
	/// IComparer �������̽� ���� �Լ�
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public int Compare(SlotInfo x, SlotInfo y)
	{
		if (x.item != null && y.item != null)		// �� ĭ�� �ƴ� ��
			return x.item.itemNum - y.item.itemNum;

		else if (x.item == null && y.item != null)	// ���� �� ĭ�� ��
			return -1;

		else if (x.item != null && y.item == null)	// �ڰ� �� ĭ�� ��
			return 1;

		else										// �� �� �� ĭ�� ��
			return 0;
	}
}
