// automatically generated by the FlatBuffers compiler, do not modify

using System;
using FlatBuffers;

public sealed class JoinLobbyToC_Data : Table {
  public static JoinLobbyToC_Data GetRootAsJoinLobbyToC_Data(ByteBuffer _bb) { return GetRootAsJoinLobbyToC_Data(_bb, new JoinLobbyToC_Data()); }
  public static JoinLobbyToC_Data GetRootAsJoinLobbyToC_Data(ByteBuffer _bb, JoinLobbyToC_Data obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public JoinLobbyToC_Data __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int Result { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<JoinLobbyToC_Data> CreateJoinLobbyToC_Data(FlatBufferBuilder builder,
      int Result = 0) {
    builder.StartObject(1);
    JoinLobbyToC_Data.AddResult(builder, Result);
    return JoinLobbyToC_Data.EndJoinLobbyToC_Data(builder);
  }

  public static void StartJoinLobbyToC_Data(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddResult(FlatBufferBuilder builder, int Result) { builder.AddInt(0, Result, 0); }
  public static Offset<JoinLobbyToC_Data> EndJoinLobbyToC_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<JoinLobbyToC_Data>(o);
  }
};

