using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class GravityManipulator : MonoBehaviour
{
    [SerializeField] BooleanVariable isPlayerFree;
    [SerializeField] BooleanVariable isShootingAllowed;
    [SerializeField] IntegerVariable screenMovementLimit_y;
    [SerializeField] GravityPullInstance instance;

    [SerializeField] float cooldown = 10;
    [SerializeField] int maxCharges = 2;

    [SerializeField] private SpriteRenderer specialLeftSprite;
    [SerializeField] private SpriteRenderer specialRightSprite;
    private int charges;

    private Transform parent;
    private AudioSource audio;

    private Coroutine Recharging;

    private void Start()
    {
        charges = maxCharges;
        parent = GameObject.FindGameObjectWithTag(MyTags.Dynamic).transform;
        audio = GetComponent<AudioSource>();
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

                        if (!specialRightSprite.enabled)
                            specialLeftSprite.enabled = false;
                        else
                            specialRightSprite.enabled = false;

                        charges--;

                        if(Recharging == null)
                            Recharging = StartCoroutine(Recharge());
                    }

                }

            }
        }

    }

    private IEnumerator Recharge()
    {
        yield return new WaitForSeconds(cooldown);

        if (specialLeftSprite.enabled)
            specialRightSprite.enabled = true;
        else
            specialLeftSprite.enabled = true;

        audio.Play();
        charges++;

        Debug.Log("Charge ++. Charges: " + charges);
        if (!fullCharge())
        {
            StartCoroutine(Recharge());
        }
        else
        {
            Recharging = null;
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
