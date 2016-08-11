using UnityEngine;
using System.Collections;

public abstract class NetworkedObject : Photon.MonoBehaviour 
{
	protected Vector3 lerpToPos;
	protected Quaternion lerpToRot;

	protected GameObject lerpObject;

	public float MovementSmoothing = 7.5f;

	// Use this for initialization
	public abstract void Start ();
	
	// Update is called once per frame
	public void Update () 
	{
		if (!photonView.isMine)
        {
            lerpObject.transform.position = Vector3.Lerp(lerpObject.transform.position, lerpToPos, Time.deltaTime * MovementSmoothing);
            lerpObject.transform.rotation = Quaternion.Lerp(lerpObject.transform.rotation, lerpToRot, Time.deltaTime * MovementSmoothing);
        }
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(lerpObject.transform.position);
            stream.SendNext(lerpObject.transform.rotation);
        }
        else
        {
            lerpToPos = (Vector3)stream.ReceiveNext();
            lerpToRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
