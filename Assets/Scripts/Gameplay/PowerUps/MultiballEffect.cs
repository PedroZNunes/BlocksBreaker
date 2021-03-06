﻿using UnityEngine;

public class MultiballEffect : MonoBehaviour
{

    [SerializeField] private GameObject trailBall;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private float trailInterval = 0.3f;

    private Sprite ballSprite;
    private GameObject trailParent;
    private float timer = 0;

    private int z = -1000;
    void OnEnable()
    {
        ballSprite = defaultSprite;
        trailParent = GameObject.FindGameObjectWithTag(MyTags.Dynamic.ToString());
    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= trailInterval)
        {

            timer = 0f;
            GameObject ball = transform.parent.parent.gameObject;
            SpriteRenderer[] sprites = ball.GetComponentsInChildren<SpriteRenderer>();
            if (sprites.Length == 1)
            {
                ballSprite = defaultSprite;
            }
            else
            {
                for (int j = 0; j < sprites.Length; j++)
                {
                    if (sprites[j].transform.parent.CompareTag(MyTags.Ball.ToString()))
                    {
                        ballSprite = sprites[j].sprite;
                    }
                }
            }

            

            GameObject trail = Instantiate(trailBall, ball.transform.position, Quaternion.identity, trailParent.transform) as GameObject;
            trail.transform.localScale = ball.transform.localScale;
            SpriteRenderer sr = trail.GetComponent<SpriteRenderer>();
            sr.sprite = ballSprite;
            sr.sortingOrder = z;
            z++;

        }
    }


}
