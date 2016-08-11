using UnityEngine;
using System.Collections;

public class Eraser : Photon.MonoBehaviour
{
    public Vector3 posistionOffset;
    private GameObject[] lines;

    void OnTriggerEnter(Collider collisionInfo)
    {
        Debug.Log("i touched an eraser and i liked it");
        lines = GameObject.FindGameObjectsWithTag("Line");
        foreach (GameObject line in lines)
        {
            Destroy(line);
        }
        photonView.RPC("EraseRemoteLines", PhotonTargets.OthersBuffered);
    }

    [PunRPC]
    void EraseRemoteLines()
    {
        lines = GameObject.FindGameObjectsWithTag("RemoteLine");
        foreach (GameObject line in lines)
        {
            Destroy(line);
            Debug.Log("deleted the remote lines");
        }
    }
}
