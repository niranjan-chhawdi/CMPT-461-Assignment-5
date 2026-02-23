using UnityEngine;
using TMPro;
using System.Collections;

public class GiraffeFeeder : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode feedKey = KeyCode.E;
    public int applesRequired = 4;

    [Header("UI")]
    public TextMeshProUGUI feedPromptText; 

    [Header("Audio")]
    public AudioSource audioSource;        
    public AudioClip feedSound;            

    [Header("Optional")]
    public GameObject fedEffect;

    private bool playerInRange = false;
    private bool alreadyFed = false;
    private PlayerInventory playerInventory;

    private void Start()
    {
        if (feedPromptText != null)
            feedPromptText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (alreadyFed) return;
        if (!playerInRange) return;
        if (playerInventory == null) return;

        // Show prompt only if enough apples
        bool canFeed = playerInventory.apples >= applesRequired;
        if (feedPromptText != null)
            feedPromptText.gameObject.SetActive(canFeed);

        // Press E to feed
        if (canFeed && Input.GetKeyDown(feedKey))
        {
            playerInventory.ConsumeApples(applesRequired);

            if (audioSource != null && feedSound != null)
                audioSource.PlayOneShot(feedSound);
                StartCoroutine(StopAfterSeconds(2f));

            if (fedEffect != null)
                Instantiate(fedEffect, transform.position + Vector3.up * 2f, Quaternion.identity);

            alreadyFed = true;

            if (feedPromptText != null)
                feedPromptText.gameObject.SetActive(false);

            Debug.Log("Giraffe fed! 🦒");
        }
    }

IEnumerator StopAfterSeconds(float seconds)
{
    yield return new WaitForSeconds(seconds);
    audioSource.Stop();
}
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        playerInventory = other.GetComponent<PlayerInventory>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        playerInventory = null;

        if (feedPromptText != null)
            feedPromptText.gameObject.SetActive(false);
    }
}