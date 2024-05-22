using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public TestEnemy testEnemy;

	private void Update()
	{
		if (testEnemy != null)
		{
			if (Input.GetMouseButtonDown(0))
			{
				testEnemy.Die();
			}
		}
	}

}
