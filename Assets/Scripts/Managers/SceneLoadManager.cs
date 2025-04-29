using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
public class SceneLoadManager : MonoBehaviour
{
    private AssetReference currentScene;
    public AssetReference map;
    public async void OnLoadRoomEvent(object data)
    {
        if (data is RoomDataSO)
        {
            var currentRoom = (RoomDataSO)data;
            currentScene = currentRoom.sceneToLoad;
        }
        await UnloadSceneTask();
        await LoadSceneTask();
    }
     private async Awaitable LoadSceneTask()
    {
        var s = currentScene.LoadSceneAsync(LoadSceneMode.Additive);
        await s.Task;
        if (s.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(s.Result.Scene);
        }
    }
     private async Awaitable UnloadSceneTask()
    {
        await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

    }
    public async void LoadMap()
    {
        await UnloadSceneTask();
        currentScene = map;
        await LoadSceneTask();
    }
}