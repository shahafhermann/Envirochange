using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.TextCore;
using DitzeGames.Effects;

public class CreatureBehaviour : MonoBehaviour {
    public GameManager gameManager;
    
    private Rigidbody2D creature_rigid;
    public float jumpHeight = 0f;
    public float groundSpeed = 5f;
    public float midAir = 5f;
    public int MAX_JUMPS_ROW = 2;
    private int num_of_jumps;
    private float turnSpeed;

    private Quaternion originalRotation;

    public float dashSpeed;
    public float dashTime;
    private ParticleSystem dashParticles;
    private bool isDashing;
    public bool allowDashing = false;

    private bool allowMovement = true;

    // private Animator animator;
    // private Vector2 _movement;
    
    // public Transform spawnPosition;
    // private TrailRenderer trail;
    public GameObject trail;

    // Start is called before the first frame update
    void Start() {
        originalRotation = gameObject.transform.rotation;
        
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = 0;
        // animator = GetComponent<Animator>();
        
        // trail = gameObject.GetComponent<TrailRenderer>();
        
        // trail.enabled = false;
        trail.SetActive(false);
        dashParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        dashParticles.Stop();
        isDashing = false;
    }

    private void Update()
    {
        // _movement.x = Input.GetAxisRaw("Horizontal");
        
        turnSpeed = groundSpeed;
        if (num_of_jumps > 0)
        {
            turnSpeed = midAir;
        }
        

        // Animation control
        // animator.SetFloat("Horizontal", _movement.x);
        // animator.SetFloat("Speed", _movement.sqrMagnitude);
    }

    private void FixedUpdate() {
        if (allowMovement) {
            if (Input.GetKey(KeyCode.LeftArrow) && !isDashing)
            {
                creature_rigid.transform.position += -transform.right * (turnSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow) && !isDashing)
            {
                creature_rigid.transform.position += transform.right * (turnSpeed * Time.deltaTime);
            }
        
        
            if (Input.GetKey(KeyCode.Space) && num_of_jumps == 0) {
                //creature_rigid.AddForce(new Vector2(0, jumpHeight * Time.fixedDeltaTime), ForceMode2D.Impulse);
                creature_rigid.velocity = new Vector2(creature_rigid.velocity.x, 0f);
                Vector2 jumpDir = new Vector2((transform.up.x + Vector2.up.x) / 2, (transform.up.y + Vector2.up.y) / 2);
                creature_rigid.AddForce(jumpDir * jumpHeight, ForceMode2D.Impulse);
            }
        

            else if (allowDashing && (num_of_jumps < MAX_JUMPS_ROW) && Input.GetKey(KeyCode.E) && !isDashing)
            {
                StartCoroutine(dashEffect());
            }
        }
    }


    IEnumerator dashEffect()
    {
        isDashing = true;
        num_of_jumps++;  
        dashParticles.Play();
        
        Vector2 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) { direction += Vector2.up; }
        if (Input.GetKey(KeyCode.DownArrow)) { direction += Vector2.down; }
        if (Input.GetKey(KeyCode.LeftArrow)) { direction += Vector2.left; }
        if (Input.GetKey(KeyCode.RightArrow)) { direction += Vector2.right; }

        if (direction == Vector2.zero)
        {
            direction += Vector2.up;
        }
        direction.Normalize();
        // trail.enabled = true;
        trail.SetActive(true);
        creature_rigid.velocity = direction * dashSpeed;
        float originalDrag = creature_rigid.drag;
        float originalAngularDrag = creature_rigid.angularDrag;
        float originalGravity = creature_rigid.gravityScale;
        creature_rigid.drag = 0f;
        creature_rigid.angularDrag = 0f;
        creature_rigid.gravityScale = 0f;
        CameraEffects.ShakeOnce(0.3f, 10f);
        yield return new WaitForSeconds(dashTime);
        
        dashParticles.Stop();
        // trail.enabled = false;
        trail.SetActive(false);
        creature_rigid.velocity = Vector2.zero;
        creature_rigid.drag = originalDrag;
        creature_rigid.angularDrag = originalAngularDrag;
        creature_rigid.gravityScale = originalGravity;
        isDashing = false;
    }
    

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps++;
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goal")) {
            allowMovement = false;
            gameManager.completeLevel();
        }
        
        if (other.gameObject.CompareTag("Bottom"))
        {
            StartCoroutine(respawn());
        }

        if (other.gameObject.CompareTag("Magnet")) {
            magnetizeTo(other.gameObject);
            
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            magnetizeTo(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            Physics2D.gravity = new Vector2(0, -9.8f);
            gameObject.transform.rotation = originalRotation;
        }
    }

    private void magnetizeTo(GameObject other) {
        Vector2 dir = -(transform.position - other.gameObject.transform.position).normalized * 9.8f;
        Physics2D.gravity = dir;
        gameObject.transform.rotation = other.transform.rotation;
    }

    IEnumerator respawn()
    {
        // trail.enabled = false;
        trail.SetActive(false);
        num_of_jumps = 0;
        creature_rigid.transform.position = gameManager.getCurrentLevel().getRespawnPosition() 
                                            + new Vector3(0, 0.5f, 0);
        yield return new WaitForSeconds(0.2f);
        // trail.enabled = false;
        trail.SetActive(false);
    }
}
