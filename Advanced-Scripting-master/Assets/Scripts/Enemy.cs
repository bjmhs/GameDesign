using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Changes the radius of the circle range which enemy spawns")]
    private float radius = 0.5f;

    #region pos_and_time_variables
    private float hor_bound = 10f;
    private float ver_bound = 5f;
    private float elapasedTime = 0f;
    private Vector2 startPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(Random.Range(-hor_bound, hor_bound), Random.Range(-ver_bound, ver_bound));
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapasedTime += Time.deltaTime;
        transform.position = startPosition + new Vector2(0, radius * Mathf.Cos(elapasedTime));
    }
}
