using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject MenuUI;
    public GameObject SettingsUI;
    public GameObject GameUI;
    private PlayerRespawn playerRespawn;
    private bool startedGame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRespawn = player.GetComponent<PlayerRespawn>();
        SettingsUI.SetActive(false);
        GameUI.SetActive(false);
        MenuUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        MenuUI.SetActive(false);
        SettingsUI.SetActive(false);
        if(!startedGame) playerRespawn.Respawn();
    }

    public void setSettingsUIScreen()
    {
        GameUI.SetActive(true);
        MenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setMenuUIScreen()
    {
        SettingsUI.SetActive(false);
        GameUI.SetActive(false);
        MenuUI.SetActive(true);
    }
}
