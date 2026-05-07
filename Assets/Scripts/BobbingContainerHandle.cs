using UnityEngine;
using System.Collections;

public class BobbingContainerHandle : MonoBehaviour
{
    public static BobbingContainerHandle instance;
    private Transform transform;
    private float bobbingPeriod = 4f;
    private float bobbingHeight = 2f;
    private bool setToRun = false;
    private bool isBobbing = false;
    private bool returnedToPosition = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (setToRun && !isBobbing)
        {
            if (transform != null)
            {
                Debug.Log("start bobbing");
                StartBobbing();
                setToRun = false;
            }
            
        }
        else if (!isBobbing && !returnedToPosition)
        {
            StopAllCoroutines();
            StopBobbing();
        }
    }

    public void StartBobbing()
    {
        if(transform != null)
        {
            isBobbing = true;
            StartCoroutine(Bobbing());
        }
        else setToRun = true;

    }
    public void StartBobbing(float height)
    {
        bobbingHeight = height;
        if (transform != null)
        {
            isBobbing = true;
            StartCoroutine(Bobbing());
        }
        else setToRun = true;
    }
    public void StartBobbing(float height, float period)
    {
        bobbingHeight = height;
        bobbingPeriod = period;
        if (transform != null)
        {
            isBobbing = true;
            StartCoroutine(Bobbing());
        }
        else setToRun = true;
    }
    private IEnumerator Bobbing()
    {
        if(transform != null)
        {
            float elapsedTime = 0f;
            while (isBobbing)
            {
                float newY = Mathf.Sin((elapsedTime / bobbingPeriod) * 2 * Mathf.PI) * bobbingHeight;
                transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        } else
        {
            Debug.LogError("BobbingContainerHandle: RectTransform not found.");
        }
        
    }

    public void StopBobbing()
    {
        isBobbing = false;
        StopAllCoroutines();
        StartCoroutine(ReturnToPosition());
    }

    public bool isBobbingActive()
    {
        return isBobbing;
    }

    public bool isInPosition()
    {
        return returnedToPosition;
    }

    private System.Collections.IEnumerator ReturnToPosition()
    {
        if(transform != null)
        {
            transform = instance.GetComponent<RectTransform>();
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = Vector3.zero;
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = endPos;
        returnedToPosition = true;
        } else
        {
            Debug.LogError("BobbingContainerHandle: RectTransform not found.");
        }
    }
}
