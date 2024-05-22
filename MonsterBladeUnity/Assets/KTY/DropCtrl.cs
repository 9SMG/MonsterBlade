using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCtrl : MonoBehaviour
{
	[Header("아이템 드랍 개수")]
	public int dropCount = 1;

	[Header("아이템 드랍 리스트")]
	public List<GameObject> dropList = new List<GameObject>();

	/// <summary>
	/// 드랍 아이템
	/// </summary>
    public void DropItem()
	{
		int count = 0;

		if (dropCount > 0)
		{
			while (count < dropCount)
			{
				int idx = Random.Range(0, dropList.Count - 1);

				GameObject go = Instantiate(dropList[idx], transform);
				go.transform.parent = null;

				count++;
			}
		}
	}
}
