using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // We only need to define these once!
    [Header("Speakers")]
    public AudioSource bgmSource; // Speaker for Background Music
    public AudioSource sfxSource; // Speaker for Sound Effects

    [Header("Audio Files")]
    public AudioClip backgroundMusic;
    public AudioClip coinSound;
    public AudioClip happyCustomerSound;
    public AudioClip angryCustomerSound;
    public AudioClip rushHourAlertSound;
    
    [Header("UI Sound")]
    public AudioClip buttonClickSound;

    private void Start()
    {
        // Automatically load the saved volumes when the game starts!
        if (bgmSource != null) 
        {
            bgmSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        }
        
        if (sfxSource != null) 
        {
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        }
    }

    // --- SFX FUNCTIONS ---
    // Other scripts will call these to play specific sounds

    public void PlayCoinSound()
    {
        if (sfxSource != null && coinSound != null)
        {
            // PlayOneShot allows multiple sound effects to play over each other
            // without interrupting the previous sound!
            sfxSource.PlayOneShot(coinSound);
        }
    }

    public void PlayHappySound()
    {
        if (sfxSource != null && happyCustomerSound != null) sfxSource.PlayOneShot(happyCustomerSound);
    }

    public void PlayAngrySound()
    {
        if (sfxSource != null && angryCustomerSound != null) sfxSource.PlayOneShot(angryCustomerSound);
    }
    
    public void PlayRushHourSound()
    {
        if (sfxSource != null && rushHourAlertSound != null) sfxSource.PlayOneShot(rushHourAlertSound);
    }

    public void PlayButtonSound()
    {
        if (sfxSource !=null && buttonClickSound !=null) sfxSource.PlayOneShot(buttonClickSound);
    }

    public void SetMusicVolume(float volume)
    {
        if (bgmSource !=null)
        {
            bgmSource.volume = volume;
        }
    }    
    
    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }
    }
}