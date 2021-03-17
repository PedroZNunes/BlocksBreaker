using System;
using UnityEngine;


[Serializable]
public class Health
{
    [SerializeField]
    private int max;

    public int current { get; private set; }
    public bool isDead { get { return (current <= 0); } }

    public void Initialize()
    {
        current = max;
    }

    public int GetMax()
    {
        return max;
    }

    public void SetMax(int newMaxHP) { max = newMaxHP; }

    public void GetHit(int amount){
        current-= amount;
    }

    public void GetHit()
    {
        GetHit(1);
    }

    public void HealUp()
    {
        if (current < max)
            current++;
    }


}
