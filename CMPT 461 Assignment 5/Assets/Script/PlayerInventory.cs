using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Apples")]
    public int apples = 0;
    public int applesNeeded = 4;

    [Header("UI")]
    public TextMeshProUGUI appleText;

    [Header("Audio")]
    public AudioSource audioSource;        
    public AudioClip counterChangeSound;   

    private void Start()
    {
        UpdateUI();
    }

    public void AddApple(int amount)
    {
        apples += amount;
        PlayCounterSound();
        UpdateUI();
    }

    public bool HasEnoughApples()
    {
        return apples >= applesNeeded;
    }

    public void ConsumeApples(int amount)
    {
        apples = Mathf.Max(0, apples - amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (appleText != null)
            appleText.text = $"Apples: {apples} / {applesNeeded}";
    }

    private void PlayCounterSound()
    {
        if (audioSource != null && counterChangeSound != null)
            audioSource.PlayOneShot(counterChangeSound);
    }
}