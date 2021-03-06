﻿using UnityEngine;

public class ExplosiveBall : PowerUp
{
    [SerializeField] private GameObject explosiveBallInstancePrefab;

    void OnDisable() { Unsubscribe(); }

    public override void OnPickUp()
    {
        PlayerBuffs.isExplosive = true;
        Ball[] balls = FindObjectsOfType<Ball>();
        for (int i = 0; i < balls.Length; i++)
        {
            ExplosiveBallInstance instance = balls[i].GetComponentInChildren<ExplosiveBallInstance>();
            if (instance == null)
            {
                Instantiate(explosiveBallInstancePrefab, balls[i].transform.position, Quaternion.identity, balls[i].transform);
            }
            else
            {
                instance.IncrementEffect();
            }
        }
    }

    protected override void Subscribe() { }
    protected override void Unsubscribe() { }

}
