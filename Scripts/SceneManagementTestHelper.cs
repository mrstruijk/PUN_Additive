using mrstruijk.SceneManagement;


public class SceneManagementTestHelper : SceneManagement
{
	public void LoadSceneHelper()
	{
		StartCoroutine(base.LoadScenes(base.areaSceneCollection.SceneNames));
	}
}
