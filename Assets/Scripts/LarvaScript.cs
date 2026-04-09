using System;
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
    public Health health; // Référence au composant de santé de la larve
    public float recoilForce;
    public float wobbleScale = 0.9f;   // how much it squashes
    public float wobbleTime = 0.1f;    // how fast it wobbles
    public float damageCooldown = 0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                BreakEgg(true);
            }
        }
        
    }
    void BreakEgg(Boolean spawn)
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
        // Détruit l'œuf après un délai pour laisser le temps aux particules de jouer
        Destroy(gameObject, brokenEggDuration);
    }
}


