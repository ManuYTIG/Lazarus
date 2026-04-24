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
    public Pipe Pipe6R;
    public Pipe Pipe7R;
    public Pipe Pipe8R;
    public Pipe Pipe9R;

    public int Pipe1Value = 1;
    public int Pipe2Value = 1;
    public int Pipe3Value = 1;
    public int Pipe4Value = 1;
    public int Pipe5Value = 1;
    public int Pipe6Value = 1;
    public int Pipe7Value = 1;
    public int Pipe8Value = 1;
    public int Pipe9Value = 1;

    [SerializeField]
    private Sprite[] pipes;
    
    public Button Pipe1;
    public Button Pipe2;
    public Button Pipe3;
    public Button Pipe4;
    public Button Pipe5;
    public Button Pipe6;
    public Button Pipe7;
    public Button Pipe8;
    public Button Pipe9;
    public Button Checker;
    void Start() 
    {
        PipePuzzleDisplay.SetActive(false);
        Pipe1.onClick.AddListener(Pipe1Click);
        Pipe2.onClick.AddListener(Pipe2Click);
        Pipe3.onClick.AddListener(Pipe3Click);
        Pipe4.onClick.AddListener(Pipe4Click);
        Pipe5.onClick.AddListener(Pipe5Click);
        Pipe6.onClick.AddListener(Pipe6Click);
        Pipe7.onClick.AddListener(Pipe7Click);
        Pipe8.onClick.AddListener(Pipe8Click);
        Pipe9.onClick.AddListener(Pipe9Click);
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
        if (Pipe1Value == 4)
        {
            if (Pipe2Value == 3)
            {
                if (Pipe3Value == 4)
                {
                    if (Pipe4Value == 1)
                    {
                        if (Pipe5Value == 2)
                        {
                            if (Pipe6Value == 4)
                            {
                                if (Pipe7Value == 1)
                                {
                                    //The tagged out code below calls to change the sprite in the Pipe script
                                    //Pipe1R.ChangeButtonImage();
                                    //Pipe2R.ChangeButtonImage();
                                    //Pipe3R.ChangeButtonImage();
                                    //Pipe4R.ChangeButtonImage();
                                    //Pipe5R.ChangeButtonImage();
                                    //Pipe6R.ChangeButtonImage();
                                    //Pipe7R.ChangeButtonImage();
                                    Debug.Log("Correct"); //This is where script that calls for something like opening a door goes
                                }
                            }
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
    void Pipe6Click() 
    {
        Pipe6R.RotateObject();
        if (Pipe6Value < 4)
        {
            Pipe6Value += 1;
        }
        else
        {
            Pipe6Value = 1;
        }
    }
    void Pipe7Click() 
    {
        Pipe7R.RotateObject();
        if (Pipe7Value < 4)
        {
            Pipe7Value += 1;
        }
        else
        {
            Pipe7Value = 1;
        }
    }
    void Pipe8Click() 
    {
        Pipe8R.RotateObject();
        if (Pipe8Value < 4)
        {
            Pipe8Value += 1;
        }
        else
        {
            Pipe8Value = 1;
        }
    }
    void Pipe9Click() 
    {
        Pipe9R.RotateObject();
        if (Pipe9Value < 4)
        {
            Pipe9Value += 1;
        }
        else
        {
            Pipe9Value = 1;
        }
    }
}