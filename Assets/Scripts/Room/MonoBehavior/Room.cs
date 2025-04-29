using UnityEngine;

public class Room : MonoBehaviour
{
    public int column;
    public int line;
    private SpriteRenderer spriteRenderer;
    public RoomDataSO roomData;
    public RoomState roomState;

    [Header("Broadcast")]
    public ObjectEventSO loadRoomEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
         spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        SetupRoom(0, 0, roomData);
    }
    private void OnMouseDown()
    {
        Debug.Log("Clicked on room: " + roomData.roomType);
        loadRoomEvent.RaiseEvent(roomData,this);
    }
    public void SetupRoom(int column, int line, RoomDataSO roomData)
    {
        this.column = column;
        this.line = line;
        this.roomData = roomData;
        // Set the sprite for the room
        spriteRenderer.sprite = roomData.roomIcon;
    }
}