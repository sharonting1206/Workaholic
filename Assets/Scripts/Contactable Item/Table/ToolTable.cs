using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// parent class, shouldn't attached to any gameObject
public class ToolTable : Table
{
	//[SerializeField]
	//public Image overtimeProgressBar;
	//public float overtimeProgress = 0;
	
	[SerializeField]
	public Image makeProgressBar;
	public float makeProgress = 0;

	public GameObject progressCanvas;
	public float makeSpeed = 1.2f;
	public float changeVelocity;	
	
	// Start is called before the first frame update
	protected virtual void Start()
	{
		// declare table properties
		ItemOnCounter = false;
		continuousAction = true;
	}

	// Update is called once per frame
	protected virtual void Update()
	{
		// update table properties if itemOnCounter status is changed
		if (ItemOnCounter == false){
			continuousAction = true;
		}else{
			continuousAction = false;
		}
	}
	
	// Sample checkCondition script, must override in the child class if it is not suitable for your case
	protected virtual bool checkCondition(){
		if (ItemSlot.transform.GetChild(0)){
			GameObject item = ItemSlot.transform.GetChild(0).gameObject;
			if (item.CompareTag("C2") && item.GetComponent<Engine>().spoiled == false){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	
	// Sample makeProduct(cook) script, must override in child class if not suitable
	public virtual void makeProduct(){
		if (checkCondition()){
			makeProgress += makeSpeed * Time.deltaTime;
			makeProgress = Mathf.Clamp(makeProgress, 0, 10f);
			progressCanvas.SetActive(true);
			makeProgressBar.enabled = true;
			makeProgressBar.fillAmount = makeProgress / 10f;
			// TODO add burst effect
			if(makeProgress >= 10f){
				makeProgressBar.enabled = false;
				progressCanvas.SetActive(false);
				makeProgress = 0;
				GameObject itemPart = ItemSlot.transform.GetChild(0).gameObject;
				itemPart.transform.GetChild(0).gameObject.SetActive(false);
				itemPart.transform.GetChild(1).gameObject.SetActive(true);
			}
		}else{
			//TODO Prompt error sound;
		}
	}
	
	// Rename as makeProduct/override with this code and delete prev func if this is more suitable for you
	
	//public void makeProductOvertime(){
	//	if (checkCondition()){
	//		makeProgress += makeSpeed * Time.deltaTime;
	//		makeProgress = Mathf.Clamp(makeProgress, 0, 10f);
	//		progressCanvas.SetActive(true);
	//		makeProgressBar.enabled = true;
	//		makeProgressBar.fillAmount = makeProgress / 10f;
	//		if(makeProgress >= 10f){
	//			makeProgressBar.enabled = false;
	//			GameObject product = ItemSlot.transform.GetChild(0).gameObject;
	//			product.transform.GetChild(0).gameObject.SetActive(false);
	//			product.transform.GetChild(1).gameObject.SetActive(true);
	//			
	//			overtimeProgressBar.enabled = true;
	//			overtimeProgress += cookSpeed * Time.deltaTime;
	//			overtimeProgress = Mathf.Clamp(overtimeProgress, 0, 5f);
	//			overtimeProgressBar.fillAmount = overtimeProgress / 5f;
	//			if(overtimeProgress >= 5f){
	//				product.transform.GetChild(1).gameObject.SetActive(false);
	//				product.transform.GetChild(4).gameObject.SetActive(true);
	//				product.GetComponent<Engine>().spoiled = true;
	//			}
	//		}
	//	}
	//}
	
	// Reset everytime if player leave table halfway during the progress of making product
	public virtual void resetIfLeave(){
		// overtimeProgress = 0;
		// overtimeProgressBar.enabled = false;
		makeProgress = 0;
		makeProgressBar.enabled = false;
		progressCanvas.SetActive(false);
	}
}
