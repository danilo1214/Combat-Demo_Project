using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour {
	public AudioSource SoundEffects;
	public AudioSource SwimEffects;
	public AudioClip[] SlashSwordSounds;
	public AudioClip[] EquipWeaponSounds;
	public AudioClip[] JumpAndRollSounds;
	public AudioClip[] WaterSplashSounds;
	public AudioClip[] WaterSwimSounds;
	public AudioClip[] WaterExitSounds;

	void Start () {
		
	}
	
	void Update () {
		
	}
	void SlashSword(){
		SoundEffects.clip = SlashSwordSounds [Random.Range (0, SlashSwordSounds.Length)];
		PlaySound ();
	}
	void EquipWeapon(){
		SoundEffects.clip = EquipWeaponSounds [Random.Range (0, EquipWeaponSounds.Length)];
		PlaySound ();
	}
	void JumpAndRoll(){
		SoundEffects.clip = JumpAndRollSounds[Random.Range(0,JumpAndRollSounds.Length)];
		PlaySound ();
	}
	public void WaterSplash(){
		SoundEffects.clip = WaterSplashSounds [Random.Range (0, WaterSplashSounds.Length)];
		PlaySound ();
	}
	void WaterSwim(){
		SwimEffects.clip = WaterSwimSounds[Random.Range(0,WaterSwimSounds.Length)];
		SwimEffects.Stop ();
		SwimEffects.Play ();
	}
	public void WaterExit(){
		SoundEffects.clip = WaterExitSounds[Random.Range(0,WaterExitSounds.Length)];
		PlaySound ();
	}
	void PlaySound(){
		SoundEffects.Stop ();
		SoundEffects.Play ();
	}
}
