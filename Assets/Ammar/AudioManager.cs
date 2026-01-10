using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _atmosphere;
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioSource _footSteps;
    [SerializeField] AudioClip[] _musicClips;
    [SerializeField] AudioClip[] _atmosphereClips;
    [SerializeField] AudioClip[] _sfxClips;
    private int _musicIndex;
    private int _atmosphereIndex;
    private int _sfxIndex;
    public static AudioManager instance;

    public enum soundEffect
    {
        BURNING,
        CAMEL,
        FALCON,
        GAMEEND,
        FOOTSTEP,
        SNAKE,
        ZOOM,
        EAT,
        DRINK,
        QUICKSAND
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        _musicIndex = 0;
        _atmosphereIndex = 0;
        _sfxIndex = 0;
        _music.clip = _musicClips[_musicIndex];
        _music.Play();
        _atmosphere.clip = _atmosphereClips[_atmosphereIndex];
        _atmosphere.Play();
    }

    public void playSFX(soundEffect sound, bool play)
    {
        if (play)
        {
            _sfx.clip = _sfxClips[(int)sound];
            _sfx.Play();
        }
        else
            _sfx.Stop();
    }

    public void playFootstep(soundEffect sound)
    {
            _footSteps.clip = _sfxClips[(int)sound];
            _footSteps.Play();
    }

    public void randomizePitch()
    {
        _sfx.pitch = Random.Range(0.8f, 1.2f);
    }

    public void normalizePitch()
    {
        _sfx.pitch = 1f;
    }
    
    public void randomizeFootPitch()
    {
        _footSteps.pitch = Random.Range(0.8f, 1.2f);
    }

}
