using UnityEngine;
using System.Collections;

public class HighFive : MonoBehaviour {

    public ParticleSystem effect;

    void OnTriggerEnter(Collider collisionInfo)
    {
        if (collisionInfo.gameObject.name == "palm")
        {
            Debug.Log("palm detected collision with " + collisionInfo.gameObject.name);
            effect.transform.position = gameObject.transform.position;
            GameObject instatiatedParticle = Instantiate(effect, effect.transform.position, Quaternion.identity) as GameObject;
            Destroy(instatiatedParticle,effect.duration);
        }
    }
}
