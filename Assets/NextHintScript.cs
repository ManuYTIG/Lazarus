using UnityEngine;

public class NextHintScript : MonoBehaviour
{
    public GameObject hint1;
    public GameObject hint2;
    public GameObject hint3;

    private int hint = 0;

    public void nextHint() {
        if(hint == 0) {
            hint1.SetActive(true);
            hint2.SetActive(false);
            hint3.SetActive(false);
            hint++;
        }

        if(hint == 1) {
            hint1.SetActive(false);
            hint2.SetActive(true);
            hint3.SetActive(false);
            hint++;
        }

        if(hint == 2) {
            hint1.SetActive(false);
            hint2.SetActive(false);
            hint3.SetActive(true);
            hint  = 0;
        }
    }
}
