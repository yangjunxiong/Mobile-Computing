using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabinet : Unit {
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
        controller.UnitDie(gameObject);
        animator.SetTrigger(animDieName);
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(FindAnimationLength(animDieName));
        Destroy(gameObject);
    }
}
