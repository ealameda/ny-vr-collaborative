using UnityEngine;
using System.Collections;

public class Toolbelt : MonoBehaviour
{
    public float yOffset;

    private Transform centerEyeAnchor;
	
    void Start()
    {
        centerEyeAnchor = transform.root.Find("Camera");
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(centerEyeAnchor.position.x, centerEyeAnchor.position.y + yOffset, centerEyeAnchor.position.z);
    }
}
