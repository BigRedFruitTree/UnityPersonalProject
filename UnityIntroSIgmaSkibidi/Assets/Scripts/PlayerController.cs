using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody myRB;
    Camera playerCam;

    Vector2 camRotation;

    public Transform weaponSlot;
    
    [Header("Weapon Stats")]
    public GameObject bullet;
    public bool canFire = true;
    public int weaponId = 0;
    public float bulletSpeed = 15;
    public float fireRate = 0;
    public float recoil = 0;
    public float ammo = 0;
    public float maxAmmo = 0;
    public float reloadAmount = 0;
    public float ammoRefillAmount = 0;
    public float maxAmmoRefillAmount = 0;
    public float bulletLifespan = 0;
    
   

    [Header("Player Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public int healthRestore = 5;

    [Header("Movement Settings")]
    public float speed = 10.0f;
    public float sprintMultiplier = 2.5f;
    public float jumpHeight = 5.0f;
    public float groundDetectDistance = 1f;
    public int jumps = 2;
    public int jumpsMax = 2;
    public bool sprintMode = false;
    public bool isGrounded = true;

    public float dashDist = 100;
    public int dashes = 1;
    public int dashMax = 1;
    public float startDashSpeed = 1;
    public float endDashSpeed = 100;
    public float dashingTime = 0;




    [Header("User Settings")]
    public bool sprintToggleOption = false;
    public float mouseSensitivity = 2.0f;
    public float Xsensitivity = 2.0f;
    public float Ysensitivity = 2.0f;
    public float camRotationLimit = 90f;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0).GetComponent<Camera>();

        camRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        camRotation.x += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        camRotation.y += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        camRotation.y = Mathf.Clamp(camRotation.y, -camRotationLimit, camRotationLimit);

        playerCam.transform.localRotation = Quaternion.AngleAxis(camRotation.y, Vector3.left);
        transform.localRotation = Quaternion.AngleAxis(camRotation.x, Vector3.up);

        if(Input.GetMouseButtonDown(0) && canFire && ammo > 0 && weaponId > 0)
        {
            
             GameObject s = Instantiate(bullet, weaponSlot.position, weaponSlot.rotation);
             s.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * bulletSpeed);
             Destroy(s, bulletLifespan);

             canFire = false;
             ammo--;

             StartCoroutine("cooldownFire");

                
            
        }

        if(Input.GetKeyDown(KeyCode.R) )
        {
            reloadAmmo();
        } 




        Vector3 temp = myRB.velocity;

        float verticalMove = Input.GetAxisRaw("Vertical");
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        if (!sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                sprintMode = true;

            if (Input.GetKeyUp(KeyCode.LeftShift))
                sprintMode = false;
        }

        if (sprintToggleOption)
        {
            if (Input.GetKey(KeyCode.LeftShift) && verticalMove > 0)
                sprintMode = true;

            if (verticalMove <= 0)
                sprintMode = false;
        }

        if (!sprintMode)
            temp.x = verticalMove * speed;

        if (sprintMode)
            temp.x = verticalMove * speed * sprintMultiplier;

        temp.z = horizontalMove * speed;

        if (Physics.Raycast(transform.position, -transform.up, groundDetectDistance))
        {
            jumps = jumpsMax;
            dashes = dashMax;
            isGrounded = true;
        }
        else
            isGrounded = false;

        

        if (Input.GetKeyDown(KeyCode.Space) && jumps > 0)
        {
            temp.y = jumpHeight;
            jumps --;
        }

        myRB.velocity = (temp.x * transform.forward) + (temp.z * transform.right) + (temp.y * transform.up);

        if (Input.GetKeyDown(KeyCode.E) && dashes > 0 && isGrounded == false)
        {

            dashes--;


            //myRB.AddForce(transform.right * dashDist, ForceMode.Impulse);

            myRB.velocity = playerCam.transform.forward * dashDist;





        }


       dashingTime += Time.deltaTime;
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
                      recoil = 1;
                      ammo = 20;
                      maxAmmo = 20;
                      reloadAmount = 20;
                      ammoRefillAmount = 20;
                    bulletLifespan = 3;
                    bulletSpeed = 1000;
                    maxAmmoRefillAmount = 5;
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
            reloadAmount = ammoRefillAmount;
            ammoRefillAmount--;
            if (ammo > maxAmmo)
                ammo = maxAmmo;

            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
            health--;
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
           

        if (reloadAmount == 0 && ammoRefillAmount > 0)
        { 
          reloadAmount = maxAmmoRefillAmount;
            ammoRefillAmount--;
        }
            
    }








    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(fireRate);
            canFire = true;
    }

}