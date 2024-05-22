using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    DropCtrl dropCtrl;

	private void Awake()
	{
		dropCtrl = GetComponentInChildren<DropCtrl>();
	}

	// Start is called before the first frame update
	void Start()
    {
        //Die();
    }

    public void Die()
	{
        dropCtrl.DropItem();
		//gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
