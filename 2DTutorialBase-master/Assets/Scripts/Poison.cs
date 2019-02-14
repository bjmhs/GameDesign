using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    #region Poison_variable
    [SerializeField]
    [Tooltip("Assign the health decremented by the poison!")]
    private int poisonAmount;
    #endregion

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Poison(poisonAmount);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
