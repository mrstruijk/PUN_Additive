using mrstruijk.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingScreen : MonoBehaviour
{
   public SceneCollection loadingScreen;



   private void Awake()
   {
      SceneManager.LoadSceneAsync(loadingScreen.scenes[0], LoadSceneMode.Additive);
   }


   public void UnloadLoadingScreen()
   {
      SceneManager.UnloadSceneAsync(loadingScreen.scenes[0]);
   }
}
