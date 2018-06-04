using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class TestScript : MonoBehaviour {
	
	public static bool is2Handed;
    public static bool is1Handed;
    public static bool isBlocking;
	public static bool isSwimming;
	public bool isLockedIn;

	public float Speed;
	public float OneHandedSpeed;
	public float UnarmedSpeed;
	public float SwimSpeed;
	public float TwoHandedSpeed;
	public float RollSpeed;
	public float JumpSpeed;
	public float SpeedWhileAttack;
	public float AttackSpeed;
	public float RotationSpeed;
	public float gravity;
	float RollSmooth;
	float RollTimer;
	float JumpTimer;
    float AttackTimer;
	float GravityConstant;

	public GameObject ArmPosOneHandedWeapon;
	public GameObject ArmPosTwoHandedWeapon;
	public GameObject ShieldPos;
	public GameObject Back;

	public GameObject OneHandedWeapon;
	public GameObject Shield;
	public GameObject TwoHandedWeapon;

	public Stats stats;
	public Animator anim;

	public CharacterController cctrl;
	public GameObject LockInCollider;
	public Transform LockInTarget;

	Vector3 MovePos;
	Vector3 RollDir;
	Quaternion newRot;
	Vector3 strafePos;
	Vector3 JumpDir;

	void Start () {
		OneHandedWeapon.transform.parent = Back.transform;
		OneHandedWeapon.transform.rotation = Back.transform.rotation;
		OneHandedWeapon.transform.position = Back.transform.position;

		TwoHandedWeapon.transform.parent = Back.transform;
		TwoHandedWeapon.transform.rotation = Back.transform.rotation;
		TwoHandedWeapon.transform.position = Back.transform.position;

		Shield.transform.parent = Back.transform;
		Shield.transform.rotation = Back.transform.rotation;
		Shield.transform.position = Back.transform.position;

		anim = GetComponent<Animator> ();
		cctrl = GetComponent<CharacterController> ();

		RollTimer = 0.6f;
		AttackTimer = AttackSpeed;
		GravityConstant = gravity;
	}

	void Update () {
		RollTimer += Time.deltaTime;
		AttackTimer += Time.deltaTime;
		JumpTimer += Time.deltaTime;

		CheckState ();
		Rotation ();
		Gravity ();
		

		if(RollTimer >= 0.6f && AttackTimer >= AttackSpeed && !isBlocking && !stats.isDead) {
			Movement();
			if(JumpTimer > 0.45f && JumpTimer < 0.8f){
				cctrl.Move (transform.rotation * JumpDir * JumpSpeed * Time.deltaTime);
			}
		}
		if(RollTimer > 0.02f && RollTimer < 0.6f){
			cctrl.Move (RollDir.normalized * Time.deltaTime * RollSpeed);
		}
		MovePos = transform.position;

		Die ();
	}

	void CheckState(){
		if(!is2Handed && !is1Handed && !isSwimming){
			if(Input.GetKey(KeyCode.E)){
				Equip1H ();
			}
			if(Input.GetKey(KeyCode.Q)){
				Equip2H ();
			}
		}
		if(is1Handed && !isSwimming){
			OneHanded ();
		}
		if (is2Handed && !isSwimming) {
			TwoHanded ();
		}
		if(!is1Handed && !is2Handed && !isSwimming){
			Unarmed();
		}
		if(isSwimming){
			Swimming ();
		}
	}
	public void Equip1H(){
		Speed = OneHandedSpeed;
		anim.SetTrigger ("Draw1H");
		is1Handed = true;
		is2Handed = false;

		OneHandedWeapon.transform.parent = ArmPosOneHandedWeapon.transform;
		OneHandedWeapon.transform.position = ArmPosOneHandedWeapon.transform.position;
		OneHandedWeapon.transform.rotation = ArmPosOneHandedWeapon.transform.rotation;

		Shield.transform.parent = ShieldPos.transform;
		Shield.transform.position = ShieldPos.transform.position;
		Shield.transform.rotation = ShieldPos.transform.rotation;

	}
	void Equip2H(){
		Speed = TwoHandedSpeed;
		anim.SetTrigger ("Draw2H");
		is2Handed = true;
		is1Handed = false;
		TwoHandedWeapon.transform.parent = ArmPosTwoHandedWeapon.transform;
		TwoHandedWeapon.transform.position = ArmPosTwoHandedWeapon.transform.position;
		TwoHandedWeapon.transform.rotation = ArmPosTwoHandedWeapon.transform.rotation;

	}
	void Holster1H(){
		if(Input.GetKeyDown(KeyCode.Z) && AttackTimer >= AttackSpeed){
			anim.SetTrigger ("Holster1H");
			HolsterAllWeapons ();
			is1Handed = false;
			is2Handed = false;
			Speed = UnarmedSpeed;
			if(isLockedIn){
			isLockedIn = false;
			LockInTarget.GetComponent<EnemyScript>().LockInEffect.SetActive (false);
			LockInTarget = null;
			}
		}
	}
	void Holster2H(){
		if(Input.GetKeyDown(KeyCode.Z) && AttackTimer >= AttackSpeed){
			anim.SetTrigger ("Holster2H");
			HolsterAllWeapons ();
			is2Handed = false;
			is1Handed = false;
			Speed = UnarmedSpeed;
			if(isLockedIn){
				isLockedIn = false;
				LockInTarget.GetComponent<EnemyScript>().LockInEffect.SetActive (false);
				LockInTarget = null;
			}
		}
	}
	void HolsterAllWeapons(){
		TwoHandedWeapon.transform.parent = Back.transform;
		TwoHandedWeapon.transform.rotation = Back.transform.rotation;
		TwoHandedWeapon.transform.position = Back.transform.position;
		OneHandedWeapon.transform.parent = Back.transform;
		OneHandedWeapon.transform.rotation = Back.transform.rotation;
		OneHandedWeapon.transform.position = Back.transform.position;
		Shield.transform.parent = Back.transform;
		Shield.transform.rotation = Back.transform.rotation;
		Shield.transform.position = Back.transform.position;
	}
		
	void Unarmed(){
		MovementAnimations ("Running");
		Jump("Jump");
		Roll ("Roll");
	}
	void OneHanded(){
		MovementAnimations ("1Hstrafing", "1Hrunning");
		Jump("1Hjump");
		Roll ("1Hroll");
		Block ("is1Hblocking");
		Holster1H ();
		LockIn ();

		if(RollTimer >= 0.6f && !isBlocking && !stats.isDead){
			WeaponEquipedMovement();
		}
			
		if(Input.GetKey(KeyCode.Mouse0) && AttackTimer >= AttackSpeed && RollTimer >=0.6f){
			anim.SetTrigger ("1Hhit");
			AttackTimer = 0f;
		}
	}
	void TwoHanded(){
		MovementAnimations ("2Hstrafing", "2Hwalking");
		Roll ("2Hroll");
		Jump("2Hjump");
		Holster2H ();
		LockIn ();

		if(RollTimer >= 0.6f && !stats.isDead){
		WeaponEquipedMovement ();
		}
		if(Input.GetKey(KeyCode.Mouse0) && AttackTimer >= AttackSpeed && RollTimer >=0.6f){
			anim.SetTrigger ("2Hhit");
			AttackTimer = 0f;
		}
	}
	void Swimming(){
		Rotation ();
		Movement ();
		MovementAnimations ("SwimMoving");
	}
	void WeaponEquipedMovement(){
		float x;
		float y;
		float z;
		x = 0f;
		y = 0f;
		z = 0f;
		if(is2Handed && !is1Handed){
			x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * TwoHandedSpeed;
			z = Input.GetAxisRaw ("Vertical") * Time.deltaTime * TwoHandedSpeed;
		}
		if(is1Handed && !is2Handed){
			x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * OneHandedSpeed;
			z = Input.GetAxisRaw("Vertical") * Time.deltaTime * OneHandedSpeed;
		}
		if(!is2Handed && !is1Handed){
		z = Input.GetAxisRaw("Vertical") * Time.deltaTime * Speed;
		}
		MovePos = new Vector3 (x,y,z);
		Vector3 WhereToMove;
		WhereToMove = Vector3.Lerp (transform.position,transform.rotation * MovePos, 1f);
		if (AttackTimer >= AttackSpeed) {
			cctrl.Move (WhereToMove);
		} else{
			cctrl.Move (WhereToMove * SpeedWhileAttack);
		}
	}
	void Movement(){
		float x;
		float y;
		float z;
		x = 0f;
		y = 0f;
		z = 0f;
		if(!is2Handed && !is1Handed){
			z = Input.GetAxisRaw("Vertical") * Time.deltaTime * Speed;
		}
		MovePos = new Vector3 (x,y,z);
		Vector3 WhereToMove;
		WhereToMove = Vector3.Lerp (transform.position,transform.rotation * MovePos, 1f);
		cctrl.Move (WhereToMove);
	}
	void Gravity(){
		if (!cctrl.isGrounded) {
			cctrl.Move (-transform.up * Time.deltaTime * gravity);
		}
	}
	void Rotation(){
		if(RollTimer > 0.6f && !isLockedIn){
			Vector3 camDir = Camera.main.transform.forward;
			camDir.y = 0.0f;
			Quaternion newRot = Quaternion.LookRotation (camDir);
			transform.rotation = Quaternion.Lerp (transform.rotation, newRot, 1000000000f);
		}

	}
	void LockIn(){
		if(Input.GetKeyDown(KeyCode.Tab)){
			if (LockInTarget == null) {
				LockInCollider.SetActive (true);
				StartCoroutine ("FindLockInTarget");
			} else {
				LockInTarget.GetComponent<EnemyScript> ().LockInEffect.SetActive (false);
				LockInTarget = null;
				isLockedIn = false;
			}
		}
		if(LockInTarget != null && RollTimer >= 0.6f){
			isLockedIn = true;
			Vector3 targetDir = LockInTarget.position - transform.position;
			targetDir.y = 0;
			Quaternion newRot = Quaternion.LookRotation (targetDir);
			transform.rotation = newRot;
		}

	}
	void Jump(string animationName){
		if(Input.GetKeyDown(KeyCode.Space) && JumpTimer>= 1.0f && RollTimer >= 0.6f && AttackTimer >= AttackSpeed){
			anim.SetTrigger (animationName);
			JumpDir = new Vector3(0f,1f,Input.GetAxisRaw("Vertical") * 0.4f);
			JumpTimer = 0f;
		}
	}
	void Block(string animationName){
		if (Input.GetKey (KeyCode.Mouse1)) {
			if (RollTimer >= 0.6f && JumpTimer >= 1.0f) {
				isBlocking = true;
				anim.SetBool (animationName, isBlocking);
			}
		} else {
			isBlocking = false;
			anim.SetBool (animationName,isBlocking);
		}
	}
	void Roll(string animationName){
		if(Input.GetKeyDown(KeyCode.R)  && RollTimer>= 0.6f && JumpTimer >= 1.0f && AttackTimer>=AttackSpeed){
			RollDir = new Vector3 (Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			RollDir.y = 0.0f;
			RollDir = transform.rotation * RollDir;
			newRot = Quaternion.LookRotation(RollDir);
			if(RollDir.magnitude != 0){
			transform.rotation = newRot;
			anim.SetTrigger (animationName);
			RollTimer = 0;
			}
		}
	}
	void MovementAnimations(string StrafeAnim, string RunAnim){
		if (Input.GetButton ("Horizontal")) {
			anim.SetBool (StrafeAnim,true);	
		}
		else {
			anim.SetBool (StrafeAnim,false);
		}
		if (Input.GetButton ("Vertical")) {
			anim.SetBool (RunAnim, true);
		} else {
			anim.SetBool (RunAnim,false);
		}
	}
	void MovementAnimations(string RunAnim){
		if (Input.GetButton ("Vertical")) {
			anim.SetBool (RunAnim, true);
		} else {
			anim.SetBool (RunAnim,false);
		}
	}
	void Die(){
		if(stats.Health <= 0 && !stats.isDead){
			Debug.Log ("I'm Dying");
			anim.SetBool ("isDead",true);
			anim.SetTrigger ("Dead");
		}
	}
	void StartDeath(){
		stats.isDead = true;
	}
	void DestroyGameObj(){
		Destroy(gameObject);
	}
	IEnumerator FindLockInTarget(){
		yield return new WaitForSeconds(0.3f);
		LockInCollider.SetActive (false);
	}
	void OnTriggerEnter(Collider col){
		if(col.tag == "Water"){
			isSwimming = true;
			is1Handed = false;
			is2Handed = false;
			isLockedIn = false;
			HolsterAllWeapons ();
			anim.SetTrigger ("EnterSwim");
			gravity = 0f;
			Speed = SwimSpeed;
			Camera.main.GetComponent<thirdpersonCamera> ().yMinLimit = 15;
			GetComponent<PlayerSoundManager> ().WaterSplash ();
			if(LockInTarget != null){
			LockInTarget.GetComponent<EnemyScript> ().LockInEffect.SetActive (false);
			LockInTarget = null;
			}
		}
	}
	void OnTriggerExit(Collider col){
		if(col.tag == "Water"){
			isSwimming = false;
			Speed = UnarmedSpeed;
			anim.SetTrigger ("ExitSwim");
			gravity = GravityConstant;
			Camera.main.GetComponent<thirdpersonCamera> ().yMinLimit = -20;
			GetComponent<PlayerSoundManager> ().WaterExit ();
		}
	}
}
