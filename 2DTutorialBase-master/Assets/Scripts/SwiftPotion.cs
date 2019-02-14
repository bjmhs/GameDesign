using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwiftPotion : MonoBehaviour
{
    #region SwiftPotion_variable
    [SerializeField]
    [Tooltip("Assign the speed value of the speed potion!")]
    private int speedBoostAmount;
    #endregion

    #region
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<PlayerController>().Speedup(speedBoostAmount);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
