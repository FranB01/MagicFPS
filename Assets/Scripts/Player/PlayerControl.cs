using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance { get; private set; }
    public float maxHealth = 100f;
    public float health;
    private bool gameOver = false;
    private bool hasKey = false;

    private UIController uiController;
    private AudioSource audio;

    [SerializeField] private AudioClip pickupKeySound;

    void Start()
    {
        health = maxHealth;
        uiController = GetComponent<UIController>();
        audio = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        // todo
    }

    public void GetHit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameOver();
            return;
        }
        uiController.GetHit((int)(health + 0.5f)); // +.5f so it always rounds up
    }

    private void GameOver()
    {
        gameOver = true;
        GetComponent<PlayerWeaponController>().enabled = false;             // make player unable to shoot
        GetComponent<FirstPersonController>().enabled = false;              // make player unable to move
        Cursor.lockState = CursorLockMode.None;                             // unlock cursor


        Debug.Log("Game Over");
        uiController.GetHit(0);
        uiController.GameOver();

        Time.timeScale = 0;
    }

    public void EndLevel()
    {
        gameOver = true;
        GetComponent<PlayerWeaponController>().enabled = false;             // make player unable to shoot
        GetComponent<FirstPersonController>().enabled = false;              // make player unable to move
        Cursor.lockState = CursorLockMode.None;    
        
        uiController.EndLevel();
        Time.timeScale = 0;
    }

    public void ObtainKey()
    {
        hasKey = true;
        audio.PlayOneShot(pickupKeySound);
    }

    public bool HasKey()
    {
        return hasKey;
    }


}