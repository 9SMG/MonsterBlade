using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Create_ItemData/Others", order =  23)] // CreateAssetMenu(defult 파일명, 메뉴 이름, 순번)
public class OtherItemData : ItemData
{
	public enum OthersType
	{
		none = 0,
	}


	[Header("재료/기타 아이템")]
	[Space(20)]
	public OthersType othersType;
}
