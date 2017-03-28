using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Unit {
    public float fireRate;

    private GameObject toAttack = null;
    private float nextFire;
    private bool isAttacking = false;

    public override void Update()
    {
        if (!toAttack)
            base.Update();
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
        if (!isAttacking && Time.time > nextFire && animator.GetCurrentAnimatorStateInfo(0).IsName(animMoveName))
        {
            if (!toAttack)
                toAttack = FindEnemyInAttackRange();
            if (toAttack) {
                animator.SetTrigger(animAttackName);
                isAttacking = true;
                StartCoroutine(AttackRoutine(toAttack));
            }
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
                Die();
        }
    }

    public override void Die()
    {
        base.Die();
        controller.UnitDie(transform.parent.gameObject);
        animator.SetTrigger(animDieName);
        nextFire = Time.time + 1000f;
        StartCoroutine(DieRoutine());
    }

    IEnumerator AttackRoutine(GameObject toAttack)
    {
        yield return new WaitForSeconds(FindAnimationLength(animAttackName));
        toAttack.GetComponentInChildren<Unit>().GetDamage(damage);
        nextFire = Time.time + fireRate;
        isAttacking = false;
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(FindAnimationLength(animDieName));
        Destroy(transform.parent.gameObject);
    }
}
