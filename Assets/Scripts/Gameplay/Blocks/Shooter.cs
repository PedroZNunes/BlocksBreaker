using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    private float currentCooldown;
    [SerializeField] private float Cooldown;

    [SerializeField] private BooleanVariable isShootingAllowed;
    [SerializeField] private PositionVariable playerPos;

    static private Transform dynamicParent;

    // Update is called once per frame
    private void Awake()
    {
        currentCooldown = Random.Range(0f, Cooldown / 2f);
        if (dynamicParent == null)
        {
            dynamicParent = GameObject.FindGameObjectWithTag(MyTags.Dynamic).transform;
        }
    }

    void FixedUpdate()
    {
        if (!isShootingAllowed.value)
            return;

        currentCooldown += Time.deltaTime;
        if (currentCooldown >= Cooldown)
        {
            Shoot();

            currentCooldown = 0f;
        }
    }

    private void Shoot()
    {
        //rotate projectile
        Vector3 aimTowards = playerPos.value - this.transform.position;
        //fire
        //Debug.Log("Instantiating shot prefab");
        Instantiate(ProjectilePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, aimTowards), dynamicParent);

    }



}
