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
			Debug.Log("���̴�!");
			// ������ ����(�ø��� �ߺ� �۵����� ���� ��ġ ���� ������ ����)
			Collider coll = GetComponent<Collider>();
			coll.enabled = false;
			Rigidbody rig = GetComponent<Rigidbody>();
			rig.isKinematic = true;

			InventoryManager.instance.AddGold(money);
			Destroy(gameObject);
		}
	}
}
