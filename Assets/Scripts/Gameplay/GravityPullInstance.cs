using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityPullInstance : MonoBehaviour
{
    public float pullAreaRadius = 3f;
    [SerializeField] IntegerVariable ballSpeed;
    [SerializeField] private LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] affectedBalls = Physics2D.OverlapCircleAll(transform.position, pullAreaRadius, mask);
        if (affectedBalls.Length == 0)
            return;

        foreach (var ball in affectedBalls)
        {
            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(this.transform.position.x - ball.transform.position.x, this.transform.position.y - ball.transform.position.y).normalized;
            rb.velocity = direction * ballSpeed.value;
        }
    }

}
