using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si tiene llave
            if (PlayerControl.Instance.HasKey())
            {
                PlayerControl.Instance.EndLevel();
            }
        }
    }

    public void RemoveText()
    {
        // can't disable??? so just blank text
        GetComponentInChildren<TextMeshPro>().text = "";
        GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
    
}