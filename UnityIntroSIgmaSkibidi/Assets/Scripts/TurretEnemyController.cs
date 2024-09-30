using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemyController : MonoBehaviour
{
    [Header("Basic Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public int damageGive = 1;
    public int damageReceive = 1;

    public PlayerController Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }




        


        
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= damageReceive;
            Destroy(collision.gameObject);

        }

    }

}
