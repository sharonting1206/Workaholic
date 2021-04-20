using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrillerTable : ToolTable
{
	
	[SerializeField]
	public Image overtimeProgressBar;
	public float overtimeProgress = 0;
	
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();
    }
	
	protected override bool checkCondition(){
		if (ItemSlot.transform.GetChild(0)){
			GameObject item = ItemSlot.transform.GetChild(0).gameObject;
			if (item.CompareTag("B1") && item.GetComponent<Propeller>().spoiled == false){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	
	//makeProductOvertime()
	public override void makeProduct(){
		if (checkCondition()){
			makeProgress += makeSpeed * Time.deltaTime;
			makeProgress = Mathf.Clamp(makeProgress, 0, 10f);
			progressCanvas.SetActive(true);
			makeProgressBar.enabled = true;
			makeProgressBar.fillAmount = makeProgress / 10f;
			if(makeProgress >= 10f){
				makeProgressBar.enabled = false;
				GameObject product = ItemSlot.transform.GetChild(0).gameObject;
				product.transform.GetChild(0).gameObject.SetActive(false);
				product.transform.GetChild(1).gameObject.SetActive(true);
				
				overtimeProgressBar.enabled = true;
				overtimeProgress += makeSpeed * Time.deltaTime;
				overtimeProgress = Mathf.Clamp(overtimeProgress, 0, 5f);
				overtimeProgressBar.fillAmount = overtimeProgress / 5f;
				if(overtimeProgress >= 5f){
					product.transform.GetChild(1).gameObject.SetActive(false);
					product.transform.GetChild(4).gameObject.SetActive(true);
					product.GetComponent<Engine>().spoiled = true;
				}
			}
		}
	}
	
	public override void resetIfLeave(){
		overtimeProgress = 0;
		overtimeProgressBar.enabled = false;
		makeProgress = 0;
		makeProgressBar.enabled = false;
		progressCanvas.SetActive(false);
	}
}
