//using UnityEngine;
//using System.Collections;

//public class RemoteMouthController : MonoBehaviour 
//{
//    public GameObject remoteMouth;

//    // Use this for initialization
//    void Start()
//    {
//        if (!photonview.ismine || showhands)
//        {
//            enablehandsrenderers(remotehand.transform);
//        }

//        if (remotehand.name == "remotehand_r")
//        {
//            rigidhand = transform.find("rigidroundhand_r(clone)");
//        }

//        if (remotehand.name == "remotehand_l")
//        {
//            rigidhand = transform.find("rigidroundhand_l(clone)");
//        }

//        palm = rigidhand.find("palm").gameobject;
//        forearm = rigidhand.find("forearm").gameobject;

//        transform rigidthumb = rigidhand.find("thumb");
//        thumb = new list<transform>();
//        for (int i = 0; i < rigidthumb.childcount; i++)
//        {
//            thumb.add(rigidthumb.getchild(i));
//        }

//        transform rigidindex = rigidhand.find("index");
//        index = new list<transform>();
//        for (int i = 0; i < rigidindex.childcount; i++)
//        {
//            index.add(rigidindex.getchild(i));
//        }

//        transform rigidmiddle = rigidhand.find("middle");
//        middle = new list<transform>();
//        for (int i = 0; i < rigidmiddle.childcount; i++)
//        {
//            middle.add(rigidmiddle.getchild(i));
//        }

//        transform rigidring = rigidhand.find("ring");
//        ring = new list<transform>();
//        for (int i = 0; i < rigidring.childcount; i++)
//        {
//            ring.add(rigidring.getchild(i));
//        }

//        transform rigidpinky = rigidhand.find("pinky");
//        pinky = new list<transform>();
//        for (int i = 0; i < rigidpinky.childcount; i++)
//        {
//            pinky.add(rigidpinky.getchild(i));
//        }
//    }

//    void Update()
//    {
//        if (photonView.isMine)
//        {
//            remotePalm.transform.position = palm.transform.position;
//            remotePalm.transform.rotation = palm.transform.rotation;

//            remoteForearm.transform.position = forearm.transform.position;
//            remoteForearm.transform.rotation = forearm.transform.rotation;

//            for (int i = 0; i < remoteThumb.Count; i++)
//            {
//                remoteThumb[i].transform.position = thumb[i].position;
//                remoteThumb[i].transform.rotation = thumb[i].rotation;
//            }

//            for (int i = 0; i < remoteIndex.Count; i++)
//            {
//                remoteIndex[i].transform.position = index[i].position;
//                remoteIndex[i].transform.rotation = index[i].rotation;
//            }

//            for (int i = 0; i < remoteMiddle.Count; i++)
//            {
//                remoteMiddle[i].transform.position = middle[i].position;
//                remoteMiddle[i].transform.rotation = middle[i].rotation;
//            }

//            for (int i = 0; i < remoteRing.Count; i++)
//            {
//                remoteRing[i].transform.position = ring[i].position;
//                remoteRing[i].transform.rotation = ring[i].rotation;
//            }

//            for (int i = 0; i < remotePinky.Count; i++)
//            {
//                remotePinky[i].transform.position = pinky[i].position;
//                remotePinky[i].transform.rotation = pinky[i].rotation;
//            }
//        }
//    }

//    void enableHandsRenderers(Transform hand)
//    {
//        for (int i = 0; i < hand.childCount; i++)
//        {
//            enableHandsRenderers(hand.GetChild(i));
//        }
//        Renderer rend = hand.GetComponent<Renderer>();
//        if (rend != null)
//        {
//            rend.enabled = true;
//        }
//    }
//}
