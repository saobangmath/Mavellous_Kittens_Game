using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the playing of music in the game.
/// </summary>
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

    /// <summary>
    /// Toggles in-game music (on / off).
    /// </summary>
    public void ToggleMusic()
    {
        _audioSource.mute = !_audioSource.mute;
    }
}
