using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attached on table gameObject that you wanna spawn the plates
public class PlateSpawn : MonoBehaviour
{
	public GameObject SpawnSpot;
	public GameObject PlatePrefabs;
	public GameObject[] PlatesInScene;

	float timer = 0f;
	float loadingtime = 0f;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		PlatesInScene = GameObject.FindGameObjectsWithTag("Plate");
		
		// Set how long you want for each spawn & max plates you want for the level
		if( timer > 2f && PlatesInScene.Length < 5 ) {
			if(gameObject.GetComponent<EmptyTable>().ItemOnCounter == false){
				loadingtime += Time.deltaTime;
				if (loadingtime > 2f){
					SpawnPlate();
					timer = 0f;				
					loadingtime = 0f;
				}
			}
		}
	}
	
	
	private void SpawnPlate(){
		Instantiate(PlatePrefabs, SpawnSpot.transform.position, Quaternion.identity);
		gameObject.GetComponent<EmptyTable>().ItemOnCounter = true;
		// Attach a sound effect better 
	}
}
