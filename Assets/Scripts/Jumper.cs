﻿using System.Timers;
using Unity.MLAgents;
using UnityEngine;

public class Jumper : Agent
{

    private Rigidbody body;
    private Environment environment;
    public float jumpSpeed = 20;
    public float movementSpeed = 1;
    public bool isGrounded;

    public override void Initialize()
    {
        base.Initialize();
        body = GetComponent<Rigidbody>();
        environment = GetComponentInParent<Environment>();
    }

    public override void OnEpisodeBegin()
    {
        Respawn();
    }

    public void Respawn()
    {
        transform.localPosition = new Vector3(0f, 0.5f, -16f);
        body.angularVelocity = Vector3.zero;
        body.velocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FixedUpdate()
    {
        if (transform.localPosition.y < 0 || transform.localPosition.z < -19)
        {
            Die();
        }
        else if (isGrounded)
        {
            AddReward(0.001f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision);
        if (collision.transform.CompareTag("Obstakel"))
        {
            Die();
        }
    }

    private void Die()
    {
        AddReward(-1f);
        Respawn();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;


        if (Input.GetKey(KeyCode.Space)) // Jump
        {
            //Debug.Log("Spatiebalk ingedrukt");
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[1] = 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[2] = 1f;

        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Point")
        {
            Destroy(other.gameObject);
            AddReward(0.10f);
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Debug.Log("Springen aangeroepen!");
        if (vectorAction[0] != 0 && isGrounded)
        {
            //Debug.Log("springen!");
            body.AddForce(Vector3.up * jumpSpeed * 300);
            isGrounded = false;
        }

        if (vectorAction[1] > 0.5f)
        {
            Vector3 rightVelocity = new Vector3(movementSpeed * vectorAction[1], 0f, 0f);
            body.velocity = body.velocity + rightVelocity;

        }

        if (vectorAction[2] > 0.5f)
        {

            Vector3 leftVelocity = new Vector3(-movementSpeed * vectorAction[2], 0f, 0f);
            body.velocity = body.velocity + leftVelocity;
        }

    }

}
