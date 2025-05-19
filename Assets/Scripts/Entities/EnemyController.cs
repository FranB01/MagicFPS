using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum EnemyState
{
    Idle,
    Chasing,
    Attacking,
    CoolDown,
}

public abstract class EnemyController : MonoBehaviour
{
    // Stats
    public float maxHp = 100f;
    protected float hp;
    public float cooldown = 1f;
    protected float lastTimeAttacked = Mathf.NegativeInfinity;

    // Behaviour
    [Tooltip("Does this enemy chase the player down?")]
    public bool chasesPlayer;
    
    [Tooltip("At what distance does the enemy start chasing the player?")]
    public float chaseStateRange;
    
    [Tooltip("At what distance does the enemy start attacking the player after noticing them?")]
    public float attackStateRange = 999f;

    [Tooltip("Should this enemy notice the player when attacked?")]
    public bool aggroWhenAttacked = true;

    // current enemy state
    protected EnemyState state = EnemyState.Idle;

    // Components
    protected PlayerControl player;
    protected NavMeshAgent agent;
    protected AudioSource audioSource;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        hp = maxHp;
        player = PlayerControl.Instance;
        if (chasesPlayer)
        {
            agent = GetComponent<NavMeshAgent>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        switch (state)
        {
            case EnemyState.Idle:
            {
                if (Vector3.Distance(transform.position, player.transform.position) < chaseStateRange)
                {
                    PlayerNoticed();
                }

                break;
            }
            case EnemyState.Chasing:
            {
                Chase();
                break;
            }
            case EnemyState.Attacking:
            {
                Attack();
                break;
            }
            case EnemyState.CoolDown:
            {
                Cooldown();
                break;
            }
        }
    }

    private void PlayerNoticed()
    {
        // Changes state depending on if the enemy should chase the player
        state = chasesPlayer ? EnemyState.Chasing : EnemyState.Attacking;
    }
    
    private void Chase()
    {
        agent.SetDestination(player.transform.position);
        if (Vector3.Distance(transform.position, player.transform.position) < attackStateRange)
        {
            state = EnemyState.Attacking;
        }
    }
    
    protected abstract void Attack();
    
    public virtual void GetHit(ProjectileController projectile)
    {
        if (projectile.CompareTag("PlayerProjectile"))
        {
            TakeDamage(projectile.damage);
        }
    }

    protected virtual void Cooldown()
    {
        if (Time.time > lastTimeAttacked + cooldown)
        {
            state = EnemyState.Chasing;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
            // todo cambiar?
        }
        
        if (aggroWhenAttacked)
        {
            PlayerNoticed();
        }
    }
    
}