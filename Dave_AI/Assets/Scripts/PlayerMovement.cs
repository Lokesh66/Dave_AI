using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Transform playerTrans;

    private string currentAnimState = string.Empty;

    private float playerSpeed = 2.0f;

    private bool isObstacle = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (IsAnyKeyOnHold())
        {
            UpdateAnimation("Walk");
        }
        else 
        {
            UpdateAnimation("Idle");
        }

        UpdatePlayerPosition();
    }

    private void UpdateAnimation(string anim)
    {
        Debug.LogError("UpdateAnimation : anim = " + anim + " currentAnimState = " + currentAnimState);

        if (!currentAnimState.Equals(anim))
        {
            animator.SetTrigger(anim);

            currentAnimState = anim;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collider Tag = " + collision.gameObject.name);
        isObstacle = true;
        UpdatePlayerPosition(false);

        StopAllCoroutines();
        StartCoroutine(ResetObstacleParameter());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter : " + other.tag);
        UpdatePlayerPosition();
    }

    private void UpdatePlayerPosition(bool canMove = true)
    {
        //Debug.LogError("canMove : " + canMove + " isObstacle = " + isObstacle);
        if (canMove && !isObstacle)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
        else {

            Vector3 direction = new Vector3(-0.003f, 0f, -0.003f);
            transform.Translate(direction * playerSpeed * Time.deltaTime);

            UpdateAnimation("Idle");
        }
    }

    IEnumerator ResetObstacleParameter()
    {
        yield return new WaitForSeconds(1.0f);

        Debug.Log("ResetObstacleParameter : " + IsAnyKeyOnHold());

        if (!IsAnyKeyOnHold())
        {
            isObstacle = false;
        }
        else {
            StopAllCoroutines();
            StartCoroutine(ResetObstacleParameter());
        }
    }

    private bool IsAnyKeyOnHold()
    {
        return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow);
    }
}
