using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Parent class for all product
public class Product: MonoBehaviour
{
	public List<string> inContainer;
	public List<string> allowedItems;
	public bool completed;
	public bool processed;
	public bool spoiled;
	public bool inCrate;
}
