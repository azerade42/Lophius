using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshyRandomNoise : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource audioSource;
    public float minWaitBetweenPlays = 10f;
    public float maxWaitBetweenPlays = 18f;
    public float waitTimeCountdown = -1f;
 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
 
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (waitTimeCountdown < 0f)
            {
                currentClip = audioClips[Random.Range(0, audioClips.Count)];
                audioSource.clip = currentClip;
                audioSource.Play();
                waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
            }
            else
            {
                waitTimeCountdown -= Time.deltaTime;
            }
        }
    }
}

