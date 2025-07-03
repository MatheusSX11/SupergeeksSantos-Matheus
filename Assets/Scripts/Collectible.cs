using UnityEngine;

public class Collectible : MonoBehaviour
{
    public static event System.Action OnCollected;
    public AudioSource sfx;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sfx.Play();
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}

