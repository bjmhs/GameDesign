using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region movement_variable
    public float movespeed;
    float x_input;
    float y_input;
    #endregion

    #region attack_variables
    public float damage;
    public float attackspeed;
    float attackTimer;
    public float hitboxTiming;
    public float endAnimationTiming;
    bool isAttacking;
    Vector2 currDirection;
    #endregion

    #region health_variables
    public float maxHP;
    float currHP;
    public Slider hpSlider;
    #endregion

    #region physics_components
    Rigidbody2D playerRB;
    #endregion

    #region animation_components
    Animator animator;
    #endregion

    #region Unity_functions
    //Called once on creation.
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        attackTimer = 0;

        currHP = maxHP;

        hpSlider.value = currHP / maxHP;
    }

    //Called every frame.
    private void Update()
    {
        if (isAttacking)
        {
            return;
        }
        //access our input values.
        x_input = Input.GetAxisRaw("Horizontal");
        y_input = Input.GetAxisRaw("Vertical");

        Move();

        //Check for attack input
        if (Input.GetKeyDown(KeyCode.J) && attackTimer <= 0)
        {
            Attack();
        } else
        {
            attackTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Interact();
        }
    }
    #endregion
    
    #region movement_functions
    //Moves the player based on WASD and 'movespeed'.
    private void Move()
    {
        animator.SetBool("Moving", true);

        //If the player presses 'D'.
        if (x_input > 0)
        {
            playerRB.velocity = Vector2.right * movespeed;
            currDirection = Vector2.right;
        }
        //If the player presses 'A'.
        else if (x_input < 0)
        {
            playerRB.velocity = Vector2.left * movespeed;
            currDirection = Vector2.left;
        }
        //If the player presses 'W'
        else if (y_input > 0)
        {
            playerRB.velocity = Vector2.up * movespeed;
            currDirection = Vector2.up;
        }
        //If the player presses 'S'
        else if (y_input < 0)
        {
            playerRB.velocity = Vector2.down * movespeed;
            currDirection = Vector2.down;
        }
        else
        {
            playerRB.velocity = Vector2.zero;
            animator.SetBool("Moving", false);
        }

        //Set animator dirction values.
        animator.SetFloat("DirX", currDirection.x);
        animator.SetFloat("DirY", currDirection.y);
    }
    #endregion

    #region attack_functions
    //Attacks in the direction that the player is facing.
    private void Attack()
    {
        Debug.Log("Attacking now");
        Debug.Log(currDirection);
        attackTimer = attackspeed;

        //Handles all attack animations and calculate hitboxes.
        StartCoroutine(AttackRoutine());

        attackTimer = attackspeed;
    }

    //Handle animations and hitboxes for the attack mechanism.
    IEnumerator AttackRoutine()
    {
        //Pause movement and freeze the player for the duration of the attack.
        isAttacking = true;
        playerRB.velocity = Vector2.zero;

        //Start animation.
        animator.SetTrigger("Attack");

        //Start sound effect
        FindObjectOfType<AudioManager>().Play("PlayerAttack");

        //Brief pasue before we calculate the hitbox.
        yield return new WaitForSeconds(hitboxTiming);

        Debug.Log("Cast hitbox now");

        //Create hitbox
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, Vector2.one, 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("tons of damage");
                hit.transform.GetComponent<Enemy>().TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(endAnimationTiming);

        //Re-enables player after attacking.
        isAttacking = false;
    }
    #endregion

    #region health_functions
    //Take damage based on 'value' parameter, which is passed in by caller.
    public void TakeDamge(float value)
    {
        //Call sound effect.
        FindObjectOfType<AudioManager>().Play("PlayerHurt");

        //Decrement HP.
        currHP -= value;
        Debug.Log("Health is now" + currHP.ToString());

        //Change UI
        hpSlider.value = currHP / maxHP;

        //Check for death
        if (currHP <= 0)
        {
            Die();
        }
    }

    //Heal player's HP based on 'value' parameter, which is passed in by called.
    public void Heal(float value)
    {
        //Increment HP.
        currHP += value;
        currHP = Mathf.Min(currHP, maxHP);
        Debug.Log("Health is now" + currHP.ToString());

        //Change UI
        hpSlider.value = currHP / maxHP;
    }

    public void Speedup(float value)
    {
        movespeed += value;
        Debug.Log("Speed is now" + movespeed.ToString());
    }

    public void Poison(float value)
    {
        TakeDamge(value);
    }

    //Destroys player object and triggers scene stuff.
    private void Die()
    {
        //Call death sound effect.
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //Destroy Gameobject
        Destroy(this.gameObject);

        //Trigger anything we need to end the game, find game manager and lose game.
        GameObject gm = GameObject.FindWithTag("GameController");
        gm.GetComponent<GameManager>().LoseGame();
    }

    #endregion

    #region interact_functions

    void Interact()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(playerRB.position + currDirection, new Vector2(0.5f, 0.5f), 0f, Vector2.zero, 0);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag("Chest"))
            {
                hit.transform.GetComponent<Chest>().Interact();
            }
        }
    }

    #endregion
}
