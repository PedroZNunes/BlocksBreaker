using System;
using UnityEngine;


[Serializable]
public class Health
{
    [SerializeField]
    private int max;

    public int current { get; private set; }
    public bool isDead { get { return (current <= 0); } }

    public void Initialize(){
        current = max;
    }

    public int GetMax()
    {
        return max;
    }

    public void SetMax(int newMaxHP) { max = newMaxHP; }

    public void TakeHit()
    {
        current--;
    }

    public void TakeHit( out bool isDead )
    {
        TakeHit();
        isDead = this.isDead;
    }

    public void HealUp()
    {
        if (current < max)
            current++; 
    }

    
}
