using UnityEngine;

public class UIControllerManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    CharacterStatsUI characterStats;
    bool openPanel = false;
    bool openMenu = false;
    bool openOptions = false;

    public bool OpenPanel => openPanel;
    private void Start()
    {
        characterStats = panel.GetComponent<CharacterStatsUI>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowPanel();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMenu();
        }
    }
    void ShowPanel()
    {
        if (openPanel)
        {
            panel.gameObject.SetActive(false);
            openPanel = false;
        }
        else if (!openPanel && !openMenu && !openOptions)
        {
            panel.gameObject.SetActive(true);
            openPanel = true;
            characterStats.RefreshTexts();
        }
    }
    void ShowMenu()
    {
        if (openMenu)
        {
            menu.gameObject.SetActive(false);
            openMenu = false;
        }
        else
        {
            menu.gameObject.SetActive(true);
            openMenu = true;
            panel.gameObject.SetActive(false);
            openPanel = false;
        }
    }
    public void ShowOptions()
    {
        options.gameObject.SetActive(true);
        openOptions = true;
        menu.gameObject.SetActive(false);
        openMenu = false;
    }
    public void BackToMenu()
    {
        options.gameObject.SetActive(false);
        openOptions = false;
        menu.gameObject.SetActive(true);
        openMenu = true;
    }
    public void BackToGame()
    {
        menu.gameObject.SetActive(false);
        openMenu = false;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
