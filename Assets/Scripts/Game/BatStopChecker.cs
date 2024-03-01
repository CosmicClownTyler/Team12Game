﻿using UnityEngine;

public class BatStopChecker : MonoBehaviour
{
    private Rigidbody rb;

    private GameObject player;
    private BatThrower playerThrower;

    // These values are used to keep track of how long a bat has existed for
    // If the existence time passes the cutoff, the bat is destroyed
    // This is used as a backup in case a bat falls out of the map or continues rolling for too long
    private float timePassed = 0;
    private float timeCutoff = 7;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerThrower = player.GetComponentInChildren<BatThrower>();

        rb = transform.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.IsSleeping() || timePassed > timeCutoff)
        {
            playerThrower.ResetThrowableTrigger();
            Destroy(transform.gameObject);
        }

        timePassed += Time.deltaTime;
    }
}