using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public int maxHealth;
    public int damage;
    public float speed;
    public float attackRange;
    public bool canMove;
    public bool isAttacker;
    public AudioClip attackAudio, dieAudio;
    public GameObject soundPlayer;

    public enum Effect { None, Slow, Stop, Vulnerable };

    protected Rigidbody rb;
    protected GameController controller;
    protected Animator animator;
    protected TileMouseHandle assignedTile = null;
    protected Renderer renderer;
    protected AudioSource audioSource;
    protected HealthBarController healthBarController;
    protected Effect effect;
    protected Color originalColor;
    protected int health;
    protected int[] effectCounter;  // 0 - slow, 1 - vulnerable

    protected const int slowEffectRatio = 5;
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
        audioSource = GetComponent<AudioSource>();
        healthBarController = GetComponentInChildren<HealthBarController>();
        health = maxHealth;
        transform.LookAt(new Vector3(transform.position.x, transform.position.y, isAttacker? attackerFaceDirection : defenderFaceDirection));
        speed = isAttacker ? -speed : speed;
        originalColor = renderer.material.color;
        effectCounter = new int[2];
        effectCounter[0] = 0;
        effectCounter[1] = 0;
    }

    public virtual void Update() {
        if (canMove) {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
        }

        if (isAttacker && transform.position.z <= controller.GetFinishPoint().position.z)
            controller.RequestGameOver(controller.isAttacker);
    }

    public virtual void GetDamage(int damage) {
        StartCoroutine(GetAttackRoutine());
    }

    public virtual void GetEffect(Effect effect, float time) {
        switch (effect) {
            case Effect.Slow:
                if (effectCounter[0] <= 0)
                    speed /= slowEffectRatio;
                effectCounter[0]++;
                break;
            case Effect.Stop:
                canMove = false;
                break;
            case Effect.Vulnerable:
                effectCounter[1]++;
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
                if (effectCounter[0] == 1) {
                    speed *= slowEffectRatio;
                    this.effect = Effect.None;
                }
                effectCounter[0]--;
                break;
            case Effect.Stop:
                canMove = true;
                break;
            case Effect.Vulnerable:
                effectCounter[1]--;
                if (effectCounter[1] == 0)
                    this.effect = Effect.None;
                break;
        }
    }

    public virtual void Attack() { }

    public virtual void Die() {
        audioSource.clip = dieAudio;
        audioSource.Play();
    }

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
            if (list[i] == null)
                continue;
            Vector3 position = list[i].transform.position;
            Unit other = list[i].GetComponent<Unit>();
            if (!other)
                other = list[i].GetComponentInChildren<Unit>();
            if (other.isAttacker != isAttacker && position.x == transform.position.x) {
                float distance = Mathf.Abs(transform.position.z - position.z);
                if (distance <= attackRange && distance < minDistance) {
                    minDistance = distance;
                    toAttack = list[i];
                }
            }
        }
        return toAttack;
    }

    protected IEnumerator GetAttackRoutine() {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(getAttackHighlightTime);
        renderer.material.color = originalColor;
        healthBarController.SetScale((float)health/maxHealth);
    }

    protected IEnumerator GetEffectRoutine(Effect effect, float time) {
        yield return new WaitForSeconds(time);
        EndEffect(effect);
    }
}
