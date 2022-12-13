using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
     public void Button_Intro()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void Buttion_Main()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Button_Dictionary()
    {
        SceneManager.LoadScene("DictionaryScene");
    }
}
