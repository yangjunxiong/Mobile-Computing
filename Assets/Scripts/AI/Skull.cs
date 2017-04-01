using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Unit {
    public GameObject bullet;
    public Transform bulletSpawn;
    public float fireRate;

    private float nextFire;
    private bool isAttacking = false;

    public override void Update()
    {
        base.Update();
        Attack();
    }

    public override void Attack()
    {
        base.Attack();
        if (!isAttacking && Time.time > nextFire && animator.GetCurrentAnimatorStateInfo(0).IsName(animIdleName))
        {
            animator.SetTrigger(animAttackName);
            isAttacking = true;
            StartCoroutine(AttackRoutine());
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

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(FindAnimationLength(animAttackName));
        Instantiate(bullet, bulletSpawn.position, Quaternion.identity).GetComponent<Projectile>().Rotate(0f, 180f, 0f);
        nextFire = Time.time + fireRate;
        audioSource.clip = attackAudio;
        audioSource.Play();
        isAttacking = false;
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(FindAnimationLength(animDieName));
        Destroy(transform.parent.gameObject);
    }
}
