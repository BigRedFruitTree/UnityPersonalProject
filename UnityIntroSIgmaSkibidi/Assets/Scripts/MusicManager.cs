using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
  
    public GameObject player;
    public GameManager GM;
    public AudioClip MainMenu;
    public AudioClip Tutorial;
    public AudioClip Level1;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            player = GameObject.Find("Player");
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            audioSource.PlayOneShot(MainMenu, 0.2f);
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            audioSource.PlayOneShot(Tutorial, 0.2f);
        }

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            audioSource.PlayOneShot(Level1, 0.2f);
        }


    }



    // Update is called once per frame
    void Update()
    {
      transform.position = player.transform.position;
    }
}
