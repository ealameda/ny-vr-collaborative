using UnityEngine;
using System.Collections;
using System;

public class RemoteLHandController : RemoteHandController
{

    protected override void enableRemoteHand()
    {
        photonView.RPC("enableRemoteLHand", PhotonTargets.OthersBuffered);
    }

    protected override void getRidgidHand()
    {
        rigidHand = transform.Find("RigidRoundHand_L(Clone)").gameObject;
    }

    [PunRPC]
    void enableRemoteLHand()
    {
        if (handEnabled == false && !photonView.isMine)
        {
            enableHandsRenderers(remoteHand.transform);
            handEnabled = true;
        }
    }
}
