using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilBulletController : Projectile {
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == attackerTagName) {
            other.gameObject.GetComponent<Unit>().GetDamage(damage);
            Destroy(gameObject);
        }
        if (other.tag == boundaryTagName)
            Destroy(gameObject);
    }
}
