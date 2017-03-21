using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    public int damage;

    protected Rigidbody rb;

    protected const string attackerTagName = "Attacker";
    protected const string defenderTagName = "Defender";
    protected const string boundaryTagName = "Boundary";

    protected virtual void Start() {
        rb = GetComponent<Rigidbody>();
    }

    protected virtual void Update() {
        rb.velocity = transform.forward * speed;
    }

    public virtual void Rotate(float xAngle, float yAngle, float zAngle) {
        transform.Rotate(xAngle, yAngle, zAngle);
    }

    public float GetSpeed() {
        return speed;
    }

    public int GetDamage() {
        return damage;
    }

    public void SetSpeed(float speed) {
        this.speed = speed;
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }
}
