using UnityEngine;

// Attach this class to make object pickable.

[RequireComponent(typeof(Rigidbody))]
public class PickableItem : MonoBehaviour
{
    // Reference to the rigidbody
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    private void Awake()
    {
		Debug.Log("halo");
         rb = GetComponent<Rigidbody>();
    }
	
	public void Destroy(){
		Destroy(gameObject);
	}	
}