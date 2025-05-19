using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 5.0f;
    public float lifeTime = 5.0f;
    public float damage = 20.0f;
    public AudioClip impactSound;
    public AudioClip hitSound;
    
    private float timeCreated;
    private PlayerControl _playerControl;
    private UIController _uiController;
    
    // Start is called before the first frame update
    void Start()
    {
        timeCreated = Time.time;
        _playerControl = FindObjectOfType<PlayerControl>();
        _uiController = FindObjectOfType<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * speed));

        // Destroy after lifetime ends
        if (Time.time - timeCreated >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
        switch (other.gameObject.tag)
        {
            case "Enemy":
            {
                Debug.Log("Hit enemy!!!");
                OnEnemyHit(other.gameObject);
                AfterHit();
                break;
            }
            case "Player":
            {
                OnPlayerHit(other.gameObject);
                AfterHit();
                break;
            }
            default:
            {
                AfterHit();
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    protected virtual void OnEnemyHit(GameObject enemy)
    {
        // gets enemy script from collider or collider's father (for enemies with collider in children)
        var enemyController = enemy.GetComponent<EnemyController>() ?? enemy.GetComponentInParent<EnemyController>();
        enemyController.TakeDamage(damage);
        _playerControl.GetComponent<AudioSource>().PlayOneShot(hitSound);
        _uiController.Hitmarker();
    }

    protected virtual void OnPlayerHit(GameObject player)
    {
        player.GetComponent<PlayerControl>().GetHit(damage);
    }

    protected virtual void OnOtherHit()
    {
    }

    // What should happen when the projectile hits ANYTHING, applied after other effects
    protected virtual void AfterHit()
    {
        Destroy(gameObject);
    }
}