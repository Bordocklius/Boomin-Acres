using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shuffles the track list, plays them one by one, reshuffles when done, repeat forever

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] tracks;
    [SerializeField, Range(0f, 1f)] private float volume = 0.5f;

    private static MusicManager instance;

    private AudioSource audioSource;
    private List<int> shuffledOrder = new List<int>();
    private int currentIndex = 0;

    void Awake()
    {
        // if one already exists from a previous scene, kill this duplicate
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // survive scene loads

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = (float)(volume * 0.1); //way too loud otherwise
    }

    void Start()
    {
        if (tracks == null || tracks.Length == 0) return;

        Shuffle();
        StartCoroutine(PlayLoop());
    }

    public void SetVolume(float value)
    {
        volume = value;
        audioSource.volume = value;
    }

    private IEnumerator PlayLoop()
    {
        while (true)
        {
            AudioClip clip = tracks[shuffledOrder[currentIndex]];
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length); // just wait, no per-frame checks

            currentIndex++;
            if (currentIndex >= shuffledOrder.Count) Shuffle();
        }
    }

    private void Shuffle()
    {
        shuffledOrder.Clear();
        for (int i = 0; i < tracks.Length; i++)
            shuffledOrder.Add(i);

        // Fisher-Yates
        for (int i = shuffledOrder.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (shuffledOrder[i], shuffledOrder[j]) = (shuffledOrder[j], shuffledOrder[i]);
        }

        currentIndex = 0;
    }
}
