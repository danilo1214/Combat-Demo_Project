using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {
	public float DayNightCycleSpeed;
	void Start () {
		
	}
	
	void Update () {


		transform.Rotate(Vector3.right * DayNightCycleSpeed * Time.deltaTime);
	}
}
