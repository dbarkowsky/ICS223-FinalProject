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
    private float timeBetweenAttacks = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CycleThroughCombat());
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
                //Attack(AttackState.ClawLasers);
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
                //StartCoroutine(ClawLasers());
                break;
            case AttackState.MouthSprinkler:
                StartCoroutine(MouthSprinkler());
                break;
            case AttackState.MouthSpray:
                StartCoroutine(MouthSpray());
                break;
        }
    }

    private IEnumerator ClawLasers()
    {
        canAttack = false;
        // Move claws

        // Fire lasers
        for (int iterations = 0; iterations < 20; iterations++)
        {
            leftArm.Fire();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        for (int iterations = 0; iterations < 20; iterations++)
        {
            rightArm.Fire();
            yield return new WaitForSecondsRealtime(0.05f);
        }
        canAttack = true;
    }

    private IEnumerator MouthSprinkler()
    {
        canAttack = false;
        // Pull claws in
        float armMoveSpeed = 1.5f;
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(258f, armMoveSpeed));
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(281f, armMoveSpeed));
        yield return new WaitForSecondsRealtime(2f);
        StopCoroutine(rightMove);
        StopCoroutine(leftMove);

        // Shoot
        body.SetFiringPattern(FiringPattern.CrabMouthSprinkler);
        body.SetFiringRepetitions(2);
        body.animationTime = 10f;
        StartCoroutine(body.Fire());
        canAttack = true;
    }

    private IEnumerator MouthSpray()
    {
        canAttack = false;
        // Pull claws in
        float armMoveSpeed = 1.5f;
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(228f, armMoveSpeed));
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(314f, armMoveSpeed));
        yield return new WaitForSecondsRealtime(2f);
        StopCoroutine(rightMove);
        StopCoroutine(leftMove);

        // Shoot
        body.SetFiringPattern(FiringPattern.CrabMouthSpray);
        body.SetFiringRepetitions(3);
        body.animationTime = 4f;
        StartCoroutine(body.Fire());
        canAttack = true;
    }

    IEnumerator ClawSwipe()
    {
        canAttack = false;
        float drawbackSpeed = 1.5f;
        float swipeSpeed = 15f;

        // NOTE: Must stop coroutines after. Claw approaches but never reaches goal, so coroutine must be manually stopped.
        // Otherwise, it interferes with future claw movement.

        // Pull back left
        var lastAttack = StartCoroutine(leftArm.RotateArmEnum(370f, drawbackSpeed));
        yield return new WaitForSecondsRealtime(3f);
        StopCoroutine(lastAttack);
        // Swing left
        lastAttack = StartCoroutine(leftArm.RotateArmEnum(206f, swipeSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(lastAttack);
        // Pull back right
        lastAttack = StartCoroutine(rightArm.RotateArmEnum(184f, drawbackSpeed));
        yield return new WaitForSecondsRealtime(3f);
        StopCoroutine(lastAttack);
        // Swing right
        lastAttack = StartCoroutine(rightArm.RotateArmEnum(330f, swipeSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(lastAttack);
        canAttack = true;
    }


}
