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

    [Header("Yummy Text")]
    public GameObject yummyText;        
    public float yummyDuration = 2f;    

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip feedSound;
    public float feedSoundSeconds = 2f; 

    [Header("Optional")]
    public GameObject fedEffect;

    private bool playerInRange = false;
    private bool alreadyFed = false;
    private PlayerInventory playerInventory;

    private void Start()
    {
        if (feedPromptText != null)
            feedPromptText.gameObject.SetActive(false);

        
        if (yummyText != null)
            yummyText.SetActive(false);
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

            // Play sound only if it exists
            if (audioSource != null && feedSound != null)
            {
                audioSource.Stop();
                audioSource.clip = feedSound;
                audioSource.loop = false;
                audioSource.Play();
                StartCoroutine(StopAfterSeconds(feedSoundSeconds));
            }

            if (fedEffect != null)
                Instantiate(fedEffect, transform.position + Vector3.up * 2f, Quaternion.identity);

            alreadyFed = true;

            if (feedPromptText != null)
                feedPromptText.gameObject.SetActive(false);

            Debug.Log("Giraffe fed! 🦒");

            StartCoroutine(ShowYummyText());
        }
    }

    private IEnumerator StopAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (audioSource != null) audioSource.Stop();
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

    private IEnumerator ShowYummyText()
    {
        if (yummyText == null) yield break;

        yummyText.SetActive(true);
        yield return new WaitForSeconds(yummyDuration);
        yummyText.SetActive(false);
    }
}