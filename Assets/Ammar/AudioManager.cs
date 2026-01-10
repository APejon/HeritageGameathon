using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _music;
    [SerializeField] AudioSource _atmosphere;
    [SerializeField] AudioSource _sfx;
    [SerializeField] AudioClip[] _musicClips;
    [SerializeField] AudioClip[] _atmosphereClips;
    [SerializeField] AudioClip[] _sfxClips;
    private int _musicIndex;
    private int _atmosphereIndex;
    private int _sfxIndex;

    void Start()
    {
        _musicIndex = 0;

    }
}
