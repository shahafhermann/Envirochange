using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class laserParticle : MonoBehaviour
{
    public bool should_loop = false;
    private float _time;

    private ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        _time = 0f;
        if (should_loop)
        {
            particles.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!should_loop)
        {
            _time += Time.deltaTime;
            if (_time >= 2 * particles.duration)
            {
                particles.Stop();
                particles.Play();
                _time = 0f;
            }
        }
    }
}
