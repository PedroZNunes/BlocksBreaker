using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class MultiBallInstance : MonoBehaviour
{

    [SerializeField] private GameObject multiBallEffect;
    [SerializeField] private GameObject ballPrefab;
    private GameObject activeEffect;
    private Transform dynamic;
    static private int Count;


    void Awake()
    {
        Debug.Assert(multiBallEffect != null, "Multiball Effect not set in the inspector");
        Debug.Assert(ballPrefab != null, "Ball Prefab not set in inspector.");
        dynamic = GameObject.FindWithTag(MyTags.Dynamic.ToString()).transform;
        Subscribe();
        Count++;
    }

    void Start()
    {
        activeEffect = Instantiate(multiBallEffect, this.transform.position, Quaternion.identity, this.transform) as GameObject;
    }

    void ActiveEffect(Collider2D blockCol, Collider2D ballCol)
    {
        if (transform.parent == ballCol.transform)
        {
            StartCoroutine(Spawn(blockCol, ballCol));
            
        }
    }

    private IEnumerator Spawn(Collider2D blockCol, Collider2D ballCol){
        yield return new WaitForSeconds(0.4f);
        GameObject newBall = Instantiate(ballPrefab, ballCol.bounds.center, Quaternion.identity, dynamic) as GameObject;
        Rigidbody2D ballRB = ballCol.GetComponent<Rigidbody2D>();
        newBall.GetComponent<Rigidbody2D>().velocity = new Vector2(-ballRB.velocity.x, -ballRB.velocity.y);
        Destroy(activeEffect);
        Count--;
        TestEndOfEffect();
        Destroy(gameObject);
        yield return 0;
    }


    private void TestEndOfEffect()
    {
        if (Count <= 0)
        {
            Unsubscribe();
            PlayerBuffs.isMulti = false;
        }
    }

    void OnDisable() { Unsubscribe(); }

    private void Subscribe() { Block.GotHit += ActiveEffect; }
    private void Unsubscribe() { Block.GotHit -= ActiveEffect; }
}
