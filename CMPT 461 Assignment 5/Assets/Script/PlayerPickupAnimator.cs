using UnityEngine;

public class PlayerPickupAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>(true);

        if (animator == null)
            Debug.LogError("No Animator found on Player or children!");
    }

    public void PlayPickup()
    {
        if (animator == null) return;

        Debug.Log("Pickup trigger fired on Animator: " + animator.gameObject.name);
        animator.ResetTrigger("Pickup");   // helps if spammed
        animator.SetTrigger("Pickup");
    }
}