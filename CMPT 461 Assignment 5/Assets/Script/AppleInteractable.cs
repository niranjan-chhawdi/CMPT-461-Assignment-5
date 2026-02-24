using UnityEngine;
using System.Collections;

public class AppleInteractable : MonoBehaviour
{
    public int value = 1;
    private bool pickedUp = false;

    public IEnumerator PickupAfterDelay(PlayerInventory inventory, float delay)
    {
        if (pickedUp) yield break;
        pickedUp = true;

        yield return new WaitForSeconds(delay);

        if (inventory != null)
            inventory.AddApple(value);

        Destroy(gameObject);
    }
}