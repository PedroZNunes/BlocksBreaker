﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ExplosiveBallInstance : MonoBehaviour
{

    [SerializeField] private GameObject explosiveEffect;
    static private Transform effectParent;
    static private int Count;
    private readonly float[] radiusMultipliers = new float[3] { 1.5f, 2f, 2.5f };
    private float effectiveRadiusMultiplier;
    private SpriteRenderer spriteRenderer;
    private int stacks = 1;
    [SerializeField] private Sprite[] sprites;


    void Awake()
    {
        Debug.Assert(explosiveEffect != null, "Explosive Effect not set in the inspector");
        spriteRenderer = GetComponent<SpriteRenderer>();
        effectParent = GameObject.FindWithTag(MyTags.Dynamic.ToString()).transform;
        Subscribe();
    }

    void Start()
    {
        Count++;
        effectiveRadiusMultiplier = radiusMultipliers[stacks - 1];
        spriteRenderer.sprite = sprites[stacks - 1];
    }


    void ActiveEffect(Collider2D blockCol, Collider2D ballCol)
    {
        if (transform.parent == ballCol.transform)
        {
            AttachToBlock(blockCol.gameObject, explosiveEffect);
            Count--;
            TestEndOfEffect();
            Destroy(gameObject);
        }
    }

    private void TestEndOfEffect()
    {
        if (Count <= 0)
        {
            Unsubscribe();
            PlayerBuffs.isExplosive = false;
        }
    }

    public void IncrementEffect()
    {
        stacks = Mathf.Clamp(stacks + 1, 1, 3);
        effectiveRadiusMultiplier = radiusMultipliers[stacks - 1];
        spriteRenderer.sprite = sprites[stacks - 1];
    }


    private void AttachToBlock(GameObject block, GameObject explosiveEffect)
    {
        GameObject effect = Instantiate(explosiveEffect, block.transform.position, Quaternion.identity, effectParent) as GameObject;
        effect.GetComponent<ExplosiveBallEffect>().SetStackAttributes(effectiveRadiusMultiplier);
    }

    void OnDisable() { Unsubscribe(); }

    private void Subscribe() { Block.GotHit += ActiveEffect; }
    private void Unsubscribe() { Block.GotHit -= ActiveEffect; }
}
