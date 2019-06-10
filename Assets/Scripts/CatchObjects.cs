using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchObjects : MonoBehaviour {

	void OnTriggerEnter(Collider col) 
	{
		if (col.tag == "Pillar") {
			col.gameObject.SetActive(false);
		}
	}
}
