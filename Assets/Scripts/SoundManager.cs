using UnityEngine;
using System.Collections;

public class SoundManager : Photon.MonoBehaviour 
{
    public AudioSource roundStartFX;
    public AudioSource roundOverFX;
    public AudioSource fireworksFX;
    public AudioSource applauseFX;

    #region Singleton
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (instance == null)
                {
                    Debug.LogError("There needs to be one active SoundManager script on a GameObject in your scene.");
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

    void OnEnable()
    {
        EventManager.StartListening(EventName.RoundStart, PlayRoundStart);
        EventManager.StartListening(EventName.RoundEnd, PlayRoundEnd);
        EventManager.StartListening(EventName.FireworksOver, StopRoundEnd);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventName.RoundStart, PlayRoundStart);
        EventManager.StopListening(EventName.RoundEnd, PlayRoundEnd);
        EventManager.StopListening(EventName.FireworksOver, StopRoundEnd);
    }

    public void PlayRoundStart()
    {
        roundStartFX.Play();
    }

    public void PlayRoundEnd()
    {
        fireworksFX.Play();
        applauseFX.Play();
    }

    public void StopRoundEnd()
    {
        fireworksFX.Stop();
        applauseFX.Stop();
    }
}
