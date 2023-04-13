using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{
    private enum AttackState
    {
        ClawSwipe,
        ClawLasers,
        MouthSprinkler,
        MouthSpray
    }

    [SerializeField] CrabBodyController body;
    [SerializeField] CrabArmController leftArm;
    [SerializeField] CrabArmController rightArm;
    private bool canAttack = true;
    private float timeBetweenAttacks = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CycleThroughCombat());
    }

    // Update is called once per frame
    void Update()
    {

    }



    IEnumerator CycleThroughCombat()
    {
        while (true)
        {
            if (canAttack)
            {
                // Select next attack
                Array values = Enum.GetValues(typeof(AttackState));
                System.Random random = new System.Random();
                AttackState attackState = (AttackState)values.GetValue(random.Next(values.Length));
                Debug.Log("Crab cycle " + attackState.ToString());
                // Call that attack
                Attack(attackState);
            }
            // Wait to attack again
            yield return new WaitForSecondsRealtime(timeBetweenAttacks);
        }
    }

    private void Attack(AttackState state)
    {
        switch (state)
        {
            case AttackState.ClawSwipe:
                StartCoroutine(ClawSwipe());
                break;
            case AttackState.ClawLasers:
                break;
            case AttackState.MouthSprinkler:
                break;
            case AttackState.MouthSpray:
                break;
        }
    }

    IEnumerator ClawSwipe()
    {
        canAttack = false;
        float drawbackSpeed = 1.5f;
        float swipeSpeed = 15f;

        // Pull back left
        leftArm.RotateArm(370f, drawbackSpeed);
        yield return new WaitForSecondsRealtime(3f);
        // Swing left
        leftArm.RotateArm(206f, swipeSpeed);
        yield return new WaitForSecondsRealtime(1f);
        // Pull back right
        rightArm.RotateArm(184f, drawbackSpeed);
        yield return new WaitForSecondsRealtime(3f);
        // Swing right
        rightArm.RotateArm(330f, swipeSpeed);
        yield return new WaitForSecondsRealtime(1f);
        canAttack = true;
    }


}
