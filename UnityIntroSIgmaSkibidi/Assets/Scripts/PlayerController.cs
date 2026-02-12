using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    
    [Header("Weapon Stats")]
    public bool canFire = true;
    public int weaponId = 0;
    public float bulletSpeed = 15;
    public float fireRate = 0;
    public int ammo = 0;
    public int maxAmmo = 0;
    public int reloadAmount = 0;
    public int maxReloads = 0;
    public int ammoNumber = 0;
    public float bulletLifespan = 0;

    [Header("Player Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public int healthRestore = 5;

    [Header("Movement Settings")]
    public float speed = 15.0f;
    public float sprintMultiplier = 2.5f;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;
    public int jumps = 2;
    public int jumpsMax = 2;
    public bool sprintMode = false;
    public bool isGrounded = true;
    public float stamina = 150;
    public float maxStamina = 150;


    [Header("User Settings")]
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;
    public bool GameOver = false;

    [Header("Audio Settings")]
    private AudioSource audioSource;
    public AudioClip jumpAudio;
    public AudioClip shoot1Audio;
    public AudioClip pickupAudio;
    public AudioClip hitHurtAudio;
    public AudioClip reloadAudio;

    [Header("Refrences")]
    public GameManager GM;
    public GameObject SpawnPoint;
    public Rigidbody myRB;
    public Camera playerCam;
    public Transform cameraHolder;
    public Vector2 camRotation;
    public Transform weaponSlot;
    public GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        cameraHolder = transform.GetChild(0);
        transform.position = SpawnPoint.transform.position;
        audioSource = GetComponent<AudioSource>();
        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GM.isPaused)
        {



            playerCam.transform.position = cameraHolder.position;

            if (GameOver == false)
            {

                camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
                camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

                camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);


                playerCam.transform.rotation = Quaternion.Euler(-camRotation.y, camRotation.x, 0);
                transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);


            }


            if (Input.GetMouseButtonDown(0) && canFire && ammo > 0 && weaponId > 0 && GameOver == false)
            {


                audioSource.PlayOneShot(shoot1Audio, 0.3f);
                GameObject s = Instantiate(bullet, weaponSlot.position, weaponSlot.rotation);
                s.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * bulletSpeed);
                Destroy(s, bulletLifespan);

                canFire = false;
                ammo--;

                StartCoroutine("cooldownFire");



            }

            if (Input.GetKeyDown(KeyCode.R) && GameOver == false)
            {
                reloadAmmo();
            }

            Vector3 temp = myRB.velocity;

            float verticalMove = Input.GetAxisRaw("Vertical");
            float horizontalMove = Input.GetAxisRaw("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
            {
                if (stamina > 0)
                {
                   
                    sprintMode = true;
                }
                else
                    sprintMode = false;


            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                sprintMode = false;
            }

            if (sprintMode == true)
                stamina--;

            if (stamina < 0)
                stamina = 0;

            if (stamina == 0)
                sprintMode = false;

            if (sprintMode == false)
            {
                stamina++;
            }

            if (stamina > maxStamina)
                stamina = maxStamina;



            if (!sprintMode)
                temp.x = verticalMove * speed;

            if (sprintMode)
                temp.x = verticalMove * speed * sprintMultiplier;

            temp.z = horizontalMove * speed;

            if (Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
            {
                jumps = jumpsMax;

                isGrounded = true;
            }
            else
                isGrounded = false;



            if (Input.GetKeyDown(KeyCode.Space) && jumps > 0 && GameOver == false)
            {
                audioSource.PlayOneShot(jumpAudio, 0.2f);
                temp.y = jumpHeight;
                jumps--;
            }

            myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

            if (health < 0)
                health = 0;

            if (health == 0)
                GameOver = true;

            if (reloadAmount > maxReloads && weaponId == 1)
            {
                reloadAmount = 8;
            }
            else if (reloadAmount > maxReloads && weaponId == 2)
            {
                reloadAmount = 4;
            } 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Weapon") && weaponId == 0)
        {
            audioSource.PlayOneShot(pickupAudio, 0.4f);
            other.gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);
            other.gameObject.transform.SetParent(weaponSlot);

            switch (other.gameObject.name)
            {
                case "Weapon1":

                    weaponId = 1;
                    fireRate = 0.50f;    
                    ammo = 20;
                    maxAmmo = 20;
                    reloadAmount = 2;
                    maxReloads = 8;
                    bulletLifespan = 2;
                    bulletSpeed = 5000;
                    ammoNumber = 20;
                    break;


                default:
                    break;
            }
            switch (other.gameObject.name)
            {
                case "Sniper3":

                    weaponId = 2;
                    fireRate = 1.5f;
                    ammo = 6;
                    maxAmmo = 6;
                    reloadAmount = 2;
                    maxReloads = 4;
                    bulletLifespan = 2;
                    bulletSpeed = 9000;
                    ammoNumber = 6;
                    break;


                default:
                    break;
            }

        }

        if ((health < maxHealth) && other.gameObject.tag == "HealthPickup")
        {
            audioSource.PlayOneShot(pickupAudio, 0.4f);
            health += healthRestore;
            if (health > maxHealth)
                health = maxHealth;

            Destroy(other.gameObject);
        }

        if (ammo < maxAmmo && other.gameObject.tag == "AmmoPickup" && reloadAmount < maxReloads || ammoNumber < 2 && other.gameObject.tag == "AmmoPickup" && reloadAmount < maxReloads)
        {
            audioSource.PlayOneShot(pickupAudio, 0.4f);
            reloadAmount++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Acid")
        {
            audioSource.PlayOneShot(hitHurtAudio, 0.5f);
            transform.position = SpawnPoint.transform.position;
        }

        if (other.gameObject.name == "EndText")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Level1");

        }



    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BasicEnemy")
        {
            audioSource.PlayOneShot(hitHurtAudio, 0.5f);
            health--;
        }

        if (collision.gameObject.name == "Elevator")
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

    }

    public void reloadAmmo()
    {
        if(ammo < maxAmmo)
        {
           if(reloadAmount > 0)
           {
                audioSource.PlayOneShot(reloadAudio, 0.5f);
                ammo += ammoNumber;
                reloadAmount--;
           }
            

        }

        if (ammo > maxAmmo)
            ammo = maxAmmo;

        if (reloadAmount < 0)
        {
          reloadAmount = 0;
        }
           
    }

    

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
            canFire = true;
    }

}