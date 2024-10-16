using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject playerUI;
    public PlayerController playerData;
    public Image healthBar;
    public GameObject crossHair;
    public TextMeshProUGUI jumpsCounter;
    public TextMeshProUGUI ammoCounter;
    public TextMeshProUGUI reloadCounter;
    public TextMeshProUGUI boostCounter;



    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
            playerData = GameObject.Find("Player").GetComponent<PlayerController>();



    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {


            healthBar.fillAmount = Mathf.Clamp((float)playerData.health / (float)playerData.maxHealth, 0, 1);


            ammoCounter.text = "Ammo: " + playerData.ammo;
            reloadCounter.text = "Reloads: " + playerData.reloadAmount;
            boostCounter.text = "Boost: " + playerData.stamina;
            jumpsCounter.text = "Jumps: " + playerData.jumps;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPaused)
                {
                    pauseMenu.SetActive(true);
                    playerUI.SetActive(false);
                    crossHair.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    Time.timeScale = 0;

                    isPaused = true;

                }
                else
                    Resume();
            }


        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        playerUI.SetActive(true);
        crossHair.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        isPaused = false;
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevel(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
        
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }



}
