// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS
{

using System;
using FlatBuffers;

public sealed class JoinLobbyToL : Table {
  public static JoinLobbyToL GetRootAsJoinLobbyToL(ByteBuffer _bb) { return GetRootAsJoinLobbyToL(_bb, new JoinLobbyToL()); }
  public static JoinLobbyToL GetRootAsJoinLobbyToL(ByteBuffer _bb, JoinLobbyToL obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public JoinLobbyToL __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string PlayerKey { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetPlayerKeyBytes() { return __vector_as_arraysegment(4); }
  public int AuthKey { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<JoinLobbyToL> CreateJoinLobbyToL(FlatBufferBuilder builder,
      StringOffset PlayerKeyOffset = default(StringOffset),
      int AuthKey = 0) {
    builder.StartObject(2);
    JoinLobbyToL.AddAuthKey(builder, AuthKey);
    JoinLobbyToL.AddPlayerKey(builder, PlayerKeyOffset);
    return JoinLobbyToL.EndJoinLobbyToL(builder);
  }

  public static void StartJoinLobbyToL(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerKey(FlatBufferBuilder builder, StringOffset PlayerKeyOffset) { builder.AddOffset(0, PlayerKeyOffset.Value, 0); }
  public static void AddAuthKey(FlatBufferBuilder builder, int AuthKey) { builder.AddInt(1, AuthKey, 0); }
  public static Offset<JoinLobbyToL> EndJoinLobbyToL(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<JoinLobbyToL>(o);
  }
};


}