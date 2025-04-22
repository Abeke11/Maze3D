
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static int SelectedIndex { get;  set; }

    public static void LoadLevel(int index)
    {
        SelectedIndex = index;
        SceneManager.LoadScene("Main");
    }
}
