using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothTable : ToolTable
{
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
			if (item.CompareTag("A2") && item.GetComponent<SphereProjector>().spoiled == false){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	
	
	public override void makeProduct(){
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
}
