using UnityEngine;
using System.Collections;

public class HighFive : Photon.MonoBehaviour {

    public GameObject effectPrimary;
    public GameObject effectSecondary;

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.name == "remotePalm") {
            GameObject tempThinggie = collisionInfo.gameObject;
            PhotonView tempThinggiePV = tempThinggie.GetComponent<PhotonView>();
            if (tempThinggiePV.isMine)
            {
                Debug.Log("palm detected collision with " + collisionInfo.gameObject.name);
                GameObject effectPrimaryInstance = Instantiate(effectPrimary, gameObject.transform.position, Quaternion.identity) as GameObject;
                GameObject effectSecondaryInstance = Instantiate(effectSecondary, gameObject.transform.position, Quaternion.identity) as GameObject;

                Destroy(effectPrimaryInstance, 2);
                Destroy(effectSecondaryInstance, 2);
            }
        }
    }
}
