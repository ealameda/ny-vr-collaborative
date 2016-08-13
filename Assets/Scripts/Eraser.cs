using UnityEngine;
using System.Collections;

public class Eraser : Photon.MonoBehaviour
{
    public Vector3 posistionOffset;
    private GameObject[] lines;
    public float eraseDelay;
    private float countdown;
    private bool erased;

    void OnTriggerEnter(Collider collisionInfo)
    {
        countdown = eraseDelay;
        erased = false;
        Debug.Log("i touched an eraser and i liked it");
    }

    void OnTriggerStay(Collider collisionInfo)
    {
        if (countdown <= 0 && !erased)
        {
            Debug.Log("erasing now");
            lines = GameObject.FindGameObjectsWithTag("Line");
            foreach (GameObject line in lines)
            {
                Destroy(line);
            }
            photonView.RPC("EraseRemoteLines", PhotonTargets.OthersBuffered);
            erased = true;
        }
        countdown -= Time.deltaTime;
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
