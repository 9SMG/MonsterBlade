using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
/*
 * 2. 아이템 사용 테스팅
 */

public class GameManager : MonoBehaviour
{
	Transform status;

	Image img_Eq1;
	Image img_Eq2;

	[SerializeField] Sprite defSpr;

	Text txt_Eq1;
	Text txt_Eq2;

	Text txt_Hp;
	Text txt_Mp;
	Text txt_Sp;

	Text txt_Atk;
	Text txt_Def;

	Image img_Sick;
	[SerializeField] Sprite sickSpr;

	[Space(10)]
	[Header("기초 스탯")]
	[SerializeField] float maxHp; // 전체 체력
	[SerializeField] float maxMp;
	[SerializeField] float maxSp;

	[Space(10)]
	[SerializeField] float atk;   // 공격력
	[SerializeField] float def;   // 방어력

	[Space(10)]
	[Header("장비")]
	[Space(20)]
	// 무기
	[SerializeField] WeaponData eq1;
	public WeaponData Eq1
	{
		get { return eq1; }
		set { eq1 = value; }
	}

	// 방어구
	[SerializeField] ArmorData eq2;
	public ArmorData Eq2
	{
		get { return eq2; }
		set { eq2 = value; }
	}

	[Space(10)]
	[Header("장비 스탯")]
	[Space(20)]
	public float it_Hp;
	public float it_Atk;
	public float it_Def;

	float MaxHp;
	//float MaxMp;
	//float MaxSp;

	[Space(10)]
	[Header("플레이어 상태")]
	[Space(50)]
	public float hp = 20;
	public float mana = 100;
	public float stamina = 100;

	[Space(10)]
	public bool isSick;

	//-----------------------------------------------------------
	StatManager statManager;
	//-----------------------------------------------------------


	// Cheat ----------------------------------------------------
	bool isCheat = false;
	public GameObject itemBox;
	//-----------------------------------------------------------

	private void Awake()
	{
		// 스테이터스 레퍼런스 --------------------------------------------
		status = GameObject.FindGameObjectWithTag("Status").transform;

		txt_Hp = status.GetChild(1).GetComponent<Text>();
		txt_Mp = status.GetChild(2).GetComponent<Text>();
		txt_Sp = status.GetChild(3).GetComponent<Text>();

		txt_Atk = status.GetChild(4).GetComponent<Text>();
		txt_Def = status.GetChild(5).GetComponent<Text>();

		txt_Eq1 = status.GetChild(6).GetComponent<Text>();
		txt_Eq2 = status.GetChild(7).GetComponent<Text>();

		img_Eq1 = txt_Eq1.GetComponentInChildren<Image>();
		img_Eq2 = txt_Eq2.GetComponentInChildren<Image>();

		img_Sick = txt_Hp.GetComponentInChildren<Image>();

		//--------------------------------------------------------------
		statManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StatManager>();
	}

	private void Start()
	{
		//isSick = true;

		//if (isSick)
		//{
		//	isSick = ItemManager.instance.BuffCoolTime(0.5f, isSick);
		//}
	}

	private void Update()
	{
		ShowSetting();

		ChangeStatus(eq1, eq2);

		// 인벤토리가 켜지면 스탯 창도 켜짐
		status.gameObject.SetActive(InventoryManager.instance.isOpen);

		if (Input.GetKey(KeyCode.LeftShift))
		{
			// 아이템 소환 치트
			if (Input.GetKeyDown(KeyCode.F6) && !isCheat)
			{

				Debug.Log("치팅!");
				isCheat = true;

				ItemSpawnCheat();
			}

			// 디버프 치트
			if (Input.GetKeyDown(KeyCode.F7))
			{
				OnSick();
			}
		}


		// 스탯 갱신
		if (statManager.statInfo._curHP > MaxHp)
		{
			statManager.statInfo._curHP = MaxHp;
		}

		if (statManager.statInfo._curMP > maxMp)
		{
			statManager.statInfo._curMP = maxMp;
		}

		if (statManager.statInfo._curStamina > maxSp)
		{
			statManager.statInfo._curStamina = maxSp;
		}


	}

	///// <summary>
	///// 스테이터스 UI세팅
	///// </summary>
	//void ShowSetting()
	//{
	//	MaxHp = maxHp + it_Hp;

	//	txt_Hp.text = "HP: " + hp.ToString("0,0") + "/" + MaxHp.ToString("0,0");
	//	txt_Mp.text = "MP: " + mana.ToString("0,0") + "/" + maxMp.ToString("0,0");
	//	txt_Sp.text = "SP: " + stamina.ToString("0,0") + "/" + maxSp.ToString("0,0");

	//	txt_Atk.text = "ATK: " + (atk + it_Atk).ToString("0,0");
	//	txt_Def.text = "DEF: " + (def + it_Def).ToString("0,0");

	//	if (eq1 == null)
	//	{
	//		txt_Eq1.text = "무기: 없음";
	//		img_Eq1.sprite = defSpr;
	//	}
	//	else
	//	{
	//		txt_Eq1.text = "무기: " + eq1.itemName;
	//		img_Eq1.sprite = eq1.itemSpr;
	//	}

	//	if (eq2 == null)
	//	{
	//		txt_Eq2.text = "방어구: 없음";
	//		img_Eq2.sprite = defSpr;
	//	}

	//	else
	//	{
	//		txt_Eq2.text = "방어구: " + eq2.itemName;
	//		img_Eq2.sprite = eq2.itemSpr;
	//	}

	//	if (isSick)
	//	{
	//		img_Sick.sprite = sickSpr;
	//	}
	//	else
	//	{
	//		img_Sick.sprite = defSpr;
	//	}
	//}

	/// <summary>
	/// 스테이터스 UI세팅
	/// </summary>
	void ShowSetting()
	{
		MaxHp = statManager.statInfo.hpMax + statManager.statInfo._eqHP;

		txt_Hp.text = "HP: " + statManager.statInfo._curHP.ToString("0,0") + "/" + MaxHp.ToString("0,0");
		txt_Mp.text = "MP: " + statManager.statInfo._curMP.ToString("0,0") + "/" + statManager.statInfo.mpMax.ToString("0,0");
		txt_Sp.text = "SP: " + statManager.statInfo._curStamina.ToString("0,0") + "/" + statManager.statInfo.staminaMax.ToString("0,0");

		txt_Atk.text = "ATK: " + (statManager.statInfo.Attack + statManager.statInfo._eqAtk).ToString("0,0");
		txt_Def.text = "DEF: " + (statManager.statInfo.Defense + statManager.statInfo._eqDef).ToString("0,0");

		if (eq1 == null)
		{
			txt_Eq1.text = "무기: 없음";
			img_Eq1.sprite = defSpr;
		}
		else
		{
			txt_Eq1.text = "무기: " + eq1.itemName;
			img_Eq1.sprite = eq1.itemSpr;
		}

		if (eq2 == null)
		{
			txt_Eq2.text = "방어구: 없음";
			img_Eq2.sprite = defSpr;
		}

		else
		{
			txt_Eq2.text = "방어구: " + eq2.itemName;
			img_Eq2.sprite = eq2.itemSpr;
		}

		if (isSick)
		{
			img_Sick.sprite = sickSpr;
		}
		else
		{
			img_Sick.sprite = defSpr;
		}
	}

	// 장비 장착
	void ChangeStatus(WeaponData _wp, ArmorData _am)
	{
		if (_wp == null && _am == null)
		{
			statManager.statInfo._eqHP = 0;
			statManager.statInfo._eqAtk = 0;
			statManager.statInfo._eqDef = 0;
		}
		else if (_am == null)
		{
			statManager.statInfo._eqHP = 0;
			statManager.statInfo._eqAtk = _wp.Atk;
			statManager.statInfo._eqDef = 0;
		}
		else if (_wp == null)
		{
			statManager.statInfo._eqHP = _am.MaxHpPlus;
			statManager.statInfo._eqAtk = 0;
			statManager.statInfo._eqDef = _am.Def;
		}
		else
		{
			statManager.statInfo._eqHP = _am.MaxHpPlus;
			statManager.statInfo._eqAtk = _wp.Atk;
			statManager.statInfo._eqDef = _am.Def;
		}
	}

	// 무기 해제
	public void UneqWeapon()
	{
		if (eq1 != null)
			eq1 = null;
	}

	// 방어구 해제
	public void UneqArmor()
	{
		if (eq2 != null)
			eq2 = null;
	}


	// 디버프 테스트 ------------------------------------------------------
	public void OnSick()
	{
		if (!isSick)
		{
			isSick = true;
			StartCoroutine(SickCoroutine(Time.time));
		}
	}

	IEnumerator SickCoroutine(float _time)
	{
		while (Time.time - _time <= 10.0f)
		{
			if (!isSick)
			{
				break;
			}
			else
			{
				yield return new WaitForSeconds(1.0f);

				statManager.statInfo._curHP -= 1;
				if (statManager.statInfo._curHP < 0)
				{
					statManager.statInfo._curHP = 0;
				}
			}
		}
		isSick = false;
		yield return null;
	}

	// 아이템 치트 --------------------------------------------------
	void ItemSpawnCheat()
	{
		Vector3 spawnPos = new Vector3(100, 10, 340);

		itemBox.transform.position = spawnPos;

		Instantiate(itemBox);
	}

	///// <summary>
	///// 딕셔너리 테스팅
	///// </summary>
	//public void Test()
	//{
	//	foreach (var read in ItemDictionary.itemDic)
	//	{
	//		Debug.Log("Key: " + read.Key + ", Value: " + read.Value);
	//	}
	//}
}
