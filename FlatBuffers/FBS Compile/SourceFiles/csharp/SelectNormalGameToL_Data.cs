// automatically generated by the FlatBuffers compiler, do not modify

using System;
using FlatBuffers;

public sealed class SelectNormalGameToL_Data : Table {
  public static SelectNormalGameToL_Data GetRootAsSelectNormalGameToL_Data(ByteBuffer _bb) { return GetRootAsSelectNormalGameToL_Data(_bb, new SelectNormalGameToL_Data()); }
  public static SelectNormalGameToL_Data GetRootAsSelectNormalGameToL_Data(ByteBuffer _bb, SelectNormalGameToL_Data obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public SelectNormalGameToL_Data __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }


  public static void StartSelectNormalGameToL_Data(FlatBufferBuilder builder) { builder.StartObject(0); }
  public static Offset<SelectNormalGameToL_Data> EndSelectNormalGameToL_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<SelectNormalGameToL_Data>(o);
  }
};
