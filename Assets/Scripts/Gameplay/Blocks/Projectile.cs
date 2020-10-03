using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private GameObject EffectPrefab;

    void FixedUpdate()
    {
        movement.Move(transform);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(MyTags.Player))
        {
            Instantiate(EffectPrefab, col.transform);
            Destroy(gameObject);
        }
    }

}
