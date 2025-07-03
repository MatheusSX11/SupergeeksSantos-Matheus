using UnityEngine;

public class Bounce : MonoBehaviour
{
    public float impulse = 10f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Impulse(impulse);
        }
    }
}