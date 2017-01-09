// automatically generated by the FlatBuffers compiler, do not modify

using System;
using FlatBuffers;

public sealed class CreateRoomToL_Data : Table {
  public static CreateRoomToL_Data GetRootAsCreateRoomToL_Data(ByteBuffer _bb) { return GetRootAsCreateRoomToL_Data(_bb, new CreateRoomToL_Data()); }
  public static CreateRoomToL_Data GetRootAsCreateRoomToL_Data(ByteBuffer _bb, CreateRoomToL_Data obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public CreateRoomToL_Data __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int Result { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public string GetPlayers(int j) { int o = __offset(6); return o != 0 ? __string(__vector(o) + j * 4) : null; }
  public int PlayersLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<CreateRoomToL_Data> CreateCreateRoomToL_Data(FlatBufferBuilder builder,
      int Result = 0,
      VectorOffset PlayersOffset = default(VectorOffset)) {
    builder.StartObject(2);
    CreateRoomToL_Data.AddPlayers(builder, PlayersOffset);
    CreateRoomToL_Data.AddResult(builder, Result);
    return CreateRoomToL_Data.EndCreateRoomToL_Data(builder);
  }

  public static void StartCreateRoomToL_Data(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddResult(FlatBufferBuilder builder, int Result) { builder.AddInt(0, Result, 0); }
  public static void AddPlayers(FlatBufferBuilder builder, VectorOffset PlayersOffset) { builder.AddOffset(1, PlayersOffset.Value, 0); }
  public static VectorOffset CreatePlayersVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartPlayersVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<CreateRoomToL_Data> EndCreateRoomToL_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<CreateRoomToL_Data>(o);
  }
};

