using UnityEngine;
using UnityEngine.UI;


public class PipeDisplay : MonoBehaviour
{
    public GameObject PipePuzzleDisplay;
    private bool isVisible = false;

    public Pipe Pipe1R;
    public Pipe Pipe2R;
    public Pipe Pipe3R;
    public Pipe Pipe4R;
    public Pipe Pipe5R;

    public int Pipe1Value = 1;
    public int Pipe2Value = 1;
    public int Pipe3Value = 1;
    public int Pipe4Value = 1;
    public int Pipe5Value = 1;

    [SerializeField]
    private Sprite[] pipes;
    
    public Button Pipe1;
    public Button Pipe2;
    public Button Pipe3;
    public Button Pipe4;
    public Button Pipe5;
    public Button Checker;
    void Start() 
    {
        PipePuzzleDisplay.SetActive(false);
        Pipe1.onClick.AddListener(Pipe1Click);
        Pipe2.onClick.AddListener(Pipe2Click);
        Pipe3.onClick.AddListener(Pipe3Click);
        Pipe4.onClick.AddListener(Pipe4Click);
        Pipe5.onClick.AddListener(Pipe5Click);
        Checker.onClick.AddListener(CheckerClick);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pressed");
            isVisible = !isVisible;
            PipePuzzleDisplay.SetActive(isVisible);
        }
    }
    void CheckerClick()
    {
        if (Pipe1Value == 1)
        {
            if (Pipe2Value == 2)
            {
                if (Pipe3Value == 3)
                {
                    if (Pipe4Value == 4)
                    {
                        if (Pipe5Value == 1)
                        {
                            Debug.Log("FinalCorrect"); //This is where script that calls for something like opening a door goes
                        }
                    }
                }
            }
        }
    }
    void Pipe1Click() 
    {
        Pipe1R.RotateObject();
        if (Pipe1Value < 4)
        {
            Pipe1Value += 1;
        }
        else
        {
            Pipe1Value = 1;
        }
    }
    void Pipe2Click() 
    {
        Pipe2R.RotateObject();
        if (Pipe2Value < 4)
        {
            Pipe2Value += 1;
        }
        else
        {
            Pipe2Value = 1;
        }
    }
    void Pipe3Click() 
    {
        Pipe3R.RotateObject();
        if (Pipe3Value < 4)
        {
            Pipe3Value += 1;
        }
        else
        {
            Pipe3Value = 1;
        }
    }
    void Pipe4Click() 
    {
        Pipe4R.RotateObject();
        if (Pipe4Value < 4)
        {
            Pipe4Value += 1;
        }
        else
        {
            Pipe4Value = 1;
        }
    }
    void Pipe5Click() 
    {
        Pipe5R.RotateObject();
        if (Pipe5Value < 4)
        {
            Pipe5Value += 1;
        }
        else
        {
            Pipe5Value = 1;
        }
    }
}