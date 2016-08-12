using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class RemoteHandController : Photon.MonoBehaviour
{
    public GameObject remoteHand;
    protected GameObject rigidHand;

    public bool showHands;
    protected bool handEnabled = false;

    // rigid hand
    protected GameObject palm;
    protected GameObject forearm;
    protected List<Transform> thumb;
    protected List<Transform> index;
    protected List<Transform> middle;
    protected List<Transform> ring;
    protected List<Transform> pinky;

    // remote hand parts
    protected GameObject remotePalm;
    protected GameObject remoteForearm;
    protected List<GameObject> remoteThumb;
    protected List<GameObject> remoteIndex;
    protected List<GameObject> remoteMiddle;
    protected List<GameObject> remotePinky;
    protected List<GameObject> remoteRing;

    // Use this for initialization
    void Start()
    {
        if (showHands)
        {
            enableHandsRenderers(remoteHand.transform);
        }

        getRidgidHand();

        palm = rigidHand.transform.Find("palm").gameObject;
        forearm = rigidHand.transform.Find("forearm").gameObject;

        Transform rigidThumb = rigidHand.transform.Find("thumb");
        thumb = new List<Transform>();
        for (int i=0;i<rigidThumb.childCount;i++)
        {
            thumb.Add(rigidThumb.GetChild(i));
        }

        Transform rigidIndex = rigidHand.transform.Find("index");
        index = new List<Transform>();
        for (int i=0;i<rigidIndex.childCount;i++)
        {
            index.Add(rigidIndex.GetChild(i));
        }

        Transform rigidMiddle = rigidHand.transform.Find("middle");
        middle = new List<Transform>();
        for (int i=0;i<rigidMiddle.childCount;i++)
        {
            middle.Add(rigidMiddle.GetChild(i));
        }

        Transform rigidRing = rigidHand.transform.Find("ring");
        ring = new List<Transform>();
        for (int i=0;i<rigidRing.childCount;i++)
        {
            ring.Add(rigidRing.GetChild(i));
        }

        Transform rigidPinky = rigidHand.transform.Find("pinky");
        pinky = new List<Transform>();
        for (int i=0;i<rigidPinky.childCount;i++)
        {
            pinky.Add(rigidPinky.GetChild(i));
        }

        remotePalm = remoteHand.transform.Find("remotePalm").gameObject;
        remoteForearm = remoteHand.transform.Find("forearm").gameObject;

        remoteThumb = new List<GameObject>();
        Transform remoteThumbTransform = remoteHand.transform.Find("thumb");
        for (int i = 0; i < remoteThumbTransform.childCount; i++)
        {
            remoteThumb.Add(remoteThumbTransform.GetChild(i).gameObject);
        }

        remoteIndex = new List<GameObject>();
        Transform remoteIndexTransform = remoteHand.transform.Find("index");
        for (int i = 0; i < remoteIndexTransform.childCount; i++)
        {
            remoteIndex.Add(remoteIndexTransform.GetChild(i).gameObject);
        }

        remoteMiddle = new List<GameObject>();
        Transform remoteMiddleTransform = remoteHand.transform.Find("middle");
        for (int i = 0; i < remoteMiddleTransform.childCount; i++)
        {
            remoteMiddle.Add(remoteMiddleTransform.GetChild(i).gameObject);
        }
        remoteRing = new List<GameObject>();
        Transform remoteRingTransform = remoteHand.transform.Find("ring");
        for (int i = 0; i < remoteRingTransform.childCount; i++)
        {
            remoteRing.Add(remoteRingTransform.GetChild(i).gameObject);
        }
        remotePinky = new List<GameObject>();
        Transform remotePinkyTransform = remoteHand.transform.Find("pinky");
        for (int i = 0; i < remotePinkyTransform.childCount; i++)
        {
            remotePinky.Add(remotePinkyTransform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (handEnabled == false && rigidHand.activeSelf)
            {
                handEnabled = true;
                //photonView.RPC("enableRemoteHands", PhotonTargets.All);
                enableRemoteHand();
                Debug.Log("enableRemoteHands RPC called");
            }

            remotePalm.transform.position = palm.transform.position;
            remotePalm.transform.rotation = palm.transform.rotation;

            remoteForearm.transform.position = forearm.transform.position;
            remoteForearm.transform.rotation = forearm.transform.rotation;

            for (int i = 0; i < remoteThumb.Count; i++)
            {
                remoteThumb[i].transform.position = thumb[i].position;
                remoteThumb[i].transform.rotation = thumb[i].rotation;
            }

            for (int i = 0; i < remoteIndex.Count; i++)
            {
                remoteIndex[i].transform.position = index[i].position;
                remoteIndex[i].transform.rotation = index[i].rotation;
            }

            for (int i = 0; i < remoteMiddle.Count; i++)
            {
                remoteMiddle[i].transform.position = middle[i].position;
                remoteMiddle[i].transform.rotation = middle[i].rotation;
            }

            for (int i = 0; i < remoteRing.Count; i++)
            {
                remoteRing[i].transform.position = ring[i].position;
                remoteRing[i].transform.rotation = ring[i].rotation;
            }

            for (int i = 0; i < remotePinky.Count; i++)
            {
                remotePinky[i].transform.position = pinky[i].position;
                remotePinky[i].transform.rotation = pinky[i].rotation;
            }
        }
    }

    protected abstract void enableRemoteHand();

    protected abstract void getRidgidHand();

    protected void enableHandsRenderers(Transform hand)
    {
        for (int i=0;i<hand.childCount;i++) 
        {
            enableHandsRenderers(hand.GetChild(i)); 
        }
        Renderer rend = hand.GetComponent<Renderer>();
        if (rend  != null)
        {
            rend.enabled = true;
        }
    }
}
