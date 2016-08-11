using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class RoomManager : Photon.MonoBehaviour
{
    public bool debugPlayerStartACK = false;
    private int remotePlayersACKCount = 0;
    public bool localPlayerStartACK = false;

    private float startTime;
    private bool playingRound = false;

    public float roundLengthInMinutes;

    private UnityAction playerJoinedEvent;

    #region Singleton
    private static RoomManager instance;
    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(RoomManager)) as RoomManager;

                if (instance == null)
                {
                    Debug.LogError("There needs to be one active RoomManager script on a GameObject in your scene.");
                }
            }
            return instance;
        }
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    void Awake()
    {
        Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (((Time.time - startTime) >= (roundLengthInMinutes*60)) && playingRound)
        {
            StopTimer();
        }
        else if (((PhotonNetwork.playerList.Length - 1) == remotePlayersACKCount) 
                && (PhotonNetwork.playerList.Length > 1) 
                && localPlayerStartACK 
                && !playingRound)
        {
            StartRound();
        }
        else if (debugPlayerStartACK && localPlayerStartACK && !playingRound)
        {
            StartRound();
        }
	}

    public void ThumbsUpACK()
    {
        localPlayerStartACK = !localPlayerStartACK;
        if (localPlayerStartACK)
        {
            photonView.RPC("remoteThumbsUpACK", PhotonTargets.Others);
        }
        else
        {
            photonView.RPC("remoteThumbsNotUp", PhotonTargets.Others);
        }
    }

    public void StartRound()
    {
        EventManager.TriggerEvent(EventName.RoundStart);
        startTime = Time.time;
        playingRound = true;
    }

    public void StopTimer()
    {
        playingRound = false;
        localPlayerStartACK = false;
        EventManager.TriggerEvent(EventName.RoundEnd);
    }

    void OnEnable()
    {
        EventManager.StartListening(EventName.ThumbsUp, ThumbsUpACK);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventName.ThumbsUp, ThumbsUpACK);
    }

    [PunRPC]
    public void remoteThumbsUpACK()
    {
        remotePlayersACKCount++;
    }

    [PunRPC]
    public void remoteThumbsNotUp()
    {
        remotePlayersACKCount--;
    }
}
