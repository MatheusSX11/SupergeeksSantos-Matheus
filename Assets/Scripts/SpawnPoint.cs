using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.lastSpawnPosition = transform;
            Debug.Log("Progresso salvo! "+ transform.position);
        }
    }
}