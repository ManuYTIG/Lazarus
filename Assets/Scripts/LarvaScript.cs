using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class LarvaScript : MonoBehaviour
{
    public float lifeTime = 5f; // Dur�e de vie de la larve en secondes
    public float brokenEggDuration = 2f; // Dur�e pendant laquelle l'�uf cass� reste � l'�cran
    public Sprite eggSprite; // Sprite de l'�uf
    public Sprite brokenEggSprite;// Sprite de l'�uf cass�
    public GameObject larvaPrefab; // Pr�fabriqu� de la larve � instancier
    public GameObject breakParticles; // Particules � jouer lors de la cassure de l'�uf
    public float recoilForce;
    public float wobbleScale = 0.9f;   // how much it squashes
    public float wobbleTime = 0.1f;    // how fast it wobbles
    private float wobbleTimer = 10f;
    private bool isWobbling = false;
    private float nextWobbleTime;
    private bool isBroken = false;
    public float particleTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isBroken = false;
        GetComponent<SpriteRenderer>().sprite = eggSprite;
        ScheduleNextWobble(); // sets nextWobbleTime to a proper random value
    }

    // Update is called once per frame
    void Update()
    {
        // Lifetime countdown
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                BreakEgg(true);
            }
        }

        // Wobble countdown
        if (!isWobbling && !isBroken)
        {
            nextWobbleTime -= Time.deltaTime;
            if (nextWobbleTime <= 0f)
            {
                StartCoroutine(Wobble());
                ScheduleNextWobble();
            }
        }
    }
    public void BreakEgg(bool spawn)
    {
        if (isBroken) return; // ← prevent double-call
        isBroken = true;

        GetComponent<SpriteRenderer>().sprite = brokenEggSprite;

        if (breakParticles != null) SpawnParticle();

        if (spawn)
        Instantiate(larvaPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject, brokenEggDuration);
    }
    private IEnumerator Wobble()
    {
        if (isWobbling || isBroken) yield break;
        isWobbling = true;

        Vector3 originalScale = transform.localScale;
        Vector3 squashed = new Vector3(wobbleScale, 1f / wobbleScale, 1f);

        // Squash
        transform.localScale = squashed;
        yield return new WaitForSeconds(wobbleTime);

        // Stretch
        transform.localScale = new Vector3(1f / wobbleScale, wobbleScale, 1f);
        yield return new WaitForSeconds(wobbleTime);

        // Reset
        transform.localScale = originalScale;

        isWobbling = false;
    }
    void ScheduleNextWobble()
    {
        // Random delay between 0.5 and 2 seconds (adjust as you like)
        nextWobbleTime = UnityEngine.Random.Range(0.5f, 2f);
    }
    private void SpawnParticle()
    {
        GameObject p = Instantiate(breakParticles, transform.position, Quaternion.identity);
        Destroy(p, particleTime);
    }
}


