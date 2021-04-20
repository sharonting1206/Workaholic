using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenseTable : Table
{
	
	public GameObject ItemToDispense;
	public Transform ItemSpawnPosition;
	// Start is called before the first frame update
	void Start()
	{
		ItemOnCounter = false;
		continuousAction = true;
	}

	// Update is called once per frame
	void Update()
	{
		if (ItemOnCounter == false){
			continuousAction = true;
		}else{
			continuousAction = false;
		}
	}
	

}
