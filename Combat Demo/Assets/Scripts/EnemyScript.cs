using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
	public float AttackRange =  5f;
	public float AttackRate = 4f;
	public float Curve;
	public float CombatRange;
	public float StopRotatingCurve = 0.13f;
	public float StartAttackCurve = 0.7f;
	public float StopAttackCurve = 1.2f;
	public bool isInCombat;
    public bool currentlyAttacking;
	public bool isDead;
	public Stats enStats;
	public GameObject DamageColider;
	public GameObject LockInEffect;
	Animator anm;
	NavMeshAgent agent;
	Rigidbody rigid;
	public Transform target;
	float attTimer;
	bool attackOnce;
	bool stopRotating;

	void Start () {
		anm = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		rigid = GetComponent<Rigidbody> ();
		agent.stoppingDistance = AttackRange;
	}

	void Update () {
		if (enStats.Health >= 0) {
			if (!currentlyAttacking) {
				MovementHandler ();
			} 
			AttackHandler ();
		} 
		else {
			Die ();
		}
	}

	void MovementHandler(){
		if(target != null){
			agent.SetDestination(target.position);
			Vector3 relDir = transform.InverseTransformDirection(agent.desiredVelocity);
			anm.SetFloat ("Movement",relDir.z,0.5f,Time.deltaTime);
			float distance = Vector3.Distance (transform.position,target.position);
			if(distance <= AttackRange){
				Vector3 dir = target.position - transform.position;
				dir.y = 0;
				Quaternion targetRot = Quaternion.LookRotation (dir);
				transform.rotation = targetRot;
				attTimer += Time.deltaTime;
				if(attTimer > AttackRate){
					currentlyAttacking = true;
					attTimer = 0;
				}
			}
		}
	}
	void AttackHandler(){
		if (currentlyAttacking) {
			Curve += Time.deltaTime;
			if (!stopRotating) {
				Vector3 dir = target.position - transform.position;
				Quaternion targetRot = Quaternion.LookRotation (dir);
				transform.rotation = targetRot;
				float Angle = Vector3.Angle (transform.forward, dir);
				if (Angle < 5) {
					if (!attackOnce) {
						agent.Stop ();
						anm.SetBool ("Attack", true);
						attackOnce = true;
					}
				}
			}
		

			if (Curve > StopRotatingCurve) {
				stopRotating = true;
			}
			if(Curve > StopAttackCurve){
				closeAttack ();
			}
			if (Curve > StartAttackCurve) {
				DamageColider.SetActive (true);
			}
			else {
				if (DamageColider.activeInHierarchy) {
					DamageColider.SetActive (false);
				}
			}
		}
	}

	void closeAttack(){
		anm.SetBool ("Attack",false);
		attackOnce = false;
		agent.Resume ();
		currentlyAttacking = false;
		stopRotating = false;
		Curve = 0;
	}
	void Die(){
		if(!isDead){
			anm.SetTrigger("Die");
			LockInEffect.SetActive (false);
			isDead = true;
		}
	}

}
