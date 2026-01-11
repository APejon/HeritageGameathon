using System.Collections;
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
    private Coroutine _musicCoroutine;
    private Coroutine _atmosphereCoroutine;
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
        _music.clip = _musicClips[_musicIndex];
        _music.Play();
        _musicCoroutine = StartCoroutine(ChangeMusic());
        _atmosphere.clip = _atmosphereClips[_atmosphereIndex];
        _atmosphere.Play();
        _atmosphereCoroutine = StartCoroutine(ChangeAtmosphere());
    }

    IEnumerator ChangeMusic()
    {
        yield return new WaitForSeconds(_musicClips[_musicIndex].length);
        _musicIndex++;
        if (_musicIndex > _musicClips.Length - 2)
            _musicIndex = 0;
        _music.clip = _musicClips[_musicIndex];
        _music.Play();
        yield return ChangeMusic();
    }

    IEnumerator ChangeAtmosphere()
    {
        yield return new WaitForSeconds(_atmosphereClips[_atmosphereIndex].length);
        _atmosphereIndex++;
        if (_atmosphereIndex > _atmosphereClips.Length - 1)
            _atmosphereIndex = 0;
        _atmosphere.clip = _atmosphereClips[_atmosphereIndex];
        _atmosphere.Play();
        yield return ChangeAtmosphere();
    }

    public void playGameOver()
    {
        StopCoroutine(_musicCoroutine);
        _music.clip = _musicClips[2];
        _music.Play();
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

    public void stopAllSources()
    {
        StopCoroutine(_musicCoroutine);
        StopCoroutine(_atmosphereCoroutine);
        _music.Stop();
        _atmosphere.Stop();
        _sfx.Stop();
        _footSteps.Stop();
    }
}
