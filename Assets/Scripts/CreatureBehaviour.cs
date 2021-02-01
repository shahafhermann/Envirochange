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

    private PlayerInput controls;
    
    public GameObject trail;
    
    private Transform eyeChild;
    private SpriteRenderer eyeSprite;
    private Animator eye_animator;
    private ParticleSystem double_jump_particle;

    private float curMagnetForce = 100f;
    private bool magnetDown = false;
    private bool jumped = false;

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
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        originalRotation = gameObject.transform.rotation;
        
        creature_rigid = GetComponent<Rigidbody2D>();
        num_of_jumps = 0;
        
        dashParticles = gameObject.GetComponentInChildren<ParticleSystem>();
        dashParticles.Stop();
        isDashing = false;
        eyeChild = gameObject.transform.GetChild(3);
        eye_animator = eyeChild.GetComponent<Animator>();
        eyeSprite = eyeChild.GetComponent<SpriteRenderer>();
        double_jump_particle = gameObject.transform.GetChild(4).GetComponent<ParticleSystem>();
        double_jump_particle.Stop();
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
                if (creature_rigid.velocity.x > 0)
                {
                    creature_rigid.velocity = new Vector2(0f, creature_rigid.velocity.y);
                }

                eye_animator.enabled = false;
                creature_rigid.transform.position += -transform.right * (turnSpeed * Time.deltaTime);
                eyeChild.transform.Rotate(0,0,turnSpeed );
            }
            if (x_movementInput > 0f && !isDashing)
            {
                if (creature_rigid.velocity.x < 0)
                {
                    creature_rigid.velocity = new Vector2(0f, creature_rigid.velocity.y);
                }
                creature_rigid.velocity = new Vector2(0f, creature_rigid.velocity.y);
                eye_animator.enabled = false;
                creature_rigid.transform.position += transform.right * (turnSpeed * Time.deltaTime);
                eyeChild.transform.Rotate(0,0,-turnSpeed );
            }
            if (x_movementInput == 0f)
            {
                eye_animator.enabled = true;
            }
            
            
            if (controls.Creature.jump.triggered && (num_of_jumps == 0 || last_resort_jump))
            {
                float jump_power = 1f;
                if (last_resort_jump)
                {
                    StartCoroutine(apply_double_jump_particle());
                    jump_power = 0.35f;
                    last_resort_jump = false;
                }
                _jump(jump_power);
                jumped = true;
            }
            
            else if (allowDashing && (num_of_jumps < MAX_JUMPS_ROW) && controls.Creature.dash.triggered && !isDashing)
            {
                jumped = true;
                StartCoroutine(dashEffect());
            }
            
            
        }

    }
    
    IEnumerator apply_double_jump_particle()
    {
        double_jump_particle.Play();
        yield return new WaitForSeconds(11f);
        double_jump_particle.Stop();
    }

    private void _jump(float jump_power)
    {
        creature_rigid.velocity = new Vector2(creature_rigid.velocity.x, 0f);
        Vector2 jumpDir = new Vector2((transform.up.x + Vector2.up.x) / 2, (transform.up.y + Vector2.up.y) / 2);

        if (magnetDown) {
            jumpDir = new Vector2(controls.Creature.movement.ReadValue<Vector2>().x, 
                controls.Creature.movement.ReadValue<Vector2>().y);
        }
        creature_rigid.AddForce(jumpDir * (jumpHeight * jump_power), ForceMode2D.Impulse);
    }

    IEnumerator dashEffect()
    {
        isDashing = true;
        num_of_jumps++;  
        dashParticles.Play();
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(.5f, .5f);
        }
        
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
        creature_rigid.drag = 0.0001f;
        creature_rigid.angularDrag = 0f;
        creature_rigid.gravityScale = 0f;
        CameraEffects.ShakeOnce(0.3f, 10f);
        gameManager.playSound(MusicControl.SoundFX.Dash);
        yield return new WaitForSeconds(dashTime * (0.75f));
        creature_rigid.velocity = direction * (dashSpeed * .5f);
        creature_rigid.drag = 0.01f;
        yield return new WaitForSeconds(dashTime * (0.25f));
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        
        
        dashParticles.Stop();
        trail.SetActive(false);
        creature_rigid.velocity = Vector2.zero;
        creature_rigid.drag = originalDrag;
        creature_rigid.angularDrag = originalAngularDrag;
        creature_rigid.gravityScale = originalGravity;
        isDashing = false;
        
        // if the creature didn't leave the platform than restore the jump
        if (direction.y < 0 && num_of_jumps == 1)
        {
            num_of_jumps--;
        }
        
    }
    
    IEnumerator dashEffect_option2()
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

        float originalGravity = creature_rigid.gravityScale;
        float originalDrag = creature_rigid.drag;
        if (direction.x != 0)
        {
            creature_rigid.gravityScale = Single.Epsilon;
            creature_rigid.drag *= 3f;
        }
        creature_rigid.velocity = Vector2.zero;
        creature_rigid.AddForce(direction * (dashSpeed + 40f), ForceMode2D.Impulse);
        
        CameraEffects.ShakeOnce(0.3f, 10f);
        gameManager.playSound(MusicControl.SoundFX.Dash);
        yield return new WaitForSeconds(dashTime);
        creature_rigid.gravityScale = originalGravity;
        creature_rigid.drag = originalDrag;

        dashParticles.Stop();
        trail.SetActive(false);
        isDashing = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Laser")) {
            if (!isRespawning) {
                isRespawning = true;
                StartCoroutine(respawn(other.gameObject.tag));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Platform"))
        {
            num_of_jumps = 0;
            last_resort_jump = false;
            jumped = false;
        }
        
        if (other.gameObject.CompareTag("Goal")) {
            if (!isRespawning) {
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(0.1f, 0.1f);
                }
                allowMovement = false;
                eye_animator.enabled = true;
                eye_animator.SetBool("nextLevel", true);
                gameManager.completeLevel();
            }
        }
        
        if (other.gameObject.CompareTag("Bottom") || other.gameObject.CompareTag("Laser")) {
            if (!isRespawning) {
                isRespawning = true;
                StartCoroutine(respawn(other.gameObject.tag));
            }
        }

        if (other.gameObject.CompareTag("Magnet")) {
            float zRot = other.gameObject.transform.rotation.eulerAngles.z;
            magnetDown = zRot > 90f && zRot < 270f;
            creature_rigid.velocity *= 0.2f;
            
            magnetizeTo(other.gameObject, true);
            
            gameManager.playSound(MusicControl.SoundFX.MagneticField);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            magnetizeTo(other.gameObject, false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Magnet")) {
            Physics2D.gravity = new Vector2(0, -9.8f);
            gameObject.transform.rotation = originalRotation;
            
            gameManager.playSound(MusicControl.SoundFX.MagneticField);
        }
        if (other.gameObject.CompareTag("Platform"))
        {
            if (jumped){StartCoroutine(coyote_time(0f));}
            else {StartCoroutine(coyote_time(.1f));}
            
        }

        if (other.gameObject.CompareTag("Magnet")) {
            magnetDown = false;
        }
    }
    
    IEnumerator coyote_time(float duration)
    {
        yield return new WaitForSeconds(duration);
        num_of_jumps++;
        last_resort_jump = true;
    }

    private void magnetizeTo(GameObject other, bool initial) {
        if (initial) {
            curMagnetForce = 100f;
        }
        else if (curMagnetForce > 85f) {
            curMagnetForce -= 1f;
        }
        else {
            curMagnetForce = 13f;
        }
        Vector2 dir = -(transform.position - other.gameObject.transform.position).normalized * curMagnetForce;
        Physics2D.gravity = dir;
    }

    IEnumerator respawn(String tag)
    {
        gameManager.playSound(MusicControl.SoundFX.Death);
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(1.0f, 1.0f);
        }

        gameManager.getCurrentLevel().getRespawnAnimator().SetTrigger("Respawn");
        gameManager.apply_death_effect();
        creature_rigid.velocity = Vector2.zero;
        if (tag == "Bottom") {
            yield return new WaitForSeconds(1.2f);
        }

        eyeSprite.enabled = false;
        gameManager.playSound(MusicControl.SoundFX.Respawn);
        yield return new WaitForSeconds(0.28f);

        trail.SetActive(false);
        num_of_jumps = 0;
        eyeSprite.enabled = true;
        creature_rigid.transform.position = gameManager.getCurrentLevel().getRespawnPosition() 
                                            + new Vector3(0, 0.25f, 0);
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        
        
        trail.SetActive(true);

        isRespawning = false;
    }
}
