using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// 0 = dash, 1 = death, 2 = nextLevel, 3 = respawn, 4 = magneticField, 5 = menuButton
public class MusicControl : MonoBehaviour {

    public enum SoundFX {
        Dash,
        Death,
        CompleteLevel,
        Respawn,
        MagneticField,
        MenuButton
    }

    public static int currentSnapshot = 0;

    public AudioMixerSnapshot[] musicSnapshots;
    public AudioClip[] soundFx;
    public AudioSource fxSource;
    public float bpm = 128f;
    [Range(0, 64)]
    public int transitionTime = 1;

    private float transition;
    private float quarterNote;

    private AudioSource[] source;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        quarterNote = 60 / bpm;
        transition = quarterNote * transitionTime;
        Debug.Log(gameObject.transform.GetChild(1));
        source = gameObject.transform.GetChild(1).GetComponents<AudioSource>();
        Debug.Log(source.Length);
    }

    public void transitionTo(int snapshotIndex) {
        source[currentSnapshot].Stop();
        AudioMixerSnapshot snapshot = musicSnapshots[snapshotIndex];
        snapshot.TransitionTo(transition);
        currentSnapshot = snapshotIndex;
        StartCoroutine(_transition());
    }

    IEnumerator _transition()
    {
        yield return new WaitForSeconds(transition);
        source[currentSnapshot].Stop();
        source[currentSnapshot].Play();
    }

    public void playSoundFX(SoundFX sound) {
        switch (sound) {
            case SoundFX.Dash:
                fxSource.clip = soundFx[0];
                break;
            case SoundFX.Death:
                fxSource.clip = soundFx[1];
                break;
            case SoundFX.CompleteLevel:
                fxSource.clip = soundFx[2];
                break;
            case SoundFX.Respawn:
                fxSource.clip = soundFx[3];
                break;
            case SoundFX.MagneticField:
                fxSource.clip = soundFx[4];
                break;
            case SoundFX.MenuButton:
                fxSource.clip = soundFx[5];
                break;
        }
        if (fxSource.isPlaying) {
            fxSource.Stop();
        }
        fxSource.Play();
    }
}
