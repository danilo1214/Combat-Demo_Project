using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	public ItemDatabase dbRef; 
	public Transform InventoryPanel;
	public List<Item> items = new List<Item> ();
	public GameObject SlotPrefab;
	public float HorMargin = 10f;
	float VerMargin = 100f;
	void Start(){
		dbRef = GameObject.FindGameObjectWithTag ("itemDatabase").GetComponent<ItemDatabase> (); 
	}
	void Update(){
		if(Input.GetKeyDown(KeyCode.T)){
			AddItem (Random.Range(1,4));
		}
	}
	void AddItem(int ID){
		GameObject addedObj = GameObject.Instantiate (SlotPrefab, InventoryPanel);
		addedObj.GetComponent<RectTransform>().localPosition = new Vector3 (HorMargin,InventoryPanel.GetComponent<RectTransform>().position.y - VerMargin - items.Count* 100);
		Slot addedSlot = addedObj.GetComponent<Slot> ();
		items.Add (dbRef.itemDatabase[ID]);
		addedSlot.SlotItem = dbRef.itemDatabase [ID];
	}


}
