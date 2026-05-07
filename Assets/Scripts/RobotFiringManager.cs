using UnityEngine;

public class FiringManager : MonoBehaviour
{
    public FloatingRobotEnemy mainScript;
    public void FireAtTarget()
    {
        mainScript.FireAtTarget();
    }
}
