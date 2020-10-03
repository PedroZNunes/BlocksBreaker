using UnityEngine;

public class SlowPlayerEffect : MonoBehaviour
{
    [SerializeField] private float slowAmountPercent = 0.70f;
    [SerializeField] private float duration = 2f;
    private float currentDuration = 0f;

    [SerializeField] private bool isStackable = true;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Player player = GetComponentInParent<Player>();
        if (player.movement != null)
        {
            //check for stackability here
            player.movement.CheckForDebuffsEvent += Slow;
        }
    }

    private void OnDisable()
    {
        Player player = GetComponentInParent<Player>();
        if (player.movement != null)
        {
            player.movement.CheckForDebuffsEvent -= Slow;
        }
    }

    private void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > duration)
            Destroy(gameObject);
    }

    private void Slow(ref float speed)
    {
        speed = speed - speed * slowAmountPercent;
    }
}
