using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// 0 = dash, 1 = death, 2 = nextLevel, 3 = respawn, 4 = magneticField, 5 = menuButton
public class MusicControl : MonoBehaviour {

    public AudioMixerSnapshot[] musicSnapshots;
    public AudioClip[] soundFx;
    public AudioSource fxSource;
    public float bpm = 128f;
    [Range(0, 64)]
    public int transitionTime = 1;

    private float transition;
    private float quarterNote;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        quarterNote = 60 / bpm;
        transition = quarterNote * transitionTime;
    }

    public void transitionTo(int snapshotIndex) {
        musicSnapshots[snapshotIndex].TransitionTo(transition);
    }

    public void playSoundFX(int soundIndex) {
        fxSource.clip = soundFx[soundIndex];
        if (fxSource.isPlaying) {
            fxSource.Stop();
        }
        fxSource.Play();
    }
}
