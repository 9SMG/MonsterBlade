using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ���콺�� ũ�ν� ������ ���� �� ������ ����â�� �߰� �ϱ� ����.

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	//ItemManager itemManager; // �̱����� �ƴ� �� ���

	public SlotInfo slotInfo; // <- ���� ���� ( ����: ������ ����, ������ ����, ��Ÿ�� ���� )

	int _id; // ������ ���� �ѹ�
	Image _img; // ������ ������ �̹���
	int _amount; // ������ ������ ����

	// ������Ƽ �����ٰ� ����
	//#region *������ ������
	//// ������ ������ ----------
	//ItemData _itemData;

	//public ItemData Item
	//{
	//	get { return _itemData; }
	//	set { _itemData = value; }
	//}
	//// ------------------------
	//#endregion

	//#region *������ ID
	//// ������ ID/Num ----------
	//int itemId;
	//public int ItemID
	//{
	//	get { return itemId; }
	//	set { itemId = value; }
	//}
	//#endregion

	//#region *������ �̹���
	//Image itemImg;

	//public Image ItemImg
	//{
	//	get { return itemImg; }
	//	set { itemImg.sprite = value.sprite; }
	//}
	//#endregion

	//#region *������ ����
	//int itemAmount = 0; // ������ ����

	//public int ItemAmount
	//{
	//	get { return itemAmount; }
	//	set { itemAmount = value; }
	//}
	//#endregion

	[HideInInspector] public bool fullSlot = false; // ���� ä���� ����

	Text amountTxt; // ���� �ؽ�Ʈ
	Button slotBtn; // ���� ��ȣ�ۿ� ��ư

	[Space(20)]
	[SerializeField] Sprite defSpr; // �ƹ��͵� ���� �� ����Ʈ ��������Ʈ

	[Space(20)]
	[SerializeField] bool isCool; // ����� ��Ÿ�� ����
	[SerializeField] float remainTime = 0;

	private void Awake()
	{
		//itemImg = transform.GetChild(0).GetComponent<Image>();
		_img = transform.GetChild(0).GetComponent<Image>();
		amountTxt = transform.GetComponentInChildren<Text>();

		slotBtn = GetComponent<Button>();

		//itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>(); // �̱��� ����� �ƴ� ��
	}

	//private void OnEnable()
	//{
	//	if (slotInfo.item != null)
	//	{
	//		InventoryManager.instance.ActivatedTimer(slotInfo);
	//	}
	//}
	//private void OnDisable()
	//{
	//	if (slotInfo.item != null)
	//	{
	//		InventoryManager.instance.DeactivatedTimer(slotInfo);
	//	}
	//}

	void Start()
    {
        SlotInit();
    }

    void Update()
    {
        SlotUpdate();
    }

	/// <summary>
	/// �⺻ ����
	/// </summary>
	void SlotSetting()
	{
		if (slotInfo.item != null)
		{
			if (slotInfo.amount != 0) // ������ 0�� �ƴ϶��
			{
				// ���� ������ ������ ���� ���� ���� -------
				_id = slotInfo.item.itemNum;
				_img.sprite = slotInfo.item.itemSpr;
				_amount = slotInfo.amount;

				fullSlot = true;
			}
			else // ������ 0�̶��
			{
				if (ItemDictionary.CheckData(_id)) // �ش� Id�� ��ųʸ��� �����ϸ�
				{
					ItemDictionary.itemDic.Remove(_id); // ������
				}
				Refresh();
			}
		}
		else
		{
			_id = 0;
			_img.sprite = defSpr; // ���� �̹��� �⺻ �̹����� ����
			_amount = 0;
		}

		// ������ ������ 0���� ũ�� ������ Ÿ���� �������� ��
		if (slotInfo.amount > 0 && slotInfo.item.isStackable)
		{
			amountTxt.text = _amount.ToString(); // ���� ǥ��
		}
		else
		{
			amountTxt.text = "";
		}
	}

	/// <summary>
	/// ���� û��
	/// </summary>
	public void Refresh()
	{
		slotInfo = new SlotInfo(); // ���� �ʱ�ȭ (������ = null, ���� = 0)

		_img.sprite = defSpr; // ���� �̹��� �⺻ �̹����� ����

		fullSlot = false;
	}

	/// <summary>
	/// ���� Init
	/// </summary>
	void SlotInit()
	{
		SlotSetting();

		slotBtn.onClick.AddListener(ItemUse);
	}

	/// <summary>
	/// ���� Update
	/// </summary>
	void SlotUpdate()
	{
		SlotSetting();

		if (fullSlot == false)
		{
			slotBtn.enabled = false;
		}
		else
		{
			slotBtn.enabled = true;
		}

		remainTime = slotInfo.remainTime;
		CoolTime(remainTime); // ���� ��Ÿ�� ���

		if (remainTime > 0)
		{
			remainTime -= Time.deltaTime;
			slotInfo.remainTime = remainTime;

			isCool = true;
		}
		else
		{
			remainTime = 0;
			slotInfo.remainTime = remainTime;

			isCool = false;
		}
	}

	/// <summary>
	/// ���� ���
	/// </summary>
	void ItemUse()
	{
		// ��Ÿ���� �ƴ� ��
		if (!isCool)
		{
			//_itemData.Use(); // �������̵�� �Լ�
			//slotInfo.item.Use();

			switch (slotInfo.item.itemType)
			{
				// ������ Ÿ���� �Ҹ��� �������� ��
				case ItemData.ItemType.Consumable:
					{
						ConsumableData consumableData = (ConsumableData)slotInfo.item;
						CoolTime(consumableData.coolTime); // �Ҹ�ǰ ��Ÿ�� �ð� ����

						if (consumableData.isEatable) // �ش� �Ҹ�ǰ�� ���� �� �ִٸ�
						{
							slotInfo.amount--;

							slotInfo.item.Use();
						}
					}
					break;

				// ������ Ÿ���� ������ �������� ��
				case ItemData.ItemType.Equipment:
					{
						slotInfo.item.Use();
					}
					break;
			}
		}
		else
		{
			return;
		}
	}

	/// <summary>
	/// �������̵�� ��Ÿ�� �Լ�
	/// </summary>
	/// <param name="coolTime"></param>
	public void CoolTime(float coolTime)
	{
		slotInfo.remainTime = coolTime;

		if (isCool == true)
		{
			Color c_item = _img.color;
			c_item.a = 0.5f;
			_img.color = c_item;
		}
		else
		{
			Color c_item = _img.color;
			c_item.a = 1;
			_img.color = c_item;
		}
	}

	// ����â�� ���� ����----------------------------------------------------------------------

	// ����â ����
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (slotBtn.enabled)
		{
			FollowWindow.instance.Data = slotInfo.item;
			FollowWindow.instance.ShowWindow();
		}
	}

	// ����â �����
	public void OnPointerExit(PointerEventData eventData)
	{
		if (slotBtn.enabled || _amount == 0)
		{
			FollowWindow.instance.CloseWindow();
		}
	}


}
