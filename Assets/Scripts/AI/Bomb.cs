using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Unit {
    public GameObject bombParticle;
    public float explosionRange = 2f;

    private GameObject toAttack = null;
    private bool isAttacking = false;

    private const float particleLastTime = 2f;

    public override void Update()
    {
        if (!toAttack)
            base.Update();
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
        if (!isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsName(animMoveName))
        {
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
        StartCoroutine(DieRoutine());
    }

    IEnumerator AttackRoutine(GameObject toAttack)
    {
        yield return new WaitForSeconds(FindAnimationLength(animAttackName));
        Instantiate(bombParticle, transform.position, Quaternion.identity).GetComponent<ParticleController>().Vanish(particleLastTime);
        GameObject[] list = controller.GetActiveUnits().ToArray();
        for (int i = 0; i < list.Length; i++) {
            if (list[i]) {
                Vector3 position = list[i].transform.position;
                Unit other = list[i].GetComponent<Unit>();
                if (!other)
                    other = list[i].GetComponentInChildren<Unit>();
                if (other.isAttacker != isAttacker && Vector3.Distance(position, transform.position) <= explosionRange)
                    other.GetDamage(damage);
            }
        }
        Instantiate(soundPlayer, Vector3.zero, Quaternion.identity).GetComponent<SoundPlayerController>().Play(attackAudio, attackAudio.length);
        isAttacking = false;
        Destroy(transform.parent.gameObject);
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(FindAnimationLength(animDieName));
        Destroy(transform.parent.gameObject);
    }
}
