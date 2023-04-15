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
    private float timeBetweenAttacks = 6.0f;
    private Coroutine attackCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        attackCoroutine =  StartCoroutine(CycleThroughCombat());
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
                SetTimeBetweenAttacks(attackState);
                Attack(attackState);
            }
            // Wait to attack again
            yield return new WaitForSecondsRealtime(timeBetweenAttacks);
        }
    }

    // To make sure the full attack carries out before the next starts.
    private void SetTimeBetweenAttacks(AttackState state)
    {
        switch (state)
        {
            case AttackState.ClawSwipe:
                timeBetweenAttacks = 3.5f;
                break;
            case AttackState.ClawLasers:
                timeBetweenAttacks = 1f;
                break;
            case AttackState.MouthSprinkler:
                timeBetweenAttacks = 8f;
                break;
            case AttackState.MouthSpray:
                timeBetweenAttacks = 8f;
                break;
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
                StartCoroutine(ClawLasers());
                break;
            case AttackState.MouthSprinkler:
                StartCoroutine(MouthSprinkler());
                break;
            case AttackState.MouthSpray:
                StartCoroutine(MouthSpray());
                break;
        }
    }

    private float GetRandomFloat(float minimum, float maximum)
    {
        System.Random random = new System.Random();
        return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    private IEnumerator ClawLasers()
    {
        canAttack = false;        
        float armMoveSpeed = 1.5f;
        // Move left arm
        float attackAngleLeft = GetRandomFloat(225f, 325f);
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(attackAngleLeft, armMoveSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(leftMove);
        // Fire left arm
        leftArm.Fire();
        yield return new WaitForSecondsRealtime(2f);
        // Move right arm
        float attackAngleRight = GetRandomFloat(210f, 315f);
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(attackAngleRight, armMoveSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(rightMove);
        // Fire right arm
        rightArm.Fire();
        yield return new WaitForSecondsRealtime(2f);

        // Do it again
        // Move left arm
        attackAngleLeft = GetRandomFloat(225f, 325f);
        leftMove = StartCoroutine(leftArm.RotateArmEnum(attackAngleLeft, armMoveSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(leftMove);
        // Fire left arm
        leftArm.Fire();
        yield return new WaitForSecondsRealtime(2f);
        // Move right arm
        attackAngleRight = GetRandomFloat(210f, 315f);
        rightMove = StartCoroutine(rightArm.RotateArmEnum(attackAngleRight, armMoveSpeed));
        yield return new WaitForSecondsRealtime(1f);
        StopCoroutine(rightMove);
        // Fire right arm
        rightArm.Fire();
        yield return new WaitForSecondsRealtime(2f);
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
