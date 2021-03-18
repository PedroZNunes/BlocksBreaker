using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostNovaZone : MonoBehaviour
{

    [SerializeField] private float duration = 5f;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float effectRadius =3f;

    [SerializeField] private GameObject frostInstancePrefab;

    private Dictionary<Block, FrostNovaBlock> instancesList;


    // Start is called before the first frame update
    private void Start()
    {
        instancesList = new Dictionary<Block, FrostNovaBlock>();
        //collider thing, gets all blocks adding each to the dictionary.
        Collider2D[] affectedBlocks = Physics2D.OverlapCircleAll(transform.position, effectRadius,layerMask);

        foreach (Collider2D blockCol in affectedBlocks)
        {
            Block block = blockCol.GetComponent<Block>();
            if(block != null){ 
                //instances a frost effect on the block and adds it to the same row as their respective block
                GameObject frostInstance = Instantiate(frostInstancePrefab, blockCol.bounds.center, blockCol.transform.rotation, transform);
                FrostNovaBlock frostBlock = frostInstance.GetComponent<FrostNovaBlock>();
                if (frostBlock != null){
                    PlatformEffector2D effector = block.gameObject.AddComponent<PlatformEffector2D>();
                    effector.surfaceArc = 0;
                    Subscribe(block);
                    instancesList.Add(block, frostBlock);
                }
            }
        }
    }


    private void OnBlockGettingHit(Block block, ref int damageAmount)
    {
        damageAmount = 10;  
        
        //kill block and tell instance to shatter
        DestroyFrost(instancesList[block]);
        instancesList.Remove(block);
        Unsubscribe(block);
    }


    // Update is called once per frame
    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            TimeExpired();
        }
    }

    private void TimeExpired()
    {
        foreach (KeyValuePair<Block, FrostNovaBlock> instance in instancesList)
        {
            PlatformEffector2D effector = instance.Key.GetComponent<PlatformEffector2D>();
            Destroy(effector);

            DestroyFrost(instance.Value);
            Unsubscribe(instance.Key);
        }
        //delete entry from dictionary
        Destroy(this.gameObject);
    }


    private void DestroyFrost(FrostNovaBlock instance)
    {
        //TODO: shatter ice animation
        Destroy(instance.gameObject);
    }


    private void Subscribe(Block block) 
    {
        block.ThisGettingHit += OnBlockGettingHit;
    }   


    private void Unsubscribe(Block block) 
    {
        block.ThisGettingHit -= OnBlockGettingHit;
    }
}
