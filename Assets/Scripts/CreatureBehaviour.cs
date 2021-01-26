using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.TextCore;
using DitzeGames.Effects;
using Kino;
using UnityEngine.InputSystem;

public class CreatureBehaviour : MonoBehaviour {
    private GameManager gameManager;
    
    private Rigidbody2D creature_rigid;
    public float jumpHeight = 0f;
    public float groundSpeed = 5f;
    public float midAir = 5f;
    public int MAX_JUMPS_ROW = 2;
    private int num_of_jumps;
    private float turnSpeed;

    private bool isRespawning = false; 

    private Quaternion originalRotation;

    public float dashSpeed;
    public float dashTime;
    private ParticleSystem dashParticles;
    private bool isDashing;
    public bool allowDashing = false;

    private bool allowMovement = true;
    
    private bool last_resort_jump = true;

    private PlayerInput  controls;
    
    public GameObject trail;
    
    private Transform eyeChild;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        controls = new PlayerInput();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Start is called before the first frame update
    void Start() {
        originalRotation = gameObject.transform.rotation;
        
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = 0;
        
        dashParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        dashParticles.Stop();
        isDashing = false;
        eyeChild = gameObject.transform.GetChild(3);
    }

    private void Update()
    {
        turnSpeed = groundSpeed;
        if (num_of_jumps > 0)
        {
            turnSpeed = midAir;
        }
        
        if (allowMovement) {
            float x_movementInput = controls.Creature.movement.ReadValue<Vector2>().x;

            if (x_movementInput < 0f && !isDashing)
            {
                creature_rigid.transform.position += -transform.right * (turnSpeed * Time.deltaTime);
                eyeChild.transform.Rotate(0,0,turnSpeed );
            }
            if (x_movementInput > 0f && !isDashing)
            {
                creature_rigid.transform.position += transform.right * (turnSpeed * Time.deltaTime);
                eyeChild.transform.Rotate(0,0,-turnSpeed );
            }
            
            
            if (controls.Creature.jump.triggered && (num_of_jumps == 0 || last_resort_jump))
            {
                float jump_power = 1f;
                if (last_resort_jump)
                {
                    jump_power = 0.35f;
                    last_resort_jump = false;
                }
                _jump(jump_power);
            }
            
            else if (allowDashing && (num_of_jumps < MAX_JUMPS_ROW) && controls.Creature.dash.triggered && !isDashing)
            {
                StartCoroutine(dashEffect());
            }
        }
    }

    private void _jump(float jump_power)
    {
        creature_rigid.velocity = new Vector2(creature_rigid.velocity.x, 0f);
        Vector2 jumpDir = new Vector2((transform.up.x + Vector2.up.x) / 2, (transform.up.y + Vector2.up.y) / 2);
        creature_rigid.AddForce(jumpDir * (jumpHeight * jump_power), ForceMode2D.Impulse);
    }

    IEnumerator dashEffect()
    {
        isDashing = true;
        num_of_jumps++;  
        dashParticles.Play();
        
        Vector2 direction = Vector3.zero;
        direction += controls.Creature.movement.ReadValue<Vector2>();

        if (direction == Vector2.zero)
        {
            direction += Vector2.up;
        }
        direction.Normalize();
        trail.SetActive(true);
        creature_rigid.velocity = direction * dashSpeed;
        float originalDrag = creature_rigid.drag;
        float originalAngularDrag = creature_rigid.angularDrag;
        float originalGravity = creature_rigid.gravityScale;
        creature_rigid.drag = 0f;
        creature_rigid.angularDrag = 0f;
        creature_rigid.gravityScale = 0f;
        CameraEffects.ShakeOnce(0.3f, 10f);
        gameManager.playSound(0);
        yield return new WaitForSeconds(dashTime);
        
        dashParticles.Stop();
        trail.SetActive(false);
        creature_rigid.velocity = Vector2.zero;
        creature_rigid.drag = originalDrag;
        creature_rigid.angularDrag = originalAngularDrag;
        creature_rigid.gravityScale = originalGravity;
        isDashing = false;
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps = 0;
            last_resort_jump = false;
        }
        
        if (other.gameObject.CompareTag("Goal")) {
            allowMovement = false;
            gameManager.completeLevel();
        }
        
        if (other.gameObject.CompareTag("Bottom") || other.gameObject.CompareTag("Laser")) {
            if (!isRespawning) {
                isRespawning = true;
                StartCoroutine(respawn());
            }
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
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps++;
            last_resort_jump = true;
        }
    }

    private void magnetizeTo(GameObject other) {
        Vector2 dir = -(transform.position - other.gameObject.transform.position).normalized * 9.8f;
        Physics2D.gravity = dir;
    }

    IEnumerator respawn()
    {
        gameManager.playSound(1);

        gameManager.getCurrentLevel().getRespawnAnimator().SetTrigger("Respawn");
        gameManager.apply_death_effect();

        yield return new WaitForSeconds(1.2f);
        gameManager.playSound(3);
        yield return new WaitForSeconds(0.28f);

        trail.SetActive(false);
        num_of_jumps = 0;
        creature_rigid.transform.position = gameManager.getCurrentLevel().getRespawnPosition() 
                                            + new Vector3(0, 0.25f, 0);
        
        
        trail.SetActive(true);

        isRespawning = false;
    }
}
