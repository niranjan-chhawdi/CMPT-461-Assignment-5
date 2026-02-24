using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GiraffeFeeder : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode feedKey = KeyCode.E;
    public int applesRequired = 4;

    [Header("Restart UI")]
    public GameObject restartText;
    public KeyCode restartKey = KeyCode.R;

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

        if (restartText != null)
            restartText.SetActive(false);
    }

    private void Update()
{
    if (!alreadyFed && restartText != null)
        restartText.SetActive(false);

    if (alreadyFed)
    {
        if (restartText != null)
            restartText.SetActive(true);

        if (Input.GetKeyDown(restartKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        return;
    }

    if (!playerInRange) return;
    if (playerInventory == null) return;

    bool canFeed = playerInventory.apples >= applesRequired;

    if (feedPromptText != null)
        feedPromptText.gameObject.SetActive(canFeed);

    if (canFeed && Input.GetKeyDown(feedKey))
    {
        playerInventory.ConsumeApples(applesRequired);

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