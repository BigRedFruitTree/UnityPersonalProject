using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameManager GM;
    Rigidbody myRB;
    Camera playerCam;
    Transform cameraHolder;

    Vector2 camRotation;

    public Transform weaponSlot;
    
    [Header("Weapon Stats")]
    public GameObject bullet;
    public bool canFire = true;
    public int weaponId = 0;
    public float bulletSpeed = 15;
    public float fireRate = 0;
    public float ammo = 0;
    public float maxAmmo = 0;
    public float reloadAmount = 0;
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
    public GameObject SpawnPoint;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;
    public bool GameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        cameraHolder = transform.GetChild(0);
        transform.position = SpawnPoint.transform.position;

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
                temp.y = jumpHeight;
                jumps--;
            }

            myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

            if (health < 0)
                health = 0;

            if (health == 0)
                GameOver = true;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
        {

            other.gameObject.transform.SetPositionAndRotation(weaponSlot.position, weaponSlot.rotation);
            other.gameObject.transform.SetParent(weaponSlot);

            switch (other.gameObject.name)
            {
                case "Weapon1":

                    weaponId = 1;
                    fireRate = 0.50f;    
                    ammo = 20;
                    maxAmmo = 20;
                    reloadAmount = 20;
                    bulletLifespan = 3;
                    bulletSpeed = 1000;
                    break;


                default:
                    break;
            }

        }

        if ((health < maxHealth) && other.gameObject.tag == "HealthPickup")
        {
            health += healthRestore;
            if (health > maxHealth)
                health = maxHealth;

            Destroy(other.gameObject);
        }

        if ((ammo < maxAmmo) && other.gameObject.tag == "AmmoPickup")
        {


            if (ammo > maxAmmo)
                ammo = maxAmmo;

            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Acid")
        {
            transform.position = SpawnPoint.transform.position;

        }

        



    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BasicEnemy")
        {
            health--;
            
        }
            
    }

    public void reloadAmmo()
    {
        if(ammo < maxAmmo)
        {
            ammo += reloadAmount;
            reloadAmount--;

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