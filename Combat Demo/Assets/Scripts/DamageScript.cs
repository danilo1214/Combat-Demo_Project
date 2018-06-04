using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {
	public float Angle;

	public AudioSource BlockedSrc;
	public AudioSource GetHitSrc;
	public ParticleSystem playerBlood;
	void Start(){
		
	}
	void OnTriggerEnter(Collider other){
		if(other.GetComponent<TestScript>() && !TestScript.isSwimming){
			TestScript playerRef = other.GetComponent<TestScript> ();
			Stats plStats = other.GetComponent<TestScript> ().stats;
			Angle = Vector3.Angle (GetComponentInParent<EnemyScript>().transform.forward, -playerRef.transform.forward);
			if(Angle < 60.0f){
			if(!TestScript.is1Handed && !TestScript.is2Handed){
				playerRef.Equip1H ();
			}
			if (TestScript.isBlocking) {
					playerRef.anim.SetTrigger ("1HgetHit");
					BlockedSrc.Play ();
			}
			else if(!TestScript.isBlocking && TestScript.is1Handed){
					plStats.Health -= GetComponentInParent<EnemyScript> ().enStats.Damage;
					playerRef.anim.SetTrigger ("1HgetHit");
					GetHitSrc.Play ();
					playerBlood.Stop ();
					playerBlood.Play ();
			}
			if(TestScript.is2Handed && !TestScript.is1Handed){
					plStats.Health -= GetComponentInParent<EnemyScript> ().enStats.Damage;
					playerRef.anim.SetTrigger ("2HgetHit");
					GetHitSrc.Play ();
					playerBlood.Stop ();
					playerBlood.Play ();
			}
			}else{
				Debug.Log ("Big Angle");
				if(!TestScript.is1Handed && !TestScript.is2Handed){
					playerRef.Equip1H ();
				}
				if(TestScript.is1Handed){
					plStats.Health -= GetComponentInParent<EnemyScript>().enStats.Damage;
					playerRef.anim.SetTrigger ("1HgetHit");
					GetHitSrc.Play ();
					playerBlood.Stop ();
					playerBlood.Play ();
				}
				if(TestScript.is2Handed){
					plStats.Health -= GetComponentInParent<EnemyScript>().enStats.Damage;
					playerRef.anim.SetTrigger ("2HgetHit");
					GetHitSrc.Play ();
					playerBlood.Stop ();
					playerBlood.Play ();
				}

			}
	}
	}
		
}
