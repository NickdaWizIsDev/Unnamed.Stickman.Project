using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelList : MonoBehaviour
{
    public Button level1;
    public Button level2;
    public Button level3;
    public Button tutorial;
    public Button back;
    public GameObject mainMenu;
    public GameObject levelList;

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }
    public void Level3()
    {
        SceneManager.LoadScene(3);
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void Back()
    {
        levelList.SetActive(false);
        mainMenu.SetActive(true);
    }
}
