using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Config")]
    public MapConfigSO mapConfig;
    [Header("Prefab")]
    public Room roomPrefab;
    public LineRenderer linePrefab;
    private float screenHeight;
    private float screenWidth;
    private float columnWidth;
    public float border;
    private Vector3 generatedPosition;
    private List<Room> rooms = new();
    private List<LineRenderer> lines = new();
    public List<RoomDataSO> roomDataList = new();
    private Dictionary<RoomType, RoomDataSO> roomDataDictionary = new();
    private void Awake()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        columnWidth = screenWidth / (mapConfig.roomBlueprints.Count + 1);
        foreach (var roomData in roomDataList)
        {
            roomDataDictionary.Add(roomData.roomType, roomData);
        }
    }
    private void Start()
    {
        CreateMap();
    }
    public void CreateMap()
    {
        List<Room> previousColumnRooms = new();

        for(int column = 0; column < mapConfig.roomBlueprints.Count; column++)
        {
            var blueprint = mapConfig.roomBlueprints[column];
            var roomCount = Random.Range(blueprint.min, blueprint.max+1);
            var startHeight = screenHeight/2 - screenHeight / (roomCount+1);
            generatedPosition = new Vector3(-screenWidth/2 + border + columnWidth*column, startHeight, 0); 
            var newPosition = generatedPosition;
            var roomGapY = (screenHeight - border * 2) / (roomCount+1);
            List<Room> currentColumnRooms = new();
            
            for (int i = 0; i < roomCount; i++)
            {
                if (column == mapConfig.roomBlueprints.Count - 1)
                {
                    newPosition.x = screenWidth / 2 - border*2;
                }else if (column != 0)
                {
                    newPosition.x = generatedPosition.x + Random.Range(-border/2, border/2);
                }
                newPosition.y = startHeight- roomGapY * i;
                var room = Instantiate(roomPrefab, newPosition, Quaternion.identity,transform);
                RoomType newType = GetRandomRoomType(mapConfig.roomBlueprints[column].roomType);
                room.SetupRoom(column,i, GetRoomData(newType));
                rooms.Add(room);
                currentColumnRooms.Add(room);
            }
            if (previousColumnRooms.Count > 0)
            {
                CreateConnections(previousColumnRooms, currentColumnRooms);

            }
            previousColumnRooms = currentColumnRooms;

        }
    }
    private void CreateConnections(List<Room> column1, List<Room> column2)
    {
       HashSet<Room> connectedColumn2 = new ();
       foreach (var room in column1)
       {
        var targetRoom = ConnectTORandomRoom(room, column2);
        connectedColumn2.Add(targetRoom);
        }
        foreach (var room in column2)
        {
            if (!connectedColumn2.Contains(room))
            {
                var targetRoom = ConnectTORandomRoom(room, column1);
                connectedColumn2.Add(targetRoom);
            }
        }
    }
    private Room ConnectTORandomRoom(Room room, List<Room> column2)
    {
        Room targetRoom = null;
        targetRoom = column2[Random.Range(0, column2.Count)];
        var line = Instantiate(linePrefab, transform);
        line.SetPosition(0, room.transform.position);
        line.SetPosition(1, targetRoom.transform.position);
        lines.Add(line);
        return targetRoom;
    }
    
    [ContextMenu("StartGame")]
    public void RegenerateRoom()
    {
        foreach (var room in rooms)
        {
            Destroy(room.gameObject);
        }
        foreach (var line in lines)
        {
            Destroy(line.gameObject);
        }
        rooms.Clear();
        lines.Clear();
        CreateMap();
    }
    private RoomDataSO GetRoomData(RoomType roomType)
    {
        return roomDataDictionary[roomType];
    }
    private RoomType GetRandomRoomType(RoomType flags)
    {
        string[] options = flags.ToString().Split(',');
        string randomOption = options[Random.Range(0, options.Length)];
        return (RoomType)System.Enum.Parse(typeof(RoomType), randomOption);
    }
}
