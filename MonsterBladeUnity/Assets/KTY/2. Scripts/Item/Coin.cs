using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int money = 1000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag("Player"))
		{
			Debug.Log("돈이다!");
			// 아이템 고정(컬리더 중복 작동으로 인한 수치 오류 방지를 위함)
			Collider coll = GetComponent<Collider>();
			coll.enabled = false;
			Rigidbody rig = GetComponent<Rigidbody>();
			rig.isKinematic = true;

			InventoryManager.instance.AddGold(money);
			Destroy(gameObject);
		}
	}
}
