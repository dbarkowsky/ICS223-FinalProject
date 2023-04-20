using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

// Parent object controller for the crab
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
    private bool canAttack = false;
    private float timeBetweenAttacks = 6.0f;
    private Coroutine attackCoroutine;
    public float explosionSize = 10f;

    // Starts with no hitboxes so it can't be shot from off-screen
    void Start()
    {
        attackCoroutine =  StartCoroutine(CycleThroughCombat());
        SetHitBoxes(false);
    }

    // Turns hitboxes on or off
    public void SetHitBoxes(bool state)
    {
        // turn on/off hitboxes
        body.GetComponent<PolygonCollider2D>().enabled = state;
        leftArm.GetComponent<PolygonCollider2D>().enabled = state;
        rightArm.GetComponent<PolygonCollider2D>().enabled = state;
    }

    // As long as it's alive and can attack, cycles through the AttackState options then attacks
    IEnumerator CycleThroughCombat()
    {
        while (body.hp > 0)
        {
            if (canAttack)
            {
                // Select next attack
                Array values = Enum.GetValues(typeof(AttackState));
                System.Random random = new System.Random();
                AttackState attackState = (AttackState)values.GetValue(random.Next(values.Length));
                // Call that attack
                SetTimeBetweenAttacks(attackState);
                Attack(attackState);
            }
            // Wait to attack again
            yield return new WaitForSeconds(timeBetweenAttacks);
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

    // Starts attack-related coroutines
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

    // Gives a random float between a min and max
    private float GetRandomFloat(float minimum, float maximum)
    {
        System.Random random = new System.Random();
        return (float)(random.NextDouble() * (maximum - minimum) + minimum);
    }

    // Sets the canAttack boolean
    public void Fight(bool canFight)
    {
        canAttack = canFight;
    }

    // Fires the claw lasers
    private IEnumerator ClawLasers()
    {
        canAttack = false;        
        float armMoveSpeed = 1.5f;
        // Move left arm
        float attackAngleLeft = GetRandomFloat(225f, 325f);
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(attackAngleLeft, armMoveSpeed));
        yield return new WaitForSeconds(1f);
        StopCoroutine(leftMove);
        // Fire left arm
        leftArm.Fire();
        yield return new WaitForSeconds(2f);
        // Move right arm
        float attackAngleRight = GetRandomFloat(210f, 315f);
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(attackAngleRight, armMoveSpeed));
        yield return new WaitForSeconds(1f);
        StopCoroutine(rightMove);
        // Fire right arm
        rightArm.Fire();
        yield return new WaitForSeconds(2f);

        // Do it again
        // Move left arm
        attackAngleLeft = GetRandomFloat(225f, 325f);
        leftMove = StartCoroutine(leftArm.RotateArmEnum(attackAngleLeft, armMoveSpeed));
        yield return new WaitForSeconds(1f);
        StopCoroutine(leftMove);
        // Fire left arm
        leftArm.Fire();
        yield return new WaitForSeconds(2f);
        // Move right arm
        attackAngleRight = GetRandomFloat(210f, 315f);
        rightMove = StartCoroutine(rightArm.RotateArmEnum(attackAngleRight, armMoveSpeed));
        yield return new WaitForSeconds(1f);
        StopCoroutine(rightMove);
        // Fire right arm
        rightArm.Fire();
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    // Fires the mouth sprinkler
    private IEnumerator MouthSprinkler()
    {
        canAttack = false;
        // Pull claws in
        float armMoveSpeed = 1.5f;
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(290f, armMoveSpeed));
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(260f, armMoveSpeed));
        yield return new WaitForSeconds(2f);
        StopCoroutine(rightMove);
        StopCoroutine(leftMove);

        // Shoot
        body.SetFiringPattern(FiringPattern.CrabMouthSprinkler);
        body.SetFiringRepetitions(1);
        body.animationTime = 6f;
        StartCoroutine(body.Fire());
        canAttack = true;
    }

    // Fires the mouth spray
    private IEnumerator MouthSpray()
    {
        canAttack = false;
        // Pull claws in
        float armMoveSpeed = 1.5f;
        var leftMove = StartCoroutine(leftArm.RotateArmEnum(228f, armMoveSpeed));
        var rightMove = StartCoroutine(rightArm.RotateArmEnum(314f, armMoveSpeed));
        yield return new WaitForSeconds(2f);
        StopCoroutine(rightMove);
        StopCoroutine(leftMove);

        // Shoot
        body.SetFiringPattern(FiringPattern.CrabMouthSpray);
        body.SetFiringRepetitions(3);
        body.animationTime = 4f;
        StartCoroutine(body.Fire());
        canAttack = true;
    }

    // Activates the claw swipes
    IEnumerator ClawSwipe()
    {
        canAttack = false;
        float drawbackSpeed = 1.5f;
        float swipeSpeed = 15f;

        // NOTE: Must stop coroutines after. Claw approaches but never reaches goal, so coroutine must be manually stopped.
        // Otherwise, it interferes with future claw movement.

        // Pull back left
        var lastAttack = StartCoroutine(leftArm.RotateArmEnum(370f, drawbackSpeed));
        yield return new WaitForSeconds(3f);
        StopCoroutine(lastAttack);
        // Swing left
        lastAttack = StartCoroutine(leftArm.RotateArmEnum(206f, swipeSpeed));
        Messenger.Broadcast(GameEvent.CLAW_SWIPE);
        yield return new WaitForSeconds(1f);
        StopCoroutine(lastAttack);
        // Pull back right
        lastAttack = StartCoroutine(rightArm.RotateArmEnum(184f, drawbackSpeed));
        yield return new WaitForSeconds(3f);
        StopCoroutine(lastAttack);
        // Swing right
        lastAttack = StartCoroutine(rightArm.RotateArmEnum(330f, swipeSpeed));
        Messenger.Broadcast(GameEvent.CLAW_SWIPE);
        yield return new WaitForSeconds(1f);
        StopCoroutine(lastAttack);
        canAttack = true;
    }

    

    private void Awake()
    {
        Messenger.AddListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.CRAB_DESTROYED, this.OnCrabDestroyed);
    }

    // Turn off hitboxes and stop attack coroutines
    private void OnCrabDestroyed()
    {
        StopAllCoroutines();
        canAttack = false;
        body.canShoot = false;
        leftArm.canShoot = false;
        rightArm.canShoot = false;
        SetHitBoxes(false);
    }
}
