using UnityEngine;
using System.Collections;

public class RemoteRHandController : RemoteHandController
{

    protected override void enableRemoteHand()
    {
        photonView.RPC("enableRemoteRHand", PhotonTargets.OthersBuffered);
    }
    protected override void getRidgidHand()
    {
        rigidHand = transform.Find("RigidRoundHand_R(Clone)").gameObject;
    }

    [PunRPC]
    void enableRemoteRHand()
    {
        if (handEnabled == false && !photonView.isMine)
        {
            enableHandsRenderers(remoteHand.transform);
            handEnabled = true;
        }
    }
}

