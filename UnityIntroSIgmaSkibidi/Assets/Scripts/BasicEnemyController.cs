using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    [Header("Basic Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public int damageGive = 1;
    public int damageReceive = 1;
    public int pushBackForce = 10;




    public PlayerController Player;
    public NavMeshAgent agent;
    public GameObject detection;



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }





        if (detection.GetComponent<BasicEnemyDetectionLogic>().detectPlayer == 1)
        {
            agent.destination = Player.transform.position;
        }
        

        


       



    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

            if (Player.weaponId == 1)
            {
                damageReceive = 1;
                health -= damageReceive;
                Destroy(collision.gameObject);
            }

            if (Player.weaponId == 2)
            {
                damageReceive = 5;
                health -= damageReceive;
                Destroy(collision.gameObject);
            }

        }

    }

   


}
