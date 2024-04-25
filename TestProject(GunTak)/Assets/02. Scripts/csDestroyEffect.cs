using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour 
{
	public float deadTime;
	void Start()
	{
		Destroy(gameObject, deadTime);
	}
}
