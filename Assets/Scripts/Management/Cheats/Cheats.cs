using UnityEngine;

public class Cheats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitAllBlocks()
    {
        foreach (Block block in GameObject.FindObjectsOfType<Block>())
        {
            block.GetHit();
        }
    }

    public void HitPlayer()
    {
        foreach (Ball ball in FindObjectsOfType<Ball>())
        {
            ball.DestroyBall();
        }
    }
}
