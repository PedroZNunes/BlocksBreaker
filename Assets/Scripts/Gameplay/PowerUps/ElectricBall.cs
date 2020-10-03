using UnityEngine;


public class ElectricBall : PowerUp
{

    [SerializeField] private ElectricBallInstance electricBallInstance;

    public override void OnPickUp()
    {
        Transform parent = FindObjectOfType<PowerUpManager>().transform;
        Instantiate(electricBallInstance, parent);
    }


    protected override void Subscribe() { }
    protected override void Unsubscribe() { }
}
