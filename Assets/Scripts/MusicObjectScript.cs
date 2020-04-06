using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MusicObjectScript : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }

    public void ToggleMusic()
    {
        _audioSource.mute = !_audioSource.mute;
    }
}
