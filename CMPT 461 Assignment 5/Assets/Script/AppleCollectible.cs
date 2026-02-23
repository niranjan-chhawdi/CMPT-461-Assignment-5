using UnityEngine;

public class AppleCollectible : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        inventory.AddApple(value);
        Destroy(gameObject);
    }
}