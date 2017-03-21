using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public int health;
    public int damage;
    public float speed;
    public float attackRange;
    public bool canMove;
    public bool isAttacker;

    public enum Effect { None, Slow, Stop, Vulnerable };

    protected Rigidbody rb;
    protected GameController controller;
    protected Animator animator;
    protected TileMouseHandle assignedTile = null;
    protected Renderer renderer;
    protected Effect effect;

    protected const float attackerFaceDirection = -20f;
    protected const float defenderFaceDirection = 20f;
    protected const float getAttackHighlightTime = 0.25f;

    protected const string attackerTagName = "Attacker";
    protected const string defenderTagName = "Defender";
    protected const string gameControllerTagName = "GameController";
    protected const string animIdleName = "Idle";
    protected const string animMoveName = "Move";
    protected const string animAttackName = "Attack";
    protected const string animDieName = "Die";

    public virtual void Start() {
        rb = GetComponent<Rigidbody>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
    }

    public virtual void Update() {
        if (canMove)
            rb.velocity = transform.forward * speed;

        if (isAttacker && transform.position.z <= controller.GetFinishPoint().position.z)
            controller.RequestGameOver(controller.isAttacker);
    }

    public virtual void GetDamage(int damage) {
        if (effect == Effect.Vulnerable)
            damage *= 2;
        StartCoroutine(GetAttackRoutine());
    }

    public virtual void GetEffect(Effect effect, float time) {
        if (this.effect != Effect.None)
            EndEffect(this.effect);
        switch (effect) {
            case Effect.Slow:
                speed /= 2;
                break;
            case Effect.Stop:
                canMove = false;
                break;
        }
        this.effect = effect;
        StartCoroutine(GetEffectRoutine(effect, time));
    }

    public virtual void EndEffect(Effect effect) {
        if (this.effect != effect)
            return;
        switch (effect) {
            case Effect.Slow:
                speed *= 2;
                break;
            case Effect.Stop:
                canMove = true;
                break;
        }
        this.effect = Effect.None;
    }

    public virtual void Attack() { }

    public virtual void Die() { }

    public void SetAssignedTile(TileMouseHandle tile) {
        assignedTile = tile;
    }

    public TileMouseHandle GetAssignedTile() {
        return assignedTile;
    }

    protected float FindAnimationLength(string name) {
        RuntimeAnimatorController ac = GetComponent<Animator>().runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == name)
                return ac.animationClips[i].length;
        return 0f;
    }

    protected GameObject FindEnemyInAttackRange() {
        GameObject[] list = controller.GetActiveUnits().ToArray();
        GameObject toAttack = null;
        float minDistance = 1000f;
        for (int i = 0; i < list.Length; i++) {
            Vector3 position = list[i].transform.position;
            if (list[i].GetComponent<Unit>().isAttacker != isAttacker && position.x == transform.position.x) {
                float distance = position.z - transform.position.z;
                if (distance <= attackRange && distance < minDistance) {
                    minDistance = position.z - transform.position.z;
                    toAttack = list[i];
                }
            }
        }
        return toAttack;
    }

    protected IEnumerator GetAttackRoutine() {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(getAttackHighlightTime);
        renderer.material.color = Color.white;
    }

    protected IEnumerator GetEffectRoutine(Effect effect, float time) {
        yield return new WaitForSeconds(time);
        EndEffect(effect);
    }
}
