using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stats{
	public float Health;
	public float Damage;
	public float Armour;
	public bool isDead;
	void Update(){
		if(Health <= 0){
			isDead = true;
		}
	}
}
