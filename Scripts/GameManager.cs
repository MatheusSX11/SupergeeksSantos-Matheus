using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}

    public List<Transform> spawns;
    public Transform debugPosition;
    public Transform lastSpawnPosition;
    public bool debug = false;
    
    public int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Collectible.OnCollected += HandleCollectibleCollected;
    }

    void Start()
    {
        GameObject player = FindObjectOfType<PlayerMovement>().gameObject;

        if (!debug)
        {
            player.transform.position = spawns[0].position;
            if (lastSpawnPosition != null)
            {
                player.transform.position = lastSpawnPosition.position;
            }
        }
        else
            player.transform.position = debugPosition.position;
    }

    public Transform GetLastPlayerTransform()
    {
        if (lastSpawnPosition != null)
            return lastSpawnPosition;
        else
            return spawns[0];
    }
    
    void OnDestroy()
    {
        Collectible.OnCollected -= HandleCollectibleCollected;
    }

    private void HandleCollectibleCollected(){

        score++;
        Debug.Log("pontuação: "+ score);
    }
}