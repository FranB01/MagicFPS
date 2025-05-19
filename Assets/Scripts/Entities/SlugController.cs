using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugController : EnemyController
{
    [SerializeField] private AudioClip castSound;
    [SerializeField] private ProjectileController projectilePrefab;
    [SerializeField] private Transform castOrigin;

    protected override void Attack()
    {
        if (Time.time - lastTimeAttacked > cooldown)
        {
            Shoot();
            lastTimeAttacked = Time.time;
        }
    }

    private void Shoot()
    {
        audioSource.PlayOneShot(castSound);
        // Vector3 directionToTarget = target.position - transform.position;
        castOrigin.LookAt(player.transform);
        ProjectileController projectile = Instantiate(projectilePrefab, castOrigin.position, castOrigin.rotation);
    }
}
