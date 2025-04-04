using System;

[Flags]
public enum RoomType
{
    Family = 1,
    Study = 2,
    Friend = 4,
    Boss = 8,
    Event = 16,
    Birth = 32
}
public enum RoomState
{
    Locked,
    Visited,
    Attainable
}
