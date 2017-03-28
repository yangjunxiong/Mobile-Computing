using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingBulletController : Projectile {
    public GameObject particle;
    public float slowingTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == attackerTagName)
        {
            other.gameObject.GetComponent<Unit>().GetEffect(Unit.Effect.Slow, slowingTime);
            Destroy(gameObject);
            GameObject obj = Instantiate(particle, other.transform.position, other.transform.rotation);
            obj.GetComponent<ParticleController>().Vanish(slowingTime);
            obj.transform.SetParent(other.transform);
        }
        if (other.tag == boundaryTagName)
            Destroy(gameObject);
    }
}
