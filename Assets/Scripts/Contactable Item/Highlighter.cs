using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Attach to any gameObject that you can interact, will highlight the item when collide with it
public class Highlighter : MonoBehaviour {

	private Color cbeforeHighlight;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		Renderer render = GetComponent<Renderer> ();

		cbeforeHighlight = render.material.color; 
		render.material.color = cbeforeHighlight + new Color(0.3f, 0.3f, 0.3f);
	}

	void OnTriggerExit(Collider other)
	{
		Renderer render = GetComponent<Renderer> ();
		render.material.color = cbeforeHighlight;
	}

}
