using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyControl : MonoBehaviour
{
    private const float rotationSpeed = .3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            PlayerControl.Instance.ObtainKey();
            foreach (var levelEnd in FindObjectsOfType<LevelEnd>())
            {
                levelEnd.RemoveText();
            }

            Destroy(gameObject);
        }
    }
}
