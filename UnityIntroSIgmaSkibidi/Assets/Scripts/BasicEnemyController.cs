using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyController : MonoBehaviour
{
    [Header("Basic Stats")]
    public int health = 5;
    public int maxHealth = 5;
    public int damageGive = 1;
    public int damageReceive = 1;
    public float playerDistance;

    [Header("Refs")]
    public AudioSource audioSource;
    public AudioClip EnemyHurtAudio;
    public PlayerController Player;
    public NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, Player.gameObject.transform.position);

        if (health <= 0)
        {
            audioSource.PlayOneShot(EnemyHurtAudio, 0.2f);
            Destroy(gameObject);
        }

        if (playerDistance <= 30f)
        {
            agent.destination = Player.transform.position;
        }
        else
        {
            agent.destination = agent.transform.position;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {

            if (Player.weaponId == 1)
            {
                audioSource.PlayOneShot(EnemyHurtAudio, 0.2f);
                damageReceive = 1;
                health -= damageReceive;
                Destroy(collision.gameObject);
            }

            if (Player.weaponId == 2)
            {
                audioSource.PlayOneShot(EnemyHurtAudio, 0.2f);
                damageReceive = 5;
                health -= damageReceive;
                Destroy(collision.gameObject);
            }

        }

    }

   


}
