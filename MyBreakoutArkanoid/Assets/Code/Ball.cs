using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Ball : MonoBehaviour
{
    private SpriteRenderer sr;
    private Rigidbody2D body;

    public bool isLightningBall;
    public ParticleSystem lightningBallEffect;
    public float lightningBallDuration = 10;
    public float speedUpOnBrickHit = 0.1f;

    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;

    private void Awake()
    {
        this.sr = GetComponentInChildren<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    public void StartLightningBall()
    {
        if (!this.isLightningBall)
        {
            this.isLightningBall = true;
            this.sr.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBallAfterTime(this.lightningBallDuration));

            OnLightningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightningBallAfterTime(float seconds)
    {
        // yield return new WaitForSeconds(seconds);

        float timer = 0f;
        float duration = seconds;
        while(timer < duration)
        {
            yield return null;
            timer += Time.deltaTime;
            // Math.Lerp(initial value, target value, timer / durtaion)
        }

        StopLightningBall();
    }

    private void StopLightningBall()
    {
        if (this.isLightningBall)
        {
            this.isLightningBall = false;
            this.sr.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Brick")
        {
            body.AddForce(body.velocity.normalized * speedUpOnBrickHit);
        }
    }

    void OnDrawGizmos()
    {
        Handles.Label(new Vector2(3, -4), body.velocity.magnitude.ToString());
    }
}
