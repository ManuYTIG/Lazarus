using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class LarvaScript : MonoBehaviour
{
    public float lifeTime = 5f; // Durée de vie de la larve en secondes
    public float brokenEggDuration = 2f; // Durée pendant laquelle l'œuf cassé reste à l'écran
    public Sprite eggSprite; // Sprite de l'œuf
    public Sprite brokenEggSprite;// Sprite de l'œuf cassé
    public GameObject larvaPrefab; // Préfabriqué de la larve à instancier
    public ParticleSystem breakParticles; // Particules à jouer lors de la cassure de l'œuf
    public float recoilForce;
    public float wobbleScale = 0.9f;   // how much it squashes
    public float wobbleTime = 0.1f;    // how fast it wobbles
    private float wobbleTimer = 10f;
    private bool isWobbling = false;
    private float nextWobbleTime;
    private bool isBroken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScheduleNextWobble();
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
    public void BreakEgg(Boolean spawn)
    {
        // Change le sprite pour l'œuf cassé
        GetComponent<SpriteRenderer>().sprite = brokenEggSprite;
        // Joue les particules de cassure
        if (breakParticles != null)
        {
            breakParticles.Play();
        }
        if(spawn)
        {
            // Instancie la larve à la position de l'œuf
            Instantiate(larvaPrefab, transform.position, Quaternion.identity);
        }
        isBroken = true;
        // Détruit l'œuf après un délai pour laisser le temps aux particules de jouer
        Destroy(gameObject, brokenEggDuration);
    }
    private IEnumerator Wobble()
    {
        if (isWobbling) yield break;
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
}


