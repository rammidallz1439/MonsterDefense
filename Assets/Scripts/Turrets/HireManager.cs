using System;
using UnityEngine;
using TMPro; // For TMP_Text

public class HireManager : MonoBehaviour
{
    public GameObject hiredCharacterPrefab; // Assign the prefab in the Inspector
    private GameObject hiredCharacterInstance; // Instance of the hired character
    private float hireDuration = 60f; // Duration of hire in seconds
    private string hireEndTimeKey = "HireEndTime"; // Key for PlayerPrefs

    public TMP_Text timerText; // Assign the TMP_Text in the Inspector to show the timer

    private float remainingTime; // Remaining time for the hire timer
    private bool isTimerRunning = false; // Tracks whether the timer is running

    void Start()
    {
        // Load saved hire state on game start
        LoadHireState();
    }

    void Update()
    {
        // Update the timer display if the timer is running
        if (isTimerRunning)
        {
            UpdateTimerDisplay();
        }
    }

    // Call this method when hiring starts
    public void HireCharacter()
    {
        // Calculate the end time
        DateTime hireEndTime = DateTime.Now.AddSeconds(hireDuration);

        // Save the end time to PlayerPrefs
        PlayerPrefs.SetString(hireEndTimeKey, hireEndTime.ToString());
        PlayerPrefs.Save();

        // Spawn the character and start the timer
        SpawnCharacter();
        StartHireTimer(hireDuration);
    }

    private void LoadHireState()
    {
        // Check if hireEndTime is saved in PlayerPrefs
        if (PlayerPrefs.HasKey(hireEndTimeKey))
        {
            // Retrieve the saved hire end time
            string savedEndTimeString = PlayerPrefs.GetString(hireEndTimeKey);
            DateTime savedEndTime = DateTime.Parse(savedEndTimeString);

            // Calculate the remaining time
            TimeSpan timePassed = savedEndTime - DateTime.Now;

            if (timePassed.TotalSeconds > 0)
            {
                // Hire duration is still valid, spawn the character
                SpawnCharacter();

                // Start the timer with the remaining time
                StartHireTimer((float)timePassed.TotalSeconds);
            }
            else
            {
                // Hire duration has expired, do nothing
                Debug.Log("Hire duration expired while app was closed.");
            }
        }
    }

    private void SpawnCharacter()
    {
        // Ensure only one character instance exists
        if (hiredCharacterInstance == null)
        {
            hiredCharacterInstance = Instantiate(hiredCharacterPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Hired character spawned.");
        }
    }

    private void StartHireTimer(float duration)
    {
        remainingTime = duration;
        isTimerRunning = true;
        StartCoroutine(HireTimerCoroutine());
    }

    private System.Collections.IEnumerator HireTimerCoroutine()
    {
        Debug.Log($"Hire timer started for {remainingTime} seconds.");

        while (remainingTime > 0)
        {
            yield return null; // Wait for the next frame
            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                TimerEnd();
            }
        }
    }

    private void TimerEnd()
    {
        isTimerRunning = false;

        // Destroy the hired character when the timer ends
        if (hiredCharacterInstance != null)
        {
            Destroy(hiredCharacterInstance);
            Debug.Log("Hired character destroyed after hire duration.");
        }

        // Clear the saved end time from PlayerPrefs
        PlayerPrefs.DeleteKey(hireEndTimeKey);
        PlayerPrefs.Save();
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Display in MM:SS format
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
