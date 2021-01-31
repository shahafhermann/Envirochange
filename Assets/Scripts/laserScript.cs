using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class laserScript : MonoBehaviour
{
    public float duration;
    public bool should_loop = false;
    
    private Animator laserAnimator;

    private ParticleSystem[] laserParticles;

    private Collider2D[] laserConnectors;

    private float _time;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0f;
        laserAnimator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        laserConnectors = GetComponentsInChildren<Collider2D>();
        laserParticles = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < laserParticles.Length; i++)
        {
            ParticleSystem particles = laserParticles[i];
            particles.Stop();
            var main = particles.main;
            main.duration = duration;
            if (should_loop)
            {
                particles.loop = enabled;
            }
            particles.Play();
            Debug.Log(particles.duration);
        }
        
        laserAnimator.speed = 4f / (duration * 2);

        if (should_loop)
        {
            laserAnimator.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!should_loop)
        {
            _time += Time.deltaTime;
            if (_time >= 2 * duration)
            {
                for (int i = 0; i < laserParticles.Length; i++)
                {
                    ParticleSystem particles = laserParticles[i];
                    particles.Stop();
                    particles.Play();
                }
                for (int i = 0; i < laserConnectors.Length; i++)
                {
                    Collider2D collider = laserConnectors[i];
                    collider.tag =  "Laser";
                }
                _time = 0f;
            }
            else if (_time >= duration && _time < 2 * duration)
            {
                for (int i = 0; i < laserConnectors.Length; i++)
                {
                    Collider2D collider = laserConnectors[i];
                    collider.tag =  "Untagged";
                }
            }
        }
        
    }
}
