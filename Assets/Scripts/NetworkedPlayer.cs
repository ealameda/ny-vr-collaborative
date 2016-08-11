using UnityEngine;
using System.Collections.Generic;

public class NetworkedPlayer : NetworkedObject
{
    public override void Start()
    {
        lerpObject = transform.Find("Camera").gameObject;
        GameObject leapSpace = lerpObject.transform.Find("LeapSpace").gameObject;
        GameObject leapHandController = leapSpace.transform.Find("LeapHandController").gameObject;

        GameObject faceLipSync = lerpObject.transform.Find("Head").gameObject;
        GameObject lipSyncTargets = faceLipSync.transform.Find("LipSyncTargets").gameObject;
        GameObject inputTypePhotonVoice = lipSyncTargets.transform.Find("InputType_PhotonVoice").gameObject;
        
        if (photonView.isMine)
        {
            lerpObject.transform.GetComponent<Camera>().enabled = true;
            lerpObject.transform.GetComponent<AudioListener>().enabled = true;

            (leapHandController.GetComponent("LeapHandController") as MonoBehaviour).enabled = true;
            (leapHandController.GetComponent("LeapServiceProvider") as MonoBehaviour).enabled = true;
        }
        if (!photonView.isMine) {
            (inputTypePhotonVoice.GetComponent("OVRLipSyncContext") as MonoBehaviour).enabled = true;
            (inputTypePhotonVoice.GetComponent("OVRLipSyncContextTextureFlip") as MonoBehaviour).enabled = true;
            (inputTypePhotonVoice.GetComponent("OVRLipSyncContextMorphTarget") as MonoBehaviour).enabled = true;
        }
    }
}