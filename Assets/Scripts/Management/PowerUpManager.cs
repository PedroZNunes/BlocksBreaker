using UnityEngine;

using Random = UnityEngine.Random;


public class PowerUpManager : MonoBehaviour
{

    [SerializeField] private PowerUp[] powerUps;

    private float currentProcTime = 0f;
    private float currentProcThreshold = 0f;
    private readonly float bonusProcPerTime = 3f;

    private static readonly Transform dropInstanceParent;


    void OnEnable()
    {
        Block.PowerUpDropEvent += DropPowerUp;
        PickUpItem.PowerUpPickedUpEvent += OnPowerUpPickedUp;
    }
    void OnDisable()
    {
        Block.PowerUpDropEvent -= DropPowerUp;
        PickUpItem.PowerUpPickedUpEvent -= OnPowerUpPickedUp;
    }

    public void OnPowerUpPickedUp(PowerUpsEnum name)
    {
        foreach (PowerUp powerUp in powerUps)
        {
            if (powerUp.GetName() == name)
            {
                powerUp.OnPickUp();
            }
        }
    }


    public void IncrementTimer()
    {
        currentProcTime += Time.deltaTime;
    }

    /// <summary>
    /// The proc chance scales with time (currentproctime) and with each block destroyed (blockbonusproc). after you roll a d100 that goes
    /// below that limit, you roll another dice for deciding which power up actually drops.
    /// </summary>
    /// <param name="blockPosition">Block Position. This is where the object is instantiated from.</param>
    /// <param name="blockBonusProc">This is a bonus percentage to the proc chance. It comes from each block destroyed and vary according to each blocks hp (for now just hp).</param>
    void DropPowerUp(Vector3 blockPosition, float blockBonusProc)
    {


        float bonusProc = currentProcTime * bonusProcPerTime;
        currentProcTime = 0f;

        currentProcThreshold += blockBonusProc + bonusProc;

        float dropDice = Random.Range(0f, 100f);
        if (!PlayerBuffs.isBuffed)
            dropDice = dropDice / 2f;

        if (dropDice <= currentProcThreshold)
        {
            print("PowerUp dropped at a " + (currentProcThreshold) + "% chance.");
            //it is going to drop something. just a matter of sorting what will be droped
            //total percentage is the sum of all chances in all objects in inspector... may or may not be 100...
            float totalPercentage = 0;
            foreach (PowerUp powerup in powerUps)
            {
                totalPercentage += powerup.GetDropChance();
            }

            if (!Mathf.Approximately(totalPercentage, 100f))
            {
                Debug.LogWarning("The sum of all power up drop chances does not amount to 100.");
            }

            float powerUpDice = Random.Range(0f, totalPercentage);

            //for each power up, reduce the drop chance from this until its 0 or lower
            for (int i = 0; i < powerUps.Length; i++)
            {
                powerUpDice -= powerUps[i].GetDropChance();
                //drop the power up
                if (powerUpDice <= 0f)
                {
                    powerUps[i].Drop(blockPosition);
                    break;
                }
            }

            currentProcThreshold = 0f;
        }
    }

}
