using UnityEngine;
using System.Collections;

public class HandStyle : MonoBehaviour {

    public Color boneColor;
    public Color jointColor;

    private GameObject[] bones;
    private GameObject[] joints;




	// Use this for initialization
	void Start () {

       

    }
	
	// Update is called once per frame
	void Update () {

        changeBoneColor();
        changeJointColor();
        
    }

    void changeBoneColor()
    {

        bones = GameObject.FindGameObjectsWithTag("hand_bones");
        foreach (GameObject bone in bones)
        {
            bone.GetComponent<Renderer>().material.color = boneColor;
        }

    }

    void changeJointColor()
    {

        joints = GameObject.FindGameObjectsWithTag("hand_joints");
        foreach (GameObject joint in joints)
        {
            joint.GetComponent<Renderer>().material.color = jointColor;
        }

    }
}
