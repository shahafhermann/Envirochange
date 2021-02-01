using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class laserScript : MonoBehaviour
{
    public float delay_time = 0f;
    public float duration;
    public bool should_loop = false;
    
    private Animator laserAnimator;

    private ParticleSystem[] laserParticles;

    private CircleCollider2D[] laserConnectors;

    private float _time;

    private bool finished_delay = false;
    // Start is called before the first frame update
    void Start()
    {
        _time = 0f;
        laserAnimator = gameObject.transform.GetChild(1).GetComponent<Animator>();
        laserConnectors = GetComponentsInChildren<CircleCollider2D>();
        laserParticles = GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < laserParticles.Length; i++)
        {
            ParticleSystem particles = laserParticles[i];
            particles.Stop();
            var main = particles.main;
            main.duration = duration;
            if (should_loop)
            {
                main.loop = enabled;
            }
            particles.Play();
        }
        
        laserAnimator.speed = 4f / (duration * 2);
        laserAnimator.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!should_loop)
        {
            _time += Time.deltaTime;
            if (finished_delay)
            {
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
                        CircleCollider2D collider = laserConnectors[i];
                        collider.tag = "Laser";
                    }

                    _time = 0f;
                }
                else if (_time >= duration && _time < 2 * duration)
                {
                    for (int i = 0; i < laserConnectors.Length; i++)
                    {
                        CircleCollider2D collider = laserConnectors[i];
                        collider.tag = "Untagged";
                    }
                }
            }
            else
            {
                if (_time >= delay_time)
                {
                    finished_delay = true;
                    laserAnimator.enabled = true;
                    _time = 0f;
                }
            }
        }
        
    }
}
