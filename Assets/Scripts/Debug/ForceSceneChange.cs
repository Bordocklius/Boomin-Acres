using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceSceneChange : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadScene1();
        }
    }

    public void LoadScene1()
    {
        for(int i = 0; i < 4; i++)
        {
            var a = GlobdataContainer.GetPlayerIDInput(i);
            if(a != null)
            {
                a.SwitchCurrentActionMap("Player");
            }
        }
        SceneManager.LoadScene(1);
    }
}
