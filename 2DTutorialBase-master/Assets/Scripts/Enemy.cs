using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region movement_variables 
    public float movespeed;
    #endregion

    #region physics_components
    Rigidbody2D enemyRB;
    #endregion

    #region targeting_variables 
    public Transform player;
    #endregion

    #region attack_variables 
    public float explosionDamage;
    public float explosionRadius;
    public GameObject explosionObj;
    #endregion

    #region health_variables
    public float maxHP;
    float currHP;
    #endregion

    #region Unity_functions
    //Runs once oncreation
    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        currHP = maxHP;
    }

    //Runs every Frame
    private void Update()
    {
        //Check where the player is
        if (player == null)
        {
            return;
        }

        Move();
    }
    #endregion

    #region movement_functions
    //Move directly at player
    private void Move()
    {
        //Calculate the movement vector. Player pos - Enemy pos = Direction to player
        Vector2 direction = player.position - transform.position;

        enemyRB.velocity = direction.normalized * movespeed;
    }
    #endregion

    #region attack_functions
    //Raycasts box for player and causes damage, spawns explosion prefab.
    private void Explode()
    {
        //Call for explosion sound.
        FindObjectOfType<AudioManager>().Play("Explosion");

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, explosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Player"))
            {
                //Cause damage
                hit.transform.GetComponent<PlayerController>().TakeDamge(explosionDamage);
                Debug.Log("Hit Player");

                //Spawn Explosion prefab in game.
                Instantiate(explosionObj, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.transform.CompareTag("Player"))
        {
            Explode();
        } 
    }

    #endregion

    #region health_functions
    //Enemy takes damge based on 'value' paramter.
    public void TakeDamage(float value)
    {
        //Call for hurt sound effect.
        FindObjectOfType<AudioManager>().Play("BatHurt");

        //Decrement HP.
        currHP -= value;
        Debug.Log("Health is now" + currHP.ToString());

        //Check for death
        if (currHP <= 0)
        {
            Die();
        }
    }

    //Destroys enemy object
    void Die()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
