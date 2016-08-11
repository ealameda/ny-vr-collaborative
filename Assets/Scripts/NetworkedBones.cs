using UnityEngine;
using System.Collections;

public class NetworkedBones : NetworkedObject 
{

	// Use this for initialization
	public override void Start() 
	{
		lerpObject = gameObject;
	}
}
