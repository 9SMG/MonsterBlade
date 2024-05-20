using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary
{
	/// <summary>
	/// 아이템 딕셔너리 = 보유한 아이템 번호를 키로 저장
	/// </summary>
	public static Dictionary<int, ItemData> itemDic = new Dictionary<int, ItemData>();

	/// <summary>
	/// 데이터 추가
	/// </summary>
	/// <param name="_id">키값</param>
	/// <param name="_item">아이템 데이터</param>
	public static void SetData(int _id, ItemData _item)
	{
		itemDic.Add(_id, _item);
	}

	/// <summary>
	/// 데이터 검색
	/// </summary>
	/// <param name="_id">키값</param>
	/// <returns></returns>
	public static bool CheckData(int _id)
	{
		if (itemDic.ContainsKey(_id))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	///// <summary>
	///// 데이터 삭제
	///// </summary>
	///// <param name="_num"></param>
	//public void RemoveData(int _num)
	//{
	//	itemDic.Remove(_num);
	//}
}
