using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBulletController : Projectile {
    public GameObject particle;
    public float effectTime;

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == defenderTagName)
        {
            base.OnTriggerEnter(other);
            other.gameObject.GetComponent<Unit>().GetEffect(Unit.Effect.Vulnerable, effectTime);
            Destroy(gameObject);
            GameObject obj = Instantiate(particle, other.transform.position, other.transform.rotation);
            obj.GetComponent<ParticleController>().Vanish(effectTime);
            obj.transform.SetParent(other.transform);
        }
        if (other.tag == boundaryTagName)
            Destroy(gameObject);
    }
}
