using System;
using System.Collections;
using UnityEngine;

public class Barboard : MonoBehaviour
{
    #region Singleton

    private static Barboard _instance;

    public static Barboard Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private Camera mainCamera;
    

    public float paddleWidth = 2;
    public float levelWidth = 3f;
    public float speed = 2f;
    public float bounceForce = 1f;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float move = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            move -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += speed * Time.deltaTime;
        }
        Vector2 newPos = transform.position;
        newPos.x += move;
        newPos.x = Mathf.Clamp(newPos.x, -levelWidth+paddleWidth, levelWidth-paddleWidth);
        transform.position = newPos;
    }

    public float angle = 45f;
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector2 hitPoint = coll.contacts[0].point;

            float difference = (transform.position.x - hitPoint.x)/paddleWidth;
            float t = (difference+1)/2;
            float rotationAngle = Mathf.Lerp(-angle,angle,t);
            var directionVectorOfBall = Quaternion.AngleAxis(rotationAngle, Vector3.forward) * Vector2.up;
            ballRb.velocity = directionVectorOfBall * ballRb.velocity.magnitude;
        }
    }

}
