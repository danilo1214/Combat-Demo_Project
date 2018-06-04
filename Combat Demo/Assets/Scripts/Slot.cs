using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour {
	public Item SlotItem;
	public Image SlotIcon;
	public Text ItemName;
	public Text ItemDamage;

	void Update(){
		SlotIcon.sprite = SlotItem.Icon;
		ItemDamage.text = SlotItem.DamageValue.ToString();
		ItemName.text = SlotItem.ID.ToString();
	}
}
