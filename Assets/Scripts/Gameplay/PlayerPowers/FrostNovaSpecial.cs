using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class FrostNovaSpecial : MonoBehaviour
{
    [SerializeField] BooleanVariable isPlayerFree;
    [SerializeField] BooleanVariable isShootingAllowed;
    [SerializeField] IntegerVariable screenMovementLimit_y;
    [SerializeField] FrostNovaZone instance;

    [SerializeField] float cooldown = 15;
    [SerializeField] int maxCharges = 1;

    [SerializeField] private SpriteRenderer specialLeftSprite;
    [SerializeField] private SpriteRenderer specialRightSprite;
    private int charges;

    private Transform parent;

    private AudioSource audioSource;

    private Coroutine recharging;

    private void Start()
    {
        charges = maxCharges;
        parent = GameObject.FindGameObjectWithTag(MyTags.Dynamic).transform;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (isPlayerFree.value && isShootingAllowed.value)
            {
                if (hasCharges()) 
                {
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (mousePosition.y > screenMovementLimit_y.value)
                    {
                        Instantiate(instance, new Vector3(mousePosition.x, mousePosition.y, 1), Quaternion.identity, parent);

                        specialLeftSprite.enabled = false;
                        specialRightSprite.enabled = false;

                        charges--;
                        charges--;

                        if(recharging == null)
                            recharging = StartCoroutine(Recharge());
                    }

                }

            }
        }

    }

    private IEnumerator Recharge()
    {
        yield return new WaitForSeconds(cooldown);

        specialRightSprite.enabled = true;
        specialLeftSprite.enabled = true;

        audioSource.Play();
        
        charges++;
        charges++;

        Debug.Log("Charge ++. Charges: " + charges);
        if (!fullCharge())
        {
            StartCoroutine(Recharge());
        }
        else
        {
            recharging = null;
        }

        yield return 0;
    }

    private bool hasCharges()
    {
        return (charges > 0);
    }

    private bool fullCharge()
    {
        return charges == maxCharges;
    }
}
