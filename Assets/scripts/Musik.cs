using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musik : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("musik");

        if(objs.Length>1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);

            audioSource = GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            System.Random random = new System.Random();
            audioSource.clip = clips[random.Next(clips.Length)];
            audioSource.Play();
        }
    }
}
