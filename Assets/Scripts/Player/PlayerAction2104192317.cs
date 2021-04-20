using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAction2 : MonoBehaviour
{
	[SerializeField]
	private Camera characterCamera;
	[SerializeField]
	private Transform ItemPickPosition;
	
	// Declare item on hand and make sure it can be picked
	private PickableItem pickedItem;
	private bool isTable;
	private bool isBin;
	private bool isDispenser;
	private bool isMaking;
	private GameObject colObj;
	private GameObject itemOnTable;
	
	
	
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		// Pick and Drop Item
		if (Input.GetKeyDown(KeyCode.Space))  {
			Debug.Log("7");
			
			if (isTable == true){
				if(isMaking == true){
					colObj.gameObject.GetComponent<ToolTable>().resetIfLeave();
					isTable = false;
				}
				if (pickedItem)
				{	
					Debug.Log("8");
					GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
					DropOnTable(pickedItem, ItemSlot.transform);
					
					//TODO change to update table condition func
					if(colObj.GetComponent<ToolTable>()){
						colObj.GetComponent<ToolTable>().ItemOnCounter = true;
					}
					else if(colObj.GetComponent<EmptyTable>()){
						colObj.GetComponent<EmptyTable>().ItemOnCounter = true;
					}
				}	
				else
				{
					Debug.Log("10");
					var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 3f))
					{
						var pickable = hit.transform.GetComponent<PickableItem>();
						if (pickable)
						{
							PickItem(pickable);
							if(colObj.GetComponent<ToolTable>()){
								colObj.GetComponent<ToolTable>().ItemOnCounter = false;
							}
							else if(colObj.GetComponent<EmptyTable>()){
								colObj.GetComponent<EmptyTable>().ItemOnCounter = false;
							}
							Debug.Log("Pick");
						}
					}
					
				}
			}else if (isDispenser == true) {
				if (pickedItem){
					if (colObj.GetComponent<DispenseTable>().ItemOnCounter == true){
						// TODO check combinable anot
						DropItem(pickedItem);
					}else{
						
						GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
						DropOnTable(pickedItem, ItemSlot.transform);
						colObj.GetComponent<DispenseTable>().ItemOnCounter = true;
					}
				}else{
					if (colObj.GetComponent<DispenseTable>().ItemOnCounter == true){
						var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
						RaycastHit hit;
						if (Physics.Raycast(ray, out hit, 3f))
						{
							var pickable = hit.transform.GetComponent<PickableItem>();
							if (pickable)
							{
								PickItem(pickable);
								colObj.GetComponent<DispenseTable>().ItemOnCounter = false;
								Debug.Log("Pick");
							}
						}
					}else{
						Debug.Log("12");
						var item = colObj.GetComponent<DispenseTable>().ItemToDispense;
						var pickable = colObj.GetComponent<DispenseTable>().ItemToDispense.GetComponent<PickableItem>();
						Debug.Log("13");
						if (pickable)
						{
							SpawnItem(item, pickable);
							Debug.Log("14");
							colObj.GetComponent<DispenseTable>().ItemOnCounter = false;
							Debug.Log("Pick");
						}
					}
					
				}
			}
			else{
				if (pickedItem)  
				{	
					if (isBin == true){
						pickedItem.Destroy();
						pickedItem = null;
						Debug.Log("11");
					}else{
						Debug.Log("9");
						DropItem(pickedItem);
					}
				}	
				else
				{
					Debug.Log("10");
					var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 3f))
					{
						var pickable = hit.transform.GetComponent<PickableItem>();
						if (pickable)
						{
							PickItem(pickable);
							Debug.Log("Pick");
						}
					}

					
				}
			}
		}
		
		
		if (Input.GetKeyDown(KeyCode.LeftShift) && colObj.CompareTag("Table"))  {
			// check counter condition and item condition
			// if possible only proceed own progress [start stop/doing] based on tag
			//  TODO
			if(colObj.GetComponent<ToolTable>()){
				colObj.GetComponent<ToolTable>().makeProduct();
				isMaking = true;
			}else{
				Debug.Log("Continue with other table.");
			}

			
		}
	}
	
	
	void OnTriggerEnter(Collider col) 
	{
		Debug.Log("Col");
		
		colObj = col.gameObject;
		Debug.Log(colObj.name);
		if(colObj.tag == "Table"){
			isTable = true;
		}else if(colObj.tag == "Bin"){
			isBin = true;
		}else if(colObj.tag == "Dispenser"){
			isDispenser = true;
		}	
		
		
	}
	
	void OnTriggerExit(Collider col){
		Debug.Log("Exit");
		if(colObj.tag == "Table"){
			isTable = false;
		}else if(colObj.tag == "Bin"){
			isBin = false;
		}else if(colObj.tag == "Dispenser"){
			isDispenser = false;
		}else if(colObj.tag == "Table" && isMaking == true){
			// colObj.GetComponent<ToolTable>().resetIfLeave();
			findColTableType().resetIfLeave();
			isTable = false;
		}
	}
	
	
	#region private func
	
	// Spawn item from item dispenser
	private void SpawnItem(GameObject prefab,PickableItem item)
	{
		Instantiate(prefab, new Vector3(ItemPickPosition.transform.position.x, ItemPickPosition.transform.position.y, ItemPickPosition.transform.position.z), Quaternion.identity);
		var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 3f))
		{
			var pickable = hit.transform.GetComponent<PickableItem>();
			if (pickable)
			{
				PickItem(pickable);
			}

		}
	}
	
	// Pick item from floor/table
	private void PickItem(PickableItem item)
	{
		pickedItem = item;
		// Disable rigidbody and reset velocities
		item.Rb.isKinematic = true;
		item.Rb.velocity = Vector3.zero;
		item.Rb.angularVelocity = Vector3.zero;
		// Set Slot as a parent
		item.transform.SetParent(ItemPickPosition);
		// Reset position and rotation
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
	}

	// Drop item on floor
	private void DropItem(PickableItem item)
	{
		// Remove reference
		pickedItem = null;
		// Remove parent
		item.transform.SetParent(null);
		// Enable rigidbody
		item.Rb.isKinematic = false;
		// Add force to throw item a little bit
		item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
	}
	
	// Drop item on table when table is empty
	private void DropOnTable(PickableItem item, Transform itemSlotOnTable)
	{
		// If combinable
		pickedItem = null;
		// Set Slot as a parent
		item.transform.SetParent(itemSlotOnTable);
		// Reset position and rotation
		item.transform.localPosition = Vector3.zero;
		item.transform.localEulerAngles = Vector3.zero;
	}

	// Check where 2 item / item + container can be combinable or not, incomplete product cannot combine with container
	private void CheckCombinable()
	{
		itemOnTable = colObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
		
		if(CheckItemType(pickedItem.gameObject) == CheckItemType(itemOnTable)){
			string itemType = CheckItemType(pickedItem.gameObject);
			if (itemType == "Engine"){
				if(pickedItem.gameObject.GetComponent<Engine>().spoiled == false && itemOnTable.GetComponent<Engine>().spoiled == false){
					List<string> MatchedItem = pickedItem.gameObject.GetComponent<Engine>().inContainer.Intersect(itemOnTable.GetComponent<Engine>().allowedItems).ToList();
					if(MatchedItem.Count() == 1){
						foreach (string item in MatchedItem){
							pickedItem.Destroy();
							pickedItem = null;
							itemOnTable.GetComponent<Engine>().inContainer.Add(item);
							itemOnTable.GetComponent<Engine>().allowedItems.Remove(item);
							for(int i=0; i< itemOnTable.transform.childCount; i++)
							{
								var child = itemOnTable.transform.GetChild(i).gameObject;
								if(child != null)
								child.SetActive(false);
							}
							itemOnTable.transform.Find(item).gameObject.SetActive(true);
						}
					}else{
						Debug.Log("MatchedItem.Count >= 1");
					}
				}else{
					//TODO error sound effect
				}
			}else if (itemType == "Propeller"){
				if(pickedItem.gameObject.GetComponent<Propeller>().spoiled == false && itemOnTable.GetComponent<Propeller>().spoiled == false){
					List<string> MatchedItem = pickedItem.gameObject.GetComponent<Propeller>().inContainer.Intersect(itemOnTable.GetComponent<Propeller>().allowedItems).ToList();
					if(MatchedItem.Count() == 1){
						foreach (string item in MatchedItem){
							pickedItem.Destroy();
							pickedItem = null;
							itemOnTable.GetComponent<Propeller>().inContainer.Add(item);
							itemOnTable.GetComponent<Propeller>().allowedItems.Remove(item);
							for(int i=0; i< itemOnTable.transform.childCount; i++)
							{
								var child = itemOnTable.transform.GetChild(i).gameObject;
								if(child != null)
								child.SetActive(false);
							}
							itemOnTable.transform.Find(item).gameObject.SetActive(true);
						}
					}else{
						Debug.Log("MatchedItem.Count >= 1");
					}
				}else{
					//TODO error sound effect
				}
			}else if (itemType == "SphereProjector"){
				if(pickedItem.gameObject.GetComponent<SphereProjector>().spoiled == false && itemOnTable.GetComponent<SphereProjector>().spoiled == false){
					List<string> MatchedItem = pickedItem.gameObject.GetComponent<SphereProjector>().inContainer.Intersect(itemOnTable.GetComponent<SphereProjector>().allowedItems).ToList();
					if(MatchedItem.Count() == 1){
						foreach (string item in MatchedItem){
							pickedItem.Destroy();
							pickedItem = null;
							itemOnTable.GetComponent<SphereProjector>().inContainer.Add(item);
							itemOnTable.GetComponent<SphereProjector>().allowedItems.Remove(item);
							for(int i=0; i< itemOnTable.transform.childCount; i++)
							{
								var child = itemOnTable.transform.GetChild(i).gameObject;
								if(child != null)
								child.SetActive(false);
							}
							itemOnTable.transform.Find(item).gameObject.SetActive(true);
						}
					}else{
						Debug.Log("MatchedItem.Count >= 1");
					}
				}else{
					//TODO error sound effect
				}
			}else{
				Debug.Log("CheckCombinable(): Error getting itemType");
			}
			

		}else if(pickedItem.gameObject.CompareTag("Crate") && IsItem(itemOnTable) == true){
			if (CheckItemType(itemOnTable) == "Engine"){
				if(itemOnTable.GetComponent<Engine>().spoiled == false && itemOnTable.GetComponent<Engine>().inCrate == false && itemOnTable.GetComponent<Engine>().completed == true){
					pickedItem.Destroy();
					pickedItem = null;
					itemOnTable.GetComponent<Engine>().inContainer.Add("Crate");
					itemOnTable.GetComponent<Engine>().allowedItems.Remove("Crate");
					itemOnTable.GetComponent<Engine>().inCrate = true;
					itemOnTable.transform.Find("Crate").gameObject.SetActive(true);
					
				}else{
					//TODO error sound effect
				}
			}else if (CheckItemType(itemOnTable) == "Propeller"){
				if(itemOnTable.GetComponent<Propeller>().spoiled == false && itemOnTable.GetComponent<Propeller>().inCrate == false && itemOnTable.GetComponent<Propeller>().completed == true){
					pickedItem.Destroy();
					pickedItem = null;
					itemOnTable.GetComponent<Propeller>().inContainer.Add("Crate");
					itemOnTable.GetComponent<Propeller>().allowedItems.Remove("Crate");
					itemOnTable.GetComponent<Propeller>().inCrate = true;
					itemOnTable.transform.Find("Crate").gameObject.SetActive(true);
					
				}else{
					//TODO error sound effect
				}
			}else if (CheckItemType(itemOnTable) == "SphereProjector"){
				if(itemOnTable.GetComponent<SphereProjector>().spoiled == false && itemOnTable.GetComponent<SphereProjector>().inCrate == false && itemOnTable.GetComponent<SphereProjector>().completed == true){
					pickedItem.Destroy();
					pickedItem = null;
					itemOnTable.GetComponent<SphereProjector>().inContainer.Add("Crate");
					itemOnTable.GetComponent<SphereProjector>().allowedItems.Remove("Crate");
					itemOnTable.GetComponent<SphereProjector>().inCrate = true;
					itemOnTable.transform.Find("Crate").gameObject.SetActive(true);
					
				}else{
					//TODO error sound effect
				}
			}else{
				Debug.Log("CheckCombinable(): Error getting itemType");
			}
			

		}else if(itemOnTable.CompareTag("Crate") && IsItem(pickedItem.gameObject) == true){
			if (CheckItemType(pickedItem.gameObject) == "Engine"){
				if(pickedItem.gameObject.GetComponent<Engine>().spoiled == false && pickedItem.gameObject.GetComponent<Engine>().inCrate == false && pickedItem.gameObject.GetComponent<Engine>().completed == true){
					itemOnTable.GetComponent<PickableItem>().Destroy();
					pickedItem.gameObject.GetComponent<Engine>().inContainer.Add("Crate");
					pickedItem.gameObject.GetComponent<Engine>().allowedItems.Remove("Crate");
					pickedItem.gameObject.GetComponent<Engine>().inCrate = true;
					pickedItem.gameObject.transform.Find("Crate").gameObject.SetActive(true);
					GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
					DropOnTable(pickedItem, ItemSlot.transform);
				}else{
					//TODO error sound effect
				}
			}else if (CheckItemType(pickedItem.gameObject) == "Propeller"){
				if(pickedItem.gameObject.GetComponent<Propeller>().spoiled == false && pickedItem.gameObject.GetComponent<Propeller>().inCrate == false && pickedItem.gameObject.GetComponent<Propeller>().completed == true){
					itemOnTable.GetComponent<PickableItem>().Destroy();
					pickedItem.gameObject.GetComponent<Propeller>().inContainer.Add("Crate");
					pickedItem.gameObject.GetComponent<Propeller>().allowedItems.Remove("Crate");
					pickedItem.gameObject.GetComponent<Propeller>().inCrate = true;
					pickedItem.gameObject.transform.Find("Crate").gameObject.SetActive(true);
					GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
					DropOnTable(pickedItem, ItemSlot.transform);
				}else{
					//TODO error sound effect
				}
			}else if (CheckItemType(pickedItem.gameObject) == "SphereProjector"){
				if(pickedItem.gameObject.GetComponent<SphereProjector>().spoiled == false && pickedItem.gameObject.GetComponent<SphereProjector>().inCrate == false && pickedItem.gameObject.GetComponent<SphereProjector>().completed == true){
					itemOnTable.GetComponent<PickableItem>().Destroy();
					pickedItem.gameObject.GetComponent<SphereProjector>().inContainer.Add("Crate");
					pickedItem.gameObject.GetComponent<SphereProjector>().allowedItems.Remove("Crate");
					pickedItem.gameObject.GetComponent<SphereProjector>().inCrate = true;
					pickedItem.gameObject.transform.Find("Crate").gameObject.SetActive(true);
					GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
					DropOnTable(pickedItem, ItemSlot.transform);
				}else{
					//TODO error sound effect
				}
			}else{
				Debug.Log("CheckCombinable(): Error getting itemType");
			}
			
		}else{
			Debug.Log("CheckCombinable(): Error");
		}
	}
	
	//TODO del if able to replace by findColTableType
	private string CheckItemType(GameObject item)
	{
		if(item.GetComponent<Engine>()){
			return "Engine";
		}else if(item.GetComponent<Propeller>()){
			return "Propeller";
		}else if(item.GetComponent<SphereProjector>()){
			return "SphereProjector";
		}else{
			Debug.Log("CheckItemType(): Error");
			return "Error";
		}
	}
	
	// Make sure item on hand is not a container
	private bool IsItem(GameObject item)
	{
		if(item.GetComponent<Engine>() || item.GetComponent<Propeller>()|| item.GetComponent<SphereProjector>()){
			return true;
		}else{
			return false;
		}
	}
	
	private ToolTable findColTableType()
	{
		if (colObj.GetComponent<WrenchTable>()){
			return colObj.GetComponent<WrenchTable>();
		}else if(colObj.GetComponent<SprayTable>()){
			return colObj.GetComponent<SprayTable>();
		}else if(colObj.GetComponent<HammerTable>()){
			return colObj.GetComponent<HammerTable>();
		}else if(colObj.GetComponent<DrillerTable>()){
			return colObj.GetComponent<DrillerTable>();
		}else if(colObj.GetComponent<ScrewDriverTable>()){
			return colObj.GetComponent<ScrewDriverTable>();
		}else if(colObj.GetComponent<ClothTable>()){
			return colObj.GetComponent<ClothTable>();
		}else{
			Debug.Log("findColTableType(): Error");
			return  colObj.GetComponent<ToolTable>();
		}
	}

	private Product findColProductType()
	{	
		if (colObj.GetComponent<Engine>()){
			return colObj.GetComponent<Engine>();
		}else if(colObj.GetComponent<SphereProjector>()){
			return colObj.GetComponent<SphereProjector>();
		}else if(colObj.GetComponent<Propeller>()){
			return colObj.GetComponent<Propeller>();
		}else{
			Debug.Log("findColProductType(): Error");
			return colObj.GetComponent<Product>();
		}
	}
	#endregion
}
