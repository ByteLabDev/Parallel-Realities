using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Mirror;

public class GunController : NetworkBehaviour
{
    public TwoBoneIKConstraint handIK;
    public float loopDelay;
    
    [SyncVar(hook = nameof(ChangeAimWeight))]
    public float weight = 0;

    
    private bool previouslyAiming = false;
    private bool previouslyShooting = false;
    
    void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.GetButton("Fire2"))
        {
            if (previouslyAiming == false)
            {
                raiseArmCommand();
                previouslyAiming = true;
            }

            if (Input.GetButton("Fire1"))
            {

                if (previouslyShooting == false)
                {
                    Debug.Log("Shooting.");
                    previouslyShooting = true;
                }
            }
            else
            {
                previouslyShooting = false;
            }
        }
        else
        {
            if (previouslyAiming == true)
            {
                lowerArmCommand();
                previouslyAiming = false;
            }
        }
    }

    IEnumerator raiseArm()
    {
        float handWeight = 0;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(loopDelay);

            handWeight += 0.1f;
            weight += 0.1f;
            if (i == 10) weight = 1;
            //handIK.weight = handWeight;
        }
    }

    IEnumerator lowerArm()
    {
        float handWeight = 1;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(loopDelay);
            handWeight -= 0.1f;
            if (i == 9) weight = 0;
            else weight -= 0.1f;
            //handIK.weight = handWeight;
        }
    }

    [Command] 
    void raiseArmCommand()
    {
        StartCoroutine(raiseArm());
    }

    [Command]
    void lowerArmCommand() 
    {
        StartCoroutine(lowerArm());
    }


    void ChangeAimWeight(float oldWeight, float newWeight)
    {
        handIK.weight = newWeight;
    }
}
