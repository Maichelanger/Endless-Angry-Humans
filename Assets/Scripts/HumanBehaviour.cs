using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    [SerializeField] private BoxCollider attackingHandCollider;
    [SerializeField] private float playerPositionCheckInterval = 0.5f;
    [SerializeField] private float health = 30f;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioSource fxAudioSource;
    [SerializeField] private AudioSource deathAudioSource;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform playerPos;
    private float persecuteTimer = 0f;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool invincible = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        attackingHandCollider.enabled = false;

        agent.SetDestination(playerPos.position);
    }

    private void Update()
    {
        if (isDead) return;

        SendSpeedToAnim();
        CheckIfAttackPlayer();
        GoToPlayer();
    }

    private void SendSpeedToAnim()
    {
        float speed = agent.velocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void CheckIfAttackPlayer()
    {
        if (isAttacking) return;

        float stoppingDistanceWithBuffer = agent.stoppingDistance + 1f;
        if (Vector3.Distance(transform.position, playerPos.position) <= stoppingDistanceWithBuffer)
        {
            animator.SetTrigger("Attack");
        }
    }

    private void GoToPlayer()
    {
        persecuteTimer += Time.deltaTime;
        if (persecuteTimer >= playerPositionCheckInterval)
        {
            agent.SetDestination(playerPos.position);
            persecuteTimer = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Hands") && !isDead)
        {
            TakeDamage(10);
        }
    }

    private void TakeDamage(float damage)
    {
        if (invincible || isDead) return;

        PlayHitSound();

        health -= damage;
        invincible = true;
        StartCoroutine(InvincibleCooldown());

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        PlayDeathSound();

        animator.SetBool("IsDead", true);
        animator.SetTrigger("Die");
        animator.SetFloat("Speed", 0);
        agent.isStopped = true;
        attackingHandCollider.enabled = false;

        Destroy(gameObject, 5f);
    }

    internal void PlayAttackSound()
    {
        fxAudioSource.pitch = Random.Range(0.8f, 1.2f);
        fxAudioSource.PlayOneShot(attackSound);
    }

    private void PlayHitSound()
    {
        fxAudioSource.pitch = Random.Range(0.8f, 1.2f);
        fxAudioSource.PlayOneShot(hitSound);
    }

    private void PlayDeathSound()
    {
        fxAudioSource.Stop();
        deathAudioSource.PlayOneShot(deathSound);
    }

    public void StartingAttackAnim()
    {
        attackingHandCollider.enabled = true;
        isAttacking = true;
    }

    public void EndingAttackAnim()
    {
        attackingHandCollider.enabled = false;
        isAttacking = false;
    }

    IEnumerator InvincibleCooldown()
    {
        yield return new WaitForSeconds(1f);
        invincible = false;
    }
}
