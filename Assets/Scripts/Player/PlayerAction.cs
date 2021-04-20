using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAction : MonoBehaviour
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
			
			// If stg is on hand
			if(pickedItem){
				if(colObj.CompareTag("Bin")){
					pickedItem.Destroy();
					pickedItem = null;
				}else if(colObj.CompareTag("Counter")){
					if (!findProductType(pickedItem.gameObject) == pickedItem.gameObject.GetComponent<Product>() && findProductType(pickedItem.gameObject).completed == true && findProductType(pickedItem.gameObject).inCrate == true){
						//pass game object type to Game Manager
					}
					pickedItem.Destroy();
				}else if(colObj.CompareTag("Table")){
					
					GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
					
					if(colObj.GetComponent<EmptyTable>()){
						
						if(colObj.GetComponent<EmptyTable>().ItemOnCounter == true){
							CheckCombinable();
						}else{
							DropOnTable(pickedItem, ItemSlot.transform);
							colObj.GetComponent<EmptyTable>().ItemOnCounter = true;
						}
					}else if(colObj.GetComponent<DispenseTable>()){
						
						if(colObj.GetComponent<DispenseTable>().ItemOnCounter == true){
							CheckCombinable();
						}else{
							DropOnTable(pickedItem, ItemSlot.transform);
							colObj.GetComponent<DispenseTable>().ItemOnCounter = true;
						}
						
					}else if(findColTableType()){
						// TOTEST
						if(!colObj.GetComponent<ToolTable>()){
							
							if(findColTableType().ItemOnCounter == true){
								CheckCombinable();
							}else{
								DropOnTable(pickedItem, ItemSlot.transform);
								findColTableType().ItemOnCounter = true;
							}
						}else{
							Debug.Log("Error: Get tool table");
						}
					}else{
						Debug.Log("Error on update ItemOnCounter after drop on table.");
					}
					
				}else{
					DropItem(pickedItem);
				}
				// If hand is empty
			}else{
				if(colObj.CompareTag("Table")){
					
					// If is in item making progress, reset
					if(isMaking == true && findColTableType()){
						findColTableType().resetIfLeave();
					}
					
					var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, 3f))
					{
						var pickable = hit.transform.GetComponent<PickableItem>();
						
						if(colObj.GetComponent<DispenseTable>() && colObj.GetComponent<DispenseTable>().ItemOnCounter == false){
							
							SpawnItem(colObj.GetComponent<DispenseTable>().ItemToDispense);
						}else{
							if (pickable){
								
								if(colObj.GetComponent<EmptyTable>() && colObj.GetComponent<EmptyTable>().ItemOnCounter == true){
									
									colObj.GetComponent<EmptyTable>().ItemOnCounter = false;
								}else if(colObj.GetComponent<DispenseTable>() && colObj.GetComponent<DispenseTable>().ItemOnCounter == true){

									colObj.GetComponent<DispenseTable>().ItemOnCounter = false;
								}else if(findColTableType() && findColTableType().ItemOnCounter == true){
									
									findColTableType().ItemOnCounter = false;
								}else{
									
									PickItem(pickable);
								}
							}
						}
					}
				}
			}
		}
		
		
		if (Input.GetKeyDown(KeyCode.LeftShift) && colObj.CompareTag("Table"))  {
			// check counter condition and item condition
			// if possible only proceed own progress [start stop/doing] based on tag
			if(findColTableType()){
				findColTableType().makeProduct();
				isMaking = true;
			}else{
				//TODO Error Sound
			}

			
		}
	}
	
	
	void OnTriggerEnter(Collider col) 
	{
		Debug.Log("Col");
		
		colObj = col.gameObject;
		Debug.Log(colObj.name);
		
		//TODO del if no use
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
		
		//TODO del if no use
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
		colObj = null;
	}
	
	
	#region private func
	
	// Spawn item from item dispenser
	private void SpawnItem(GameObject prefab)
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
		
		if(findProductType(pickedItem.gameObject) == findProductType(itemOnTable)){
			//TODO: should include processed bool check
			if(findProductType(pickedItem.gameObject).spoiled == false && findProductType(pickedItem.gameObject).processed == true && findProductType(itemOnTable).spoiled == false && findProductType(itemOnTable).processed == true){
				List<string> MatchedItem = findProductType(itemOnTable).inContainer.Intersect(findProductType(itemOnTable).allowedItems).ToList();
				if(MatchedItem.Count() == 1){
					foreach (string item in MatchedItem){
						pickedItem.Destroy();
						pickedItem = null;
						findProductType(itemOnTable).inContainer.Add(item);
						findProductType(itemOnTable).allowedItems.Remove(item);
						// for(int i=0; i< itemOnTable.transform.childCount; i++)
						// {
						// 	var child = itemOnTable.transform.GetChild(i).gameObject;
						// 	if(child != null)
						// 	child.SetActive(false);
						// }
						itemOnTable.transform.Find(item).gameObject.SetActive(true);
					}
				}else if(MatchedItem.Count() == 0){
					// TODO Error Sound
				}else{
					Debug.Log("MatchedItem.Count >= 1 / =0");	
				}
			}else{
				Debug.Log("CheckCombinable(): Error getting itemType");
			}
		}else if(pickedItem.gameObject.CompareTag("Crate") && IsItem(itemOnTable) == true){		
			if(findProductType(itemOnTable).spoiled == false && findProductType(itemOnTable).inCrate == false && findProductType(itemOnTable).completed == true){
				pickedItem.Destroy();
				pickedItem = null;
				findProductType(itemOnTable).inContainer.Add("Crate");
				findProductType(itemOnTable).allowedItems.Remove("Crate");
				findProductType(itemOnTable).inCrate = true;
				itemOnTable.transform.Find("Crate").gameObject.SetActive(true);
			}else{
				//TODO error sound effect
			}
			
		}else if(itemOnTable.CompareTag("Crate") && IsItem(pickedItem.gameObject) == true){
			if(findProductType(pickedItem.gameObject).spoiled == false && findProductType(pickedItem.gameObject).inCrate == false && findProductType(pickedItem.gameObject).completed == true){
				itemOnTable.GetComponent<PickableItem>().Destroy();
				findProductType(pickedItem.gameObject).inContainer.Add("Crate");
				findProductType(pickedItem.gameObject).allowedItems.Remove("Crate");
				findProductType(pickedItem.gameObject).inCrate = true;
				pickedItem.gameObject.transform.Find("Crate").gameObject.SetActive(true);
				GameObject ItemSlot = colObj.transform.GetChild(0).gameObject;
				DropOnTable(pickedItem, ItemSlot.transform);
			}else{
				//TODO error sound effect
			}
		}else{
			//TODO error sound effect
		}
	}

	// TODO del if able to replace by findColTableType
	// private string CheckItemType(GameObject item)
	// {
	// 	if(item.GetComponent<Engine>()){
	// 		return "Engine";
	// 	}else if(item.GetComponent<Propeller>()){
	// 		return "Propeller";
	// 	}else if(item.GetComponent<SphereProjector>()){
	// 		return "SphereProjector";
	// 	}else{
	// 		Debug.Log("CheckItemType(): Error");
	// 		return "Error";
	// 	}
	// }

	// Make sure item on hand is not a container
	private bool IsItem(GameObject item)
	{
		if(item.GetComponent<Engine>() || item.GetComponent<Propeller>()|| item.GetComponent<SphereProjector>()){
			return true;
		}else{
			return false;
		}
	}
	
	// private bool IsToolTable(GameObject item)
	// {
	// 	if(item.GetComponent<WrenchTable>() || item.GetComponent<SprayTable>()|| item.GetComponent<HammerTable>() || item.GetComponent<DrillerTable>() || item.GetComponent<ScrewDriverTable>()|| item.GetComponent<ClothTable>()){
	// 		return true;
	// 	}else{
	// 		return false;
	// 	}
	// }

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

	private Product findProductType(GameObject obj)
	{	
		if (obj.GetComponent<Engine>()){
			return obj.GetComponent<Engine>();
		}else if(obj.GetComponent<SphereProjector>()){
			return obj.GetComponent<SphereProjector>();
		}else if(obj.GetComponent<Propeller>()){
			return obj.GetComponent<Propeller>();
		}else{
			Debug.Log("findColProductType(): Error");
			return obj.GetComponent<Product>();
		}
	}
#endregion
}
