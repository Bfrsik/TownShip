using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Unit : MonoBehaviour
{
    private float maxHp = 0;
    private float hp = 0;

    private float speed = 0;
    private float maxSpeed = 0;

    private float maxStamina = 0;
    private float stamina = 0;
    private float staminaRegen = 0;

    private float acceleration = 2000;
    private float decceleration = 2000;

    private float velPower = 1;

    [SerializeField] private Entity race;
    
    [HideInInspector] public Vector2 inputVector = Vector2.zero;
    [HideInInspector] public Collider2D coll;
    [HideInInspector] public Rigidbody2D rb;

    private bool isRunning;
    private bool isMoving;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        maxHp = race.maxHP;
        hp = maxHp;
        speed = race.speed;
        maxSpeed = race.maxSpeed;
        maxStamina = race.stamina;
        stamina = maxStamina;
        rb.mass = race.mass;
        staminaRegen = race.staminaRegeneration;



    }

    private void FixedUpdate()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift);
        isMoving = inputVector != Vector2.zero;

        if (isMoving)
        {
            if (isRunning && stamina > 0)
            {
                Run(inputVector);
                stamina -= Time.fixedDeltaTime;
            }
            else
            {
                Move(inputVector);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (stamina != maxStamina)
            {
                if (stamina < maxStamina)
                {
                    stamina += Time.fixedDeltaTime * staminaRegen;
                }
                else if (stamina > maxStamina)
                {
                    stamina = maxStamina;
                }
            }
        }
        
        inputVector = Vector2.zero;
    }


    private void Move(Vector2 dir)
    {
        //Vector2 dirSpeed = dir * speed;
        //rb.velocity = dirSpeed;

        dir = dir.normalized;
        Vector2 targetVecSpeed = dir * speed;
        Vector2 speedDif = targetVecSpeed - rb.velocity;
        Vector2 acceleRate;
        acceleRate.x = (Mathf.Abs(targetVecSpeed.x) > 0.01f) ? acceleration : decceleration;
        acceleRate.y = (Mathf.Abs(targetVecSpeed.y) > 0.01f) ? acceleration : decceleration;
        Vector2 movement;
        movement.x = speedDif.x * acceleRate.x;
        movement.y = speedDif.y * acceleRate.y;
        rb.AddForce(movement * Vector2.one);


        // float targetSpeed = dir * speed;
        // float speedDif = targetSpeed - rb.velocity.x;
        // float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        // float movement = Mathf.Pow(Math.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        // if (jumpCheck.isJumping)
        // {
        //     rb.AddForce(movement * Vector2.right * 0.5f);
        // }
    }

    private void Run(Vector2 dir)
    {
        dir = dir.normalized;
        Vector2 dirSpeed = dir * maxSpeed;
        rb.velocity = dirSpeed;
    }
}