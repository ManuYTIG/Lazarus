using UnityEngine;

public class ExitHintScript : MonoBehaviour
{
    public GameObject Hint;


    public void removeHint() {
        Hint.SetActive(false);
    }
}
