using UnityEngine;
using System.Collections.Generic;

public class FloatingRobotEnemy : MonoBehaviour
{
    public float startX = 0f;
    public float startY = 0f;
    public float endX = 10f;
    public float endY = 0f;
    public float timeBetween = 5f;
    public float detectionRange = 5f;
    public string[] targetTags = {"Player", "DroppedItem"};
    public float bobbingAmplitude = 0.2f;
    public BobbingContainerHandle bobbingHandler;
    public Animator animator;
    public GameObject beam;
    public Transform orbTransform;
    public AudioSource bodySource;
    public AudioClip fireSound;
    public AudioClip chargeSound;

    private bool movingToEnd = true;
    private float timerMovement = 0f;
    private Vector3 initialPosition;
    private GameObject currentBeam;
    private Vector3 shootingTarget;
    private float fireCooldown = 2f;
    private float fireTime = 0f;
    private bool firing = false;
    private Vector2 direction;
    private GameObject prevObj;
    private List<GameObject> objectsTouching = new List<GameObject>();
    private Transform target = null;
    private bool checkedCollisions = false;
    private bool hasTarget = false;
    private float baseScaleX;
    private float offsetY = -0.05f;
    private float offsetX = 0.15f;

    void Start()
    {
        bobbingHandler.StartBobbing(bobbingAmplitude);
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        direction = (new Vector2(endX, endY)).normalized;
        objectsTouching = new List<GameObject>();
        baseScaleX = transform.localScale.x; // store original scale
    }

    void Update()
    {
        if(firing)
        {
            //keep pointing at the target
            UpdateBeamTransform(shootingTarget);
            fireTime += Time.deltaTime;
            if(fireTime >= fireCooldown)
            {
                StopFiring();
                fireTime = 0f;
            }
        } else
        {
            if (checkedCollisions)
            {
                hasTarget = false;
                if (objectsTouching.Count == 0)
                {
                    if (prevObj != null)
                    {
                        animator.Play("FloatingRobotIdle");
                        bodySource.Stop();
                        prevObj = null;
                        target = null;
                    }
                }
                else
                {
                    if (objectsTouching.Contains(prevObj))
                    {
                        //continue as normal if we are still touching the same object
                        hasTarget = true;
                    }
                    for (int tag = 0; tag < targetTags.Length && !hasTarget; tag++)
                    {
                        //find another object if possible, and choose it by priority
                        for (int i = 0; i < objectsTouching.Count; i++)
                        {
                            if (objectsTouching[i].CompareTag(targetTags[tag]))
                            {
                                //this is the new object to target
                                Debug.Log("Found new target: " + objectsTouching[i].name);
                                target = objectsTouching[i].transform;
                                prevObj = objectsTouching[i];
                                hasTarget = true;
                                animator.Play("ChargeFloatingRobot");
                                if (bodySource != null && chargeSound != null)
                                {
                                    bodySource.Stop();
                                    bodySource.PlayOneShot(chargeSound);
                                }
                                break;
                            }
                        }
                    }
                }

                objectsTouching = new List<GameObject>();
                checkedCollisions = false;
            }
        }

        
        if(hasTarget)
        {
            TargetPosition();
        } else
        {
            timerMovement += Time.deltaTime;
            if(timerMovement >= timeBetween)
            {
                Debug.Log("Swtiching direction");
                movingToEnd = !movingToEnd;
                timerMovement = 0f;
            }
            direction = (movingToEnd ? (new Vector3(initialPosition.x + endX, initialPosition.y + endY, transform.position.z) - transform.position).normalized : (new Vector3(initialPosition.x + startX, initialPosition.y + startY, transform.position.z) - transform.position).normalized);

            transform.position += (Vector3)direction * (new Vector2(initialPosition.x + endX, initialPosition.y + endY) - new Vector2(initialPosition.x + startX, initialPosition.y + startY)).magnitude/timeBetween * Time.deltaTime;

            HandleSpriteFlip(direction.x);
        }
    }

    public void FireAtTarget()
    {
        if(target == null) return;
        Debug.Log("Firing at target");
        TargetPosition();
        shootingTarget = target.position;
        currentBeam = Instantiate(beam, transform.position, Quaternion.identity);
        currentBeam.transform.parent = transform;
        UpdateBeamTransform(shootingTarget);
        if (target.CompareTag("Player"))
        {
            //fire at player
            target.GetComponent<Health>().RemoveHealth(target.GetComponent<Health>().health);
        }
        bodySource.Stop();
        bodySource.PlayOneShot(fireSound);
        firing = true;
    }

    public void StopFiring()
    {
        Debug.Log("Stopping firing at target");
        animator.Play("FloatingRobotIdle");
        if (currentBeam != null)
        {
            Destroy(currentBeam);
            hasTarget = false;
        }
        firing = false;
    }

    private void UpdateBeamTransform(Vector3 targetPosition)
    {
        if (currentBeam == null || orbTransform == null) return;

        Vector3 originLocal = transform.InverseTransformPoint(orbTransform.position);
        originLocal.x += (transform.localScale.x > 0 ? -1 : 1) * offsetX;
        originLocal.y += offsetY;

        Vector3 targetLocal = transform.InverseTransformPoint(targetPosition);
        Vector3 directionLocal = targetLocal - originLocal;
        if (directionLocal.sqrMagnitude < 0.0001f) return;

        currentBeam.transform.localPosition = originLocal + directionLocal * 0.5f;
        currentBeam.transform.localRotation = Quaternion.FromToRotation(Vector3.right, directionLocal);
        currentBeam.transform.localScale = new Vector3(directionLocal.magnitude, currentBeam.transform.localScale.y, 1f);
    }

    public void TargetPosition()
    {
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        HandleSpriteFlip(direction.x);
    }

    private void HandleSpriteFlip(float horizontalDirection)
    {
        if (Mathf.Abs(horizontalDirection) < 0.01f)
            return; // avoid jitter when moving mostly vertically

        float newScaleX = baseScaleX * (horizontalDirection > 0 ? 1 : -1);
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        checkedCollisions = true;
        //simply update the array (the update method will handle the rest)
        for (int i = targetTags.Length - 1; i >= 0; i--)
        {
            if (collision.gameObject.CompareTag(targetTags[i]))
            {
                Debug.Log("Colliding with " + collision.gameObject.name);
                if (!objectsTouching.Contains(collision.gameObject)) objectsTouching.Add(collision.gameObject);
                break;
            }
        }
    }
}