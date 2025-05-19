using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    WandController[] wands;

    // index of the current wand
    private int wandIndex;


    void Start()
    {
        // TODO remove later!
        wands = GetComponentsInChildren<WandController>();
    }

    void Update()
    {
        wands[wandIndex].ShootInput(Input.GetButton("Fire1"));
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            wandIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wandIndex = 1;
        }

        // set current wand as visible
        for (int i = 0; i < wands.Length; i++)
        {
            wands[i].model.SetActive(i == wandIndex);
        }
        
    }
}