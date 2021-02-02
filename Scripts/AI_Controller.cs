using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public GameObject sphere;
    [HideInInspector]public GameObject player;
    public float health;
    public float damage;
    public GameObject bullet;
    public GameObject gunPoint;
    private bool isDead;
    private float time;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (!isDead)
        {
            if (sphere.GetComponent<AI_PatrolSphere>().playerSpotted)
            {
                time = time + Time.unscaledDeltaTime;
                shootPlayer();
            }
            else
            {
                relax();
            }
        }
        else
        {
            animator.SetBool("die", true);
        }

    }

    void shootPlayer()
    {
        transform.LookAt(player.transform);
        animator.SetBool("aim", true);
        if (time > 3)
            shoot();
    }

    void shoot()
    {
        GetComponent<AudioSource>().Play();
        animator.SetTrigger("shootTrig");
        Instantiate(bullet, gunPoint.transform.position, transform.rotation);
        time = 0;
    }

    void relax()
    {
        animator.SetBool("aim", false);
        time = 0;
    }

    public void doDamage()
    {
        if(health>0)
        {
            health = health - damage;
            updateHealth();
        }
    }

    void updateHealth()
    {
        if (health <= 0)
        {
            isDead = true;
            animator.SetBool("die", true);
            GetComponent<Rigidbody>().detectCollisions = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    
}
