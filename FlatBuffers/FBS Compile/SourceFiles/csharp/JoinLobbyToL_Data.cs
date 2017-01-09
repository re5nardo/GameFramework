// automatically generated by the FlatBuffers compiler, do not modify

using System;
using FlatBuffers;

public sealed class JoinLobbyToL_Data : Table {
  public static JoinLobbyToL_Data GetRootAsJoinLobbyToL_Data(ByteBuffer _bb) { return GetRootAsJoinLobbyToL_Data(_bb, new JoinLobbyToL_Data()); }
  public static JoinLobbyToL_Data GetRootAsJoinLobbyToL_Data(ByteBuffer _bb, JoinLobbyToL_Data obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public JoinLobbyToL_Data __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string PlayerKey { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayerKeyBytes() { return __vector_as_arraysegment(4); }
  public int AuthKey { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<JoinLobbyToL_Data> CreateJoinLobbyToL_Data(FlatBufferBuilder builder,
      StringOffset PlayerKeyOffset = default(StringOffset),
      int AuthKey = 0) {
    builder.StartObject(2);
    JoinLobbyToL_Data.AddAuthKey(builder, AuthKey);
    JoinLobbyToL_Data.AddPlayerKey(builder, PlayerKeyOffset);
    return JoinLobbyToL_Data.EndJoinLobbyToL_Data(builder);
  }

  public static void StartJoinLobbyToL_Data(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerKey(FlatBufferBuilder builder, StringOffset PlayerKeyOffset) { builder.AddOffset(0, PlayerKeyOffset.Value, 0); }
  public static void AddAuthKey(FlatBufferBuilder builder, int AuthKey) { builder.AddInt(1, AuthKey, 0); }
  public static Offset<JoinLobbyToL_Data> EndJoinLobbyToL_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<JoinLobbyToL_Data>(o);
  }
};

