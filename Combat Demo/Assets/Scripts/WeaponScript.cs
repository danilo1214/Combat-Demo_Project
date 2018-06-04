using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {
	bool isAttacking;
	public Collider OneHadnedWeaponCol;
	public Collider TwoHadnedWeaponCol;
	public Collider weaponCol;
	TestScript playerReference;
	void Start () {
		playerReference = GameObject.FindGameObjectWithTag ("Player").GetComponent<TestScript> ();
	}
	
	void Update () {
		
	}
	void OnTriggerEnter(Collider weaponCol){
		if(isAttacking){
			if(weaponCol.tag == "Enemy"){
				EnemyScript enemyTarget = weaponCol.GetComponent<EnemyScript> ();
				if(!enemyTarget.isDead){
				if(!enemyTarget.currentlyAttacking){
				enemyTarget.GetComponent<Animator> ().SetTrigger ("GetHit");
				}
				enemyTarget.GetComponentInChildren<ParticleSystem> ().Stop ();
				enemyTarget.GetComponentInChildren<ParticleSystem> ().Play ();
				enemyTarget.GetComponent<AudioSource> ().Stop ();
				enemyTarget.GetComponent<AudioSource> ().Play ();
				enemyTarget.enStats.Health -= playerReference.stats.Damage;
				}
			}
		}
	}
	void Attack(){
		if(TestScript.is1Handed && !TestScript.is2Handed){
			weaponCol = OneHadnedWeaponCol;
		}
		if(!TestScript.is1Handed && TestScript.is2Handed){
			weaponCol = TwoHadnedWeaponCol;
		}
		isAttacking = true;
	}
	void StopAttack(){
		isAttacking = false;
	}
}
