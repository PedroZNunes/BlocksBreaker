using UnityEngine;


[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Player : MonoBehaviour
{

    [SerializeField] private Health health;
    public Movement movement;

    [SerializeField] private PositionVariable playerPos;

    [SerializeField] private AudioClip takeHitClip;
    [Tooltip("Set elements according to HP")]
    [SerializeField] private Sprite[] hpSprites;
    [SerializeField] private SpriteRenderer healthSpriteRenderer;
    [SerializeField] public Transform ballRespawn;
    private AudioSource audioSource;
    private Animator animator;

    public bool isDead { get { return health.isDead; } }


    void OnValidate()
    {
        if (hpSprites.Length != health.GetMax())
        {
            hpSprites = new Sprite[health.GetMax()];
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        health.Initialize();
    }

    void Start()
    {
        UpdateSprite();
    }

    private void FixedUpdate()
    {
        movement.InputControlledMove(transform);
        playerPos.value = transform.position;
    }

    public void TakeHit()
    {
        animator.SetTrigger("TakeHit");
        audioSource.PlayOneShot(takeHitClip, 0.3f);
        health.TakeHit();
        UpdateSprite();
    }

    public void GainHP()
    {
        health.HealUp();
        UpdateSprite();
    }



    private void UpdateSprite()
    {
        if (health.isDead)
        {
            healthSpriteRenderer.sprite = null;
            animator.SetTrigger("Die");
            return;
        }
        healthSpriteRenderer.sprite = hpSprites[health.current - 1];
    }

}

public class PlayerBuffs : Player
{

    static public bool isMulti = false;
    static public bool isElectric = false;
    static public bool isExplosive = false;
    static public bool isBuffed = false;

    void Update()
    {
        isBuffed = (isMulti == true || isExplosive == true || isElectric == true);
    }


}