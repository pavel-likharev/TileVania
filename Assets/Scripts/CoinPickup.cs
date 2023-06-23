using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] int valueCoin;

    int[] values = { 100, 200, 300 };
    bool wasCollected = false;

    void Start()
    {
        valueCoin = values[Random.Range(0, values.Length)];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().TakeCoin(valueCoin);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
