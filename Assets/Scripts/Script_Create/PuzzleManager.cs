using System.Collections;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject correctPanel;
    public GameObject currentPuzzle;
    public GameObject nextPuzzle;
    public int totalPieces = 5;

    private int correctPieces = 0;

    public AudioSource correctSound;

    public GameObject confetti1;
    public GameObject confetti2;

    private ParticleSystem confetti1PS;
    private ParticleSystem confetti2PS;

    void Start()
    {
        // Ensure confetti objects are deactivated at start
        if (confetti1 != null)
            confetti1.SetActive(false);
        if (confetti2 != null)
            confetti2.SetActive(false);

        // Cache the ParticleSystems
        if (confetti1 != null)
            confetti1PS = confetti1.GetComponent<ParticleSystem>();
        if (confetti2 != null)
            confetti2PS = confetti2.GetComponent<ParticleSystem>();
    }

    public void PiecePlacedCorrectly()
    {
        correctPieces++;

        if (correctPieces >= totalPieces)
        {
            StartCoroutine(ShowCorrectAndNext());
        }
    }

    IEnumerator ShowCorrectAndNext()
    {
        // Show Correct Panel
        correctPanel.SetActive(true);

        // Add Score
        GameManager.instance.AddScore(5);

        // Play Applause SFX
        if (correctSound != null && PlayerPrefs.GetInt("MusicEnabled")==1)
        {
            correctSound.Play();
        }

        // Activate Confetti and Play
        if (confetti1 != null)
        {
            confetti1.SetActive(true);
            confetti1PS?.Play();
        }
        if (confetti2 != null)
        {
            confetti2.SetActive(true);
            confetti2PS?.Play();
        }

        // Wait 3 seconds
        yield return new WaitForSeconds(3f);

        // Stop SFX after 3 seconds
        if (correctSound != null && correctSound.isPlaying)
        {
            correctSound.Stop();
        }

        // Stop and deactivate Confetti
        if (confetti1 != null)
        {
            confetti1PS?.Stop();
            confetti1.SetActive(false);
        }
        if (confetti2 != null)
        {
            confetti2PS?.Stop();
            confetti2.SetActive(false);
        }

        // Hide Correct Panel
        correctPanel.SetActive(false);

        // Move to Next Puzzle
        currentPuzzle.SetActive(false);
        int currentCoin = PlayerPrefs.GetInt("coinData", 0);
        currentCoin=currentCoin+5;
        PlayerPrefs.SetInt("coinData", currentCoin);
        if (nextPuzzle != null)
            nextPuzzle.SetActive(true);
    }
}
