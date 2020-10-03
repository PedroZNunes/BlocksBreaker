using UnityEngine;

public class GainHP : PowerUp
{

    private Player player;
    // Use this for initialization

    public override void OnPickUp()
    {
        player = FindObjectOfType<Player>();
        Debug.Assert(player != null, "player not found");
        player.GainHP();
    }

    protected override void Subscribe() { }

    protected override void Unsubscribe() { }

}
