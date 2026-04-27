using UnityEngine;

public class BloodPoolManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void StartBloodPool()
    {
        if (TryGetComponent<Animator>(out Animator animator))
        {
            animator.Play("ExpandingBloodPool");
        }
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator != null)
        {
            animator.Play("BloodPoolEnd");
        }
    }
}
