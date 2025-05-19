using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WandShootType
{
    Automatic,
    Charge,
}

public class WandController : MonoBehaviour
{
    // shooting type
    public WandShootType ShootType;

    // wand stats (general)
    public float delayBetweenShots = 0.5f;

    // wand stats (automatic shooting)
    public float manaCost = 5f;
    public float manaRegenerationRate = 20f;
    private float maxMana = 100f;
    private float currentMana = 0f;

    // wand stats (charge shot)
    [Tooltip("Percentage charge to add per second")]
    public float chargingRate = 100f;

    private float maxCharge = 100f;
    private float charge = 0f;
    private float chargeDecreaseRate = 200f;

    // audio
    public AudioClip castSound;

    // projectile
    public ProjectileController projectilePrefab;

    // last time it has shot
    private float lastShotTime = Mathf.NegativeInfinity;

    // Position
    public Transform castOrigin;

    // Camera for shooting direction calc
    private FirstPersonController firstPersonController;

    // Components
    private AudioSource audioSource;
    private UIController uiController;
    public GameObject model;

    private void Awake()
    {
        audioSource = GetComponentInParent<AudioSource>();
        firstPersonController = FindObjectOfType<FirstPersonController>();
        uiController = FindObjectOfType<UIController>();

        currentMana = maxMana;
    }

    private void Update()
    {
    }

    public void ShootInput(bool inputHeld)
    {
        if (inputHeld)
        {
            switch (ShootType)
            {
                case WandShootType.Automatic:
                {
                    if (Time.time - lastShotTime > delayBetweenShots && currentMana >= manaCost)
                    {
                        Shoot();
                    }

                    break;
                }

                case WandShootType.Charge:
                {
                    if (Time.time - lastShotTime > delayBetweenShots)
                    {
                        charge += chargingRate * Time.deltaTime;
                        if (charge > maxCharge)
                        {
                            charge = 0f;
                            Shoot();
                        }
                    }

                    uiController.SetMana((int)currentMana);
                    break;
                }
            }
        }
        else
        {
            switch (ShootType)
            {
                case WandShootType.Automatic:
                {
                    if (Time.time - lastShotTime > delayBetweenShots)
                    {
                        currentMana = Mathf.Clamp(currentMana + manaRegenerationRate * Time.deltaTime, 0, maxMana);
                    }

                    break;
                }
                case WandShootType.Charge:
                {
                    charge = Mathf.Clamp(charge - chargeDecreaseRate * Time.deltaTime, 0f, maxCharge);
                    break;
                }
            }
        }

        // crappy code (check if its active)
        if (model.activeSelf)
        {
            if (ShootType == WandShootType.Automatic)
            {
                uiController.SetMana((int)currentMana);
            }
            else
            {
                uiController.SetMana((int)charge);
            }
        }
    }

    private void Shoot()
    {
        lastShotTime = Time.time;
        currentMana -= manaCost;

        audioSource.PlayOneShot(castSound);
        ProjectileController projectile = Instantiate(projectilePrefab, castOrigin.position, castOrigin.rotation);
        // TODO 
    }
}