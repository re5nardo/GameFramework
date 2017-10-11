// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS
{

using System;
using FlatBuffers;

public sealed class PlayerEnterRoomToC : Table {
  public static PlayerEnterRoomToC GetRootAsPlayerEnterRoomToC(ByteBuffer _bb) { return GetRootAsPlayerEnterRoomToC(_bb, new PlayerEnterRoomToC()); }
  public static PlayerEnterRoomToC GetRootAsPlayerEnterRoomToC(ByteBuffer _bb, PlayerEnterRoomToC obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public PlayerEnterRoomToC __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public int PlayerEntityID { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string CharacterID { get { int o = __offset(8); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetCharacterIDBytes() { return __vector_as_arraysegment(8); }

  public static Offset<PlayerEnterRoomToC> CreatePlayerEnterRoomToC(FlatBufferBuilder builder,
      int PlayerIndex = 0,
      int PlayerEntityID = 0,
      StringOffset CharacterIDOffset = default(StringOffset)) {
    builder.StartObject(3);
    PlayerEnterRoomToC.AddCharacterID(builder, CharacterIDOffset);
    PlayerEnterRoomToC.AddPlayerEntityID(builder, PlayerEntityID);
    PlayerEnterRoomToC.AddPlayerIndex(builder, PlayerIndex);
    return PlayerEnterRoomToC.EndPlayerEnterRoomToC(builder);
  }

  public static void StartPlayerEnterRoomToC(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddPlayerIndex(FlatBufferBuilder builder, int PlayerIndex) { builder.AddInt(0, PlayerIndex, 0); }
  public static void AddPlayerEntityID(FlatBufferBuilder builder, int PlayerEntityID) { builder.AddInt(1, PlayerEntityID, 0); }
  public static void AddCharacterID(FlatBufferBuilder builder, StringOffset CharacterIDOffset) { builder.AddOffset(2, CharacterIDOffset.Value, 0); }
  public static Offset<PlayerEnterRoomToC> EndPlayerEnterRoomToC(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<PlayerEnterRoomToC>(o);
  }
};


}
