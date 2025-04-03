using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(fileName = "RoomDataSO", menuName = "Map/RoomDataSO")]
public class RoomDataSO : ScriptableObject
{
    public Sprite roomIcon;
    public RoomType roomType;
    public AssetReference sceneToLoad;
    public Vector3 roomPosition;
    public Vector3 roomRotation;
    public Vector3 roomScale;
    public bool isVisited = false;

    // Add any other properties you need for the room data
}