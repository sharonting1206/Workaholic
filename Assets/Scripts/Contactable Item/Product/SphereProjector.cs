using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjector : Product
{
    // Start is called before the first frame update
    void Start()
    {
		completed = false;
		processed = false;
		spoiled = false;
		inCrate = false;
		if(gameObject.CompareTag("A1")){
			inContainer.Add("A1");
			allowedItems.Add("A2");
			allowedItems.Add("A3");
			allowedItems.Add("Crate");
		}
		else if(gameObject.CompareTag("A2")){
			inContainer.Add("A2");
			allowedItems.Add("A1");
			allowedItems.Add("A3");
			allowedItems.Add("Crate");
		}
		else if(gameObject.CompareTag("A3")){
			inContainer.Add("A3");
			allowedItems.Add("A1");
			allowedItems.Add("A2");
			allowedItems.Add("Crate");
			
		}else{
			Debug.Log("Error in SphereProjector.cs");
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
