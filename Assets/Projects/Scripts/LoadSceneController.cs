using UnityEngine.SceneManagement;

public static class LoadSceneController 
{
    public static void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level "+level);
    }
}