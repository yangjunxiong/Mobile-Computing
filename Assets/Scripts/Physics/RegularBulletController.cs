using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularBulletController : Projectile {
    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == attackerTagName) {
            base.OnTriggerEnter(other);
            other.gameObject.GetComponent<Unit>().GetDamage(damage);
            Destroy(gameObject);
        }
        if (other.tag == boundaryTagName)
            Destroy(gameObject);
    }
}
