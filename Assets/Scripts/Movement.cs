using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Movement : MonoBehaviour
{
    private Transform player;
    public float laneChangeSpeed = 15f;
    private Rigidbody rb;

    private int currentLane = 1;
    private float laneOffset = 3.5f;

    private void Start()
    {
        player = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        Vector3 startPosition = player.position;
        startPosition.x = 0;
        player.position = startPosition;
    }

    private void Update()
    {
        laneChangeSpeed = 15f; 
        HandleLaneMovement();
    }

    private void HandleLaneMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
        }

        Vector3 targetPosition = player.position;
        targetPosition.x = (currentLane - 1) * laneOffset;

        player.position = Vector3.MoveTowards(player.position, targetPosition, laneChangeSpeed * Time.deltaTime);
    }
}
