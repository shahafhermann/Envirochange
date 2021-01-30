using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserParticle : MonoBehaviour
{
    private float _time;

    private ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        _time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 2 * particles.duration)
        {
            Debug.Log(_time);
            particles.Stop();
            particles.Play();
            _time = 0f;
        }
    }
}
