using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Sincèrement désolé pour ce que je vais faire, mais c'est une jam j'ai pas envie de me casser la tête

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip _lancer;
    [SerializeField] private AudioClip _grab;
    [SerializeField] private AudioClip _QTEWin;
    [SerializeField] private AudioClip _QTELose;
    [SerializeField] private AudioClip _button;
    [SerializeField] private AudioClip _startWave;
    [SerializeField] private AudioClip _waveWin;
    [SerializeField] private AudioClip _decompte;
    [SerializeField] private AudioClip _overButton;
    [SerializeField] private AudioClip _goodKey;
    [SerializeField] private AudioClip _badKey;
    [SerializeField] private AudioClip _phase2;
    [SerializeField] private AudioClip _transition;

    private AudioSource _audioSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayLancer()
    {
        _audioSource.PlayOneShot(_lancer);
    }

    public void PlayGrab()
    {
        _audioSource.PlayOneShot(_grab);
    }

    public void PlayQTEWin()
    {
        _audioSource.PlayOneShot(_QTEWin);
    }

    public void PlayQTELose()
    {
        _audioSource.PlayOneShot(_QTELose);
    }

    public void PlayButton()
    {
        _audioSource.PlayOneShot(_button);
    }

    public void PlayStartWave()
    {
        _audioSource.PlayOneShot(_startWave);
    }

    public void PlayWaveWin()
    {
        _audioSource.PlayOneShot(_waveWin);
    }

    public void PlayDecompte()
    {
        _audioSource.PlayOneShot(_decompte);
    }

    public void PlayOverButton()
    {
        _audioSource.PlayOneShot(_overButton);
    }

    public void PlayGoodKey()
    {
        _audioSource.PlayOneShot(_goodKey);
    }

    public void PlayBadKey()
    {
        _audioSource.PlayOneShot(_badKey);
    }

    public void PlayPhase2()
    {
        _audioSource.PlayOneShot(_phase2);
    }

    public void PlayTransition()
    {
        _audioSource.PlayOneShot(_transition);
    }
}
