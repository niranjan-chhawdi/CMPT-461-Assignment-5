using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.F;
    public float pickupDelay = 0.55f;

    public GameObject pickupPrompt;   

    private AppleInteractable currentApple;
    private PlayerInventory inventory;
    private PlayerPickupAnimator pickupAnim;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        pickupAnim = GetComponent<PlayerPickupAnimator>();

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }

    private void Update()
    {
        if (currentApple == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            if (pickupAnim != null)
                pickupAnim.PlayPickup();

            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);

            StartCoroutine(currentApple.PickupAfterDelay(inventory, pickupDelay));
            currentApple = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var apple = other.GetComponent<AppleInteractable>();
        if (apple != null)
        {
            currentApple = apple;

            if (pickupPrompt != null)
                pickupPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var apple = other.GetComponent<AppleInteractable>();
        if (apple != null && apple == currentApple)
        {
            currentApple = null;

            if (pickupPrompt != null)
                pickupPrompt.SetActive(false);
        }
    }
}