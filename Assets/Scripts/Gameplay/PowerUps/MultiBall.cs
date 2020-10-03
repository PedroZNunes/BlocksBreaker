using UnityEngine;


public class MultiBall : PowerUp
{

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject multiBallInstancePrefab;

    private readonly GameObject multiballEffect;

    void OnDisable() { Unsubscribe(); }

    public override void OnPickUp()
    {
        PlayerBuffs.isMulti = true;
        Ball[] balls = FindObjectsOfType<Ball>();
        for (int i = 0; i < balls.Length; i++)
        {
            MultiBallInstance instance = balls[i].GetComponentInChildren<MultiBallInstance>();
            if (instance == null)
            {
                Instantiate(multiBallInstancePrefab, balls[i].transform.position, Quaternion.identity, balls[i].transform);
            }
        }
    }

    protected override void Subscribe() { }
    protected override void Unsubscribe() { }

}