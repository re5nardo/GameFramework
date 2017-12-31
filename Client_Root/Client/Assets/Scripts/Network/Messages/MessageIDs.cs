﻿

public class MessageID
{
    //  Client -> Lobby (0 ~ 9999)
    public const ushort JoinLobbyToL_ID =                       0;
    public const ushort SelectNormalGameToL_ID =                1;


    //  Lobby -> Client (10000 ~ 19999)
    public const ushort JoinLobbyToC_ID =                   10000;
    public const ushort SelectNormalGameToC_ID =            10001;


    //  Client -> Room (40000 ~ 49999)
    public const ushort ReadyForStartToR_ID =               40000;
    public const ushort PreparationStateToR_ID =            40001;
    public const ushort GameEventRunToR_ID =                40002;
    public const ushort EnterRoomToR_ID =                   40003;
    public const ushort GameEventStopToR_ID =               40005;
    public const ushort GameInputSkillToR_ID =              40006;
    public const ushort GameInputMoveToR_ID =               40008;
    public const ushort GameInputRotationToR_ID =           40009;
    public const ushort PlayerInputToR_ID =                 40010;


    //  Room -> Client (50000 ~ 59999)
    public const ushort PreparationStateToC_ID =            50000;
    public const ushort GameStartToC_ID =                   50001;
    public const ushort EnterRoomToC_ID =                   50003;
    public const ushort PlayerEnterRoomToC_ID =             50004;
    public const ushort WorldSnapShotToC_ID =               50008;
    public const ushort WorldInfoToC_ID =                   50009;
    public const ushort TickInfoToC_ID =                    50010;
}
