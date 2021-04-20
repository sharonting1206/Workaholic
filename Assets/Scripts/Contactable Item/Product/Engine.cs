using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Product
// Product
{

    // Start is called before the first frame update
    void Start()
    {

		completed = false;
		processed = false;
		spoiled = false;
		inCrate = false;
		if(gameObject.CompareTag("C1")){
			inContainer.Add("C1");
			allowedItems.Add("C2");
			allowedItems.Add("Crate");
		}
		else if(gameObject.CompareTag("C2")){
			inContainer.Add("C2");
			allowedItems.Add("C1");
			allowedItems.Add("Crate");
		}
		else {
			Debug.Log("Error in Engine.cs");
		}
    }

    // Update is called once per frame
    void Update()
    {

    }
	
}
