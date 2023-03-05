using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
 //   public GameObject Angler; 

    private float delayBeforeLoading = 1.7f; 
    [SerializeField]
   // private string sceneToLoad; 

    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime; 
        if(timeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void PlayGame()
    {
       // animator.SetTrigger("Bite");
        
       //yield return new WaitForSeconds(1);
      // StartCoroutine(ButtonDelay());

       // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    // IEnumerator ButtonDelay()
    // {
    //  print(Time.time);
    //  yield return new WaitForSeconds(30);
    //  print(Time.time);


    // }

    public void QuitGame()
    {
        Application.Quit();
    }
}
