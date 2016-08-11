using UnityEngine;
using System.Collections;

public class HighFive : MonoBehaviour {

    public GameObject effectPrimary;
    public GameObject effectSecondary;

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.name == "palm")
        {
            Debug.Log("palm detected collision with " + collisionInfo.gameObject.name);
            GameObject effectPrimaryInstance = Instantiate(effectPrimary, gameObject.transform.position, Quaternion.identity) as GameObject;
            GameObject effectSecondaryInstance = Instantiate(effectSecondary, gameObject.transform.position, Quaternion.identity) as GameObject;

            Destroy(effectPrimaryInstance, 2);
            Destroy(effectSecondaryInstance, 2);
        }
    }
}
