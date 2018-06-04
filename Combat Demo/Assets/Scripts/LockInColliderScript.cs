using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockInColliderScript : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}
	void OnTriggerEnter(Collider col){
		if(col.GetComponent<EnemyScript>() && GetComponentInParent<TestScript>().LockInTarget == null){
			EnemyScript enemyRef = col.GetComponent<EnemyScript> ();
			enemyRef.LockInEffect.SetActive (true);
			GetComponentInParent<TestScript> ().LockInTarget = enemyRef.transform;
		}
	}
}
