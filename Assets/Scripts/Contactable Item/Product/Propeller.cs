using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : Product
{
    // Start is called before the first frame update
    void Start()
    {
		completed = false;
		processed = false;
		spoiled = false;
		inCrate = false;
		if(gameObject.CompareTag("B1")){
			inContainer.Add("B1");
			allowedItems.Add("B2");
			allowedItems.Add("Crate");
		}
		else if(gameObject.CompareTag("B2")){
			inContainer.Add("B2");
			allowedItems.Add("B1");
			allowedItems.Add("Crate");
		}
		else {
			Debug.Log("Error in Propeller.cs");
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
