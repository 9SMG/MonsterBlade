using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // 마우스를 크로스 오버한 슬롯 내 아이템 정보창을 뜨게 하기 위함.

public class InvenSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	//ItemManager itemManager; // 싱글톤이 아닐 때 사용

	public SlotInfo slotInfo; // <- 슬롯 정보 ( 내용: 아이템 정보, 아이템 수량, 쿨타임 여부 )

	int _id; // 아이템 고유 넘버
	Image _img; // 보여질 아이템 이미지
	int _amount; // 보여질 아이템 수량

	// 프로퍼티 쓰려다가 망함
	//#region *아이템 데이터
	//// 아이템 데이터 ----------
	//ItemData _itemData;

	//public ItemData Item
	//{
	//	get { return _itemData; }
	//	set { _itemData = value; }
	//}
	//// ------------------------
	//#endregion

	//#region *아이템 ID
	//// 아이템 ID/Num ----------
	//int itemId;
	//public int ItemID
	//{
	//	get { return itemId; }
	//	set { itemId = value; }
	//}
	//#endregion

	//#region *아이템 이미지
	//Image itemImg;

	//public Image ItemImg
	//{
	//	get { return itemImg; }
	//	set { itemImg.sprite = value.sprite; }
	//}
	//#endregion

	//#region *아이템 수량
	//int itemAmount = 0; // 아이템 수량

	//public int ItemAmount
	//{
	//	get { return itemAmount; }
	//	set { itemAmount = value; }
	//}
	//#endregion

	[HideInInspector] public bool fullSlot = false; // 슬롯 채워짐 여부

	Text amountTxt; // 수량 텍스트
	Button slotBtn; // 슬롯 상호작용 버튼

	[Space(20)]
	[SerializeField] Sprite defSpr; // 아무것도 없을 때 디폴트 스프라이트

	[Space(20)]
	[SerializeField] bool isCool; // 사용템 쿨타임 적용
	[SerializeField] float remainTime = 0;

	private void Awake()
	{
		//itemImg = transform.GetChild(0).GetComponent<Image>();
		_img = transform.GetChild(0).GetComponent<Image>();
		amountTxt = transform.GetComponentInChildren<Text>();

		slotBtn = GetComponent<Button>();

		//itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>(); // 싱글톤 방식이 아닐 때
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
	/// 기본 세팅
	/// </summary>
	void SlotSetting()
	{
		if (slotInfo.item != null)
		{
			if (slotInfo.amount != 0) // 수량이 0이 아니라면
			{
				// 슬롯 변수에 아이템 슬롯 정보 세팅 -------
				_id = slotInfo.item.itemNum;
				_img.sprite = slotInfo.item.itemSpr;
				_amount = slotInfo.amount;

				fullSlot = true;
			}
			else // 수량이 0이라면
			{
				if (ItemDictionary.CheckData(_id)) // 해당 Id가 딕셔너리에 존재하면
				{
					ItemDictionary.itemDic.Remove(_id); // 삭제해
				}
				Refresh();
			}
		}
		else
		{
			_id = 0;
			_img.sprite = defSpr; // 슬롯 이미지 기본 이미지로 변경
			_amount = 0;
		}

		// 아이템 수량이 0보다 크고 데이터 타입이 스택형일 때
		if (slotInfo.amount > 0 && slotInfo.item.isStackable)
		{
			amountTxt.text = _amount.ToString(); // 수량 표시
		}
		else
		{
			amountTxt.text = "";
		}
	}

	/// <summary>
	/// 슬롯 청소
	/// </summary>
	public void Refresh()
	{
		slotInfo = new SlotInfo(); // 슬롯 초기화 (아이템 = null, 수량 = 0)

		_img.sprite = defSpr; // 슬롯 이미지 기본 이미지로 변경

		fullSlot = false;
	}

	/// <summary>
	/// 슬롯 Init
	/// </summary>
	void SlotInit()
	{
		SlotSetting();

		slotBtn.onClick.AddListener(ItemUse);
	}

	/// <summary>
	/// 슬롯 Update
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
		CoolTime(remainTime); // 남은 쿨타임 계승

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
	/// 슬롯 사용
	/// </summary>
	void ItemUse()
	{
		// 쿨타임이 아닐 때
		if (!isCool)
		{
			//_itemData.Use(); // 오버라이드된 함수
			//slotInfo.item.Use();

			switch (slotInfo.item.itemType)
			{
				// 아이템 타입이 소모형 아이템일 때
				case ItemData.ItemType.Consumable:
					{
						ConsumableData consumableData = (ConsumableData)slotInfo.item;
						CoolTime(consumableData.coolTime); // 소모품 쿨타임 시간 적용

						if (consumableData.isEatable) // 해당 소모품이 먹을 수 있다면
						{
							slotInfo.amount--;

							slotInfo.item.Use();
						}
					}
					break;

				// 아이템 타입이 장착형 아이템일 때
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
	/// 오버라이드된 쿨타임 함수
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

	// 설명창을 위한 구현----------------------------------------------------------------------

	// 설명창 띄우기
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (slotBtn.enabled)
		{
			FollowWindow.instance.Data = slotInfo.item;
			FollowWindow.instance.ShowWindow();
		}
	}

	// 설명창 숨기기
	public void OnPointerExit(PointerEventData eventData)
	{
		if (slotBtn.enabled || _amount == 0)
		{
			FollowWindow.instance.CloseWindow();
		}
	}


}
