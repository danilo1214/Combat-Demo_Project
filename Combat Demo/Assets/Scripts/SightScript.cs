using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightScript : MonoBehaviour {

	EnemyScript enmAI;
	void Start () {
		enmAI = GetComponentInParent<EnemyScript> ();
	}

	void Update () {
		
	}
	void OnTriggerEnter(Collider other){
		if(enmAI.target == null){
			if(other.GetComponent<TestScript>()){
				enmAI.target = other.transform;
			}
		}
	}
}
