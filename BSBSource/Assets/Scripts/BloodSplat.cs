using UnityEngine;

public class BloodSplat : MonoBehaviour {


	void Update ()
	{
	    if (transform.localScale.x >= 1f)
	    {
	        enabled = false;
            return;
        }
	    
        var newScale = new Vector3(transform.localScale.x + Time.deltaTime, transform.localScale.y + Time.deltaTime, transform.localScale.z);
	    transform.localScale = newScale;

	}
}
