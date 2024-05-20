using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SlotInfo
{
	// 아이템
	public ItemData item;

	// 수량
	public int amount;

	// 객체지향에서 무너질까? 무너지지 않는걸까?
	[HideInInspector] public float remainTime; // 남은 쿨타임
}


public class InventoryManager : MonoBehaviour, IComparer<SlotInfo>
{
	// 싱글톤
	public static InventoryManager instance;

	#region 변수 선언
	GameObject inven;
	Transform invenSlots; // 코드를 줄이기 위한 선언.

	Transform goldPnl; // 골드 패널
	Text goldTxt; // 골드 텍스트

	int gold = 0; // 보유 골드
	public int Gold
	{
		set { gold = value; }
		get { return gold; }
	}

	//List<InvenSlot> slots = new List<InvenSlot>(); // 슬롯 리스트

	const int SLOTSIZE = 24; // 최대 슬롯 사이즈
	InvenSlot[] slots = new InvenSlot[SLOTSIZE];



	//public List<SlotInfo> slotInfos = new List<SlotInfo>();


	//public List<ItemData> itemDatas = new List<ItemData>(); // 획득한 아이템 리스트

	//int prevListCount; // 이전 리스트 수

	bool isOpen = false; // 인벤토리 열림 여부

	public bool fullInven = false; // 인벤토리 꽉참 여부

	#endregion

	/// <summary>
	/// 인벤토리 용 계속 흐르는 시간
	/// </summary>
	[SerializeField] float invenTimer; // 시간 체크 용

	/// <summary>
	/// 비활성화된 시간
	/// </summary>
	[SerializeField] float disableTimer;

	private void Awake()
	{
		#region 싱글톤 화
		instance = this;
		#endregion

		inven = GameObject.FindGameObjectWithTag("Inventory");
		invenSlots = inven.transform.GetChild(1);

		goldPnl = inven.transform.GetChild(2);
		goldTxt = goldPnl.GetComponentInChildren<Text>();

		// 슬롯 조작을 위한 세팅
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

	// 인벤토리 여닫
	void OpenInventory()
	{
		isOpen = !isOpen;
		inven.SetActive(isOpen);
		FollowWindow.instance.CloseWindow();

		// 슬롯 쿨타임 계산
		if (!isOpen)
		{
			DisabledTime(); // 창을 닫은 시간 체크
		}
		else
		{
			EnabledTime(); // 창을 열고 남은 시간 계산
		}
	}

	// 초기화
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
	/// 아이템 획득 함수 (아이템 슬롯 정보)
	/// </summary>
	/// <param name="_item">보내는 아이템 정보</param>
	public void AddItem(SlotInfo _item)
	{
		ItemData _data = _item.item; // 문장 간략화를 위한 선언

		// 중복 체크
		if (!ItemDictionary.CheckData(_data.itemNum)) // 해당 key가 없다면
		{
			// 딕셔너리의 등록 --
			ItemDictionary.SetData(_data.itemNum, _data);

			FindEmptySlot(_item);
		}
		else
		{
			Debug.Log("중복 아이템 획득");

			if (_data.isStackable)
			{
				FindIncreaseSlot(_item);
			}
			else
			{
				FindEmptySlot(_item);
			}
		}

		#region *기존 리스트 방식
		///*
		// * <리스트 방식>
		// * 아이템 클래스에서 함수가 호출된다 (아이템 정보, 획득한 수량)
		// * 아이템 리스트에 넣는다. 해당 슬롯의 수량을 증가한다.
		// * 
		// * 아이템 클래스에서 함수가 호출된다 "
		// * 아이템 데이타를 보유 아이템 리스트와 비교한다.
		// * 중복된 데이타가 있다면 수량만 증가한다.
		// * 중복된 데이타가 없다면 아이템 리스트에 넣는다. 해당 슬롯의 수량을 증가한다.
		// */
		//
		//if (itemDatas.Count == 0) // 리스트에 아무것도 없을 때
		//{
		//	itemDatas.Add(_itemData);
		//	slots[0].Item = _itemData;
		//	slots[0].ItemImg.sprite = _itemData.itemSpr;
		//	slots[0].ItemAmount += amount;
		//}
		//else
		//{
		//	var checkData = _itemData; // 중복 체크용 변수
		//
		//	if (itemDatas.Contains(checkData) && checkData.isStackable) // 리스트 안에 중복되는 것이 있고 겹쳐지는 아이템일 때
		//	{
		//		for (int i = 0; i < itemDatas.Count; i++) // 중복을 찾는다.
		//		{
		//			if (checkData == itemDatas[i]) // 중복을 찾으면.
		//			{
		//				slots[i].ItemAmount += amount; // 해당 슬롯의 수량만 증가시켜라
		//				break;
		//			}
		//		}
		//	}
		//	else // 중복되는 것이 없거나 겹치는 아이템이 아닐 때
		//	{
		//		itemDatas.Add(_itemData); // 요소를 추가한다.
		//
		//		int emptyIdx = 0; // 비어있는 슬롯에 넣기 위함.
		//
		//		for (int j = 0; j < slots.Count; j++)
		//		{
		//			if (slots[j].Item == null) // 빈 슬롯을 발견하면 반복문을 나가라
		//			{
		//				emptyIdx = j;
		//				break;
		//			}
		//		}
		//
		//		slots[emptyIdx].Item = _itemData;
		//		slots[emptyIdx].ItemImg.sprite = _itemData.itemSpr;
		//		slots[emptyIdx].ItemAmount += amount; // 수량을 증가한다.
		//	}
		//}
		#endregion

		Debug.Log(_data.itemName + ", " + _item.amount + "개 획득");
	}

	/// <summary>
	/// 스택 시킬 슬롯 찾기
	/// </summary>
	/// <param name="_item"></param>
	void FindIncreaseSlot(SlotInfo _item)
	{
		// 슬롯의 수량 증가 --
		int findIdx = 0; // 찾는 인덱스 체크변수

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != _item.item) // 슬롯의 정보가 찾는 정보와 같지 않다면
			{
				findIdx++; // 인덱스를 증가
			}
			else // 찾았다면 반복문 탈출
			{
				Debug.Log("중복 발견: " + findIdx);
				break;
			}
		}

		slots[findIdx].slotInfo.amount += _item.amount; // 해당 슬롯의 수량 증가
	}

	/// <summary>
	/// 빈 슬롯 찾기
	/// </summary>
	/// <param name="_item"></param>
	void FindEmptySlot(SlotInfo _item)
	{
		// 슬롯의 등록 --
		int emptyIdx = 0; // 비어있는 인덱스 체크변수

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != null)
			{
				emptyIdx++; // 비어있지 않다면 인덱스 증가
			}
			else // 빈 칸 발견 시 반복문 탈출
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
	/// 돈 추가
	/// </summary>
	/// <param name="_money"></param>
	public void AddGold(int _money)
	{
		gold += _money;
	}

	/// <summary>
	/// 돈 빼기
	/// </summary>
	/// <param name="_money"></param>
	public void RemoveGold(int _money)
	{
		gold -= _money;
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// 비활성화 된 시간
	/// </summary>
	void DisabledTime()
	{
		disableTimer = invenTimer; // 창을 닫은 시간을 체크
	}
	/// <summary>
	/// 활성화시 시간 계산
	/// </summary>
	void EnabledTime()
	{
		// 비활성화 동안 흐른 시간 = 전체 시간 - 비활성화 된 시간
		float activatedTime = invenTimer - disableTimer;

		disableTimer = 0; // 비활성화 된 시간 초기화

		foreach (var slot in slots)
		{
			slot.slotInfo.remainTime -= activatedTime;
		}
	}

	//---------------------------------------------------------------------

	/// <summary>
	/// 아이템 정렬
	/// </summary>
	public void SortItem()
	{
		List<SlotInfo> infoList = new List<SlotInfo>(); // 보유 아이템 정보를 담을 리스트

		foreach (var slot in slots)
		{
			if (slot.slotInfo.item != null) // 아이템 정보가 있는 애들만 리스트에 넣기
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
	/// IComparer 인터페이스 정렬 함수
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public int Compare(SlotInfo x, SlotInfo y)
	{
		if (x.item != null && y.item != null)		// 빈 칸이 아닐 때
			return x.item.itemNum - y.item.itemNum;

		else if (x.item == null && y.item != null)	// 앞이 빈 칸일 때
			return -1;

		else if (x.item != null && y.item == null)	// 뒤가 빈 칸일 때
			return 1;

		else										// 둘 다 빈 칸일 때
			return 0;
	}
}
