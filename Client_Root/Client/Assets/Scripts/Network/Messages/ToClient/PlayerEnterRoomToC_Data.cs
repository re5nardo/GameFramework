// automatically generated by the FlatBuffers compiler, do not modify

using System;
using FlatBuffers;

public sealed class PlayerEnterRoomToC_Data : Table {
  public static PlayerEnterRoomToC_Data GetRootAsPlayerEnterRoomToC_Data(ByteBuffer _bb) { return GetRootAsPlayerEnterRoomToC_Data(_bb, new PlayerEnterRoomToC_Data()); }
  public static PlayerEnterRoomToC_Data GetRootAsPlayerEnterRoomToC_Data(ByteBuffer _bb, PlayerEnterRoomToC_Data obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public PlayerEnterRoomToC_Data __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string CharacterID { get { int o = __offset(6); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetCharacterIDBytes() { return __vector_as_arraysegment(6); }

  public static Offset<PlayerEnterRoomToC_Data> CreatePlayerEnterRoomToC_Data(FlatBufferBuilder builder,
      int PlayerIndex = 0,
      StringOffset CharacterIDOffset = default(StringOffset)) {
    builder.StartObject(2);
    PlayerEnterRoomToC_Data.AddCharacterID(builder, CharacterIDOffset);
    PlayerEnterRoomToC_Data.AddPlayerIndex(builder, PlayerIndex);
    return PlayerEnterRoomToC_Data.EndPlayerEnterRoomToC_Data(builder);
  }

  public static void StartPlayerEnterRoomToC_Data(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerIndex(FlatBufferBuilder builder, int PlayerIndex) { builder.AddInt(0, PlayerIndex, 0); }
  public static void AddCharacterID(FlatBufferBuilder builder, StringOffset CharacterIDOffset) { builder.AddOffset(1, CharacterIDOffset.Value, 0); }
  public static Offset<PlayerEnterRoomToC_Data> EndPlayerEnterRoomToC_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<PlayerEnterRoomToC_Data>(o);
  }
};

