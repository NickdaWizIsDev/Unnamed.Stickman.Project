using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    public bool hasEntered;

    public GameObject enemyWave;
    public GameObject endBox;

    public BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            hasEntered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasEntered)
        {
            col.isTrigger = false;
            enemyWave.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        int enemyCount = enemyWave.transform.childCount;
        if(enemyCount <= 0)
        {
            col.gameObject.SetActive(false);
            endBox.SetActive(false);
            enemyWave.SetActive(false);
        }
    }
}
