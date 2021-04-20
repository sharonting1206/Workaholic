using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to Empty GameObject
// Watch this: https://www.youtube.com/watch?v=kTvBRkPTvRY&t=207s&ab_channel=Gamad
public class RandomSpawn : MonoBehaviour
{
	//Size = spawn area size
	public Vector3 size;
	public GameObject SpawnSpots;
	public List<GameObject> ItemPrefabsL;
	float timer = 0f;
	
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update(){
		
		timer += Time.deltaTime;
		
		// Set how long you want for each spawn
		if( timer > 10f ) {
			SpawnItem();
			timer = 0f;
		}
	}
	
	void OnDrawGizmosSelected()
	{
		// Display the explosion radius when selected
		Gizmos.color = new Color(1, 0, 0, 5F);
		Gizmos.DrawCube(transform.localPosition, size);
	}
	
	private void SpawnItem(){
		
		Vector3 position = transform.localPosition + new Vector3(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));
		Instantiate(ItemPrefabsL[Random.Range(0, ItemPrefabsL.Count)], position, Quaternion.identity);
		// Attach a sound effect better
		
	}
	
}
