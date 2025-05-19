using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonControl : EnemyController
{
    public int damage = 30;
    public float attackDelay = .5f;
    public float attackRange = 2f;
    
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        Animate();
    }

    protected override void Attack()
    {
        lastTimeAttacked = Time.time;
        StartCoroutine(AttackAfterDelay());

        state = EnemyState.CoolDown;
    }

    protected override void Cooldown()
    {
        base.Cooldown();
        agent.SetDestination(transform.position);
    }

    private IEnumerator AttackAfterDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
        {
            player.GetHit(damage);
        }
    }

    private void Animate()
    {
        int animState = 0;
        switch (state)
        {
            case EnemyState.Idle:
            case EnemyState.Chasing:
            case EnemyState.Attacking:
                animState = (int)state;
                break;
            case EnemyState.CoolDown:
                animState = 2;
                break;
        }

        animator.SetInteger("AnimState", animState);
    }
}