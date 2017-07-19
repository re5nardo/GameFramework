// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.GameEvent
{

using System;
using FlatBuffers;

public sealed class Position : Table {
  public static Position GetRootAsPosition(ByteBuffer _bb) { return GetRootAsPosition(_bb, new Position()); }
  public static Position GetRootAsPosition(ByteBuffer _bb, Position obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Position __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int EntityID { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public float StartTime { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0.0f; } }
  public float EndTime { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0.0f; } }
  public FBS.Data.Vector3 StartPos { get { return GetStartPos(new FBS.Data.Vector3()); } }
  public FBS.Data.Vector3 GetStartPos(FBS.Data.Vector3 obj) { int o = __offset(10); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public FBS.Data.Vector3 EndPos { get { return GetEndPos(new FBS.Data.Vector3()); } }
  public FBS.Data.Vector3 GetEndPos(FBS.Data.Vector3 obj) { int o = __offset(12); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }

  public static void StartPosition(FlatBufferBuilder builder) { builder.StartObject(5); }
  public static void AddEntityID(FlatBufferBuilder builder, int EntityID) { builder.AddInt(0, EntityID, 0); }
  public static void AddStartTime(FlatBufferBuilder builder, float StartTime) { builder.AddFloat(1, StartTime, 0.0f); }
  public static void AddEndTime(FlatBufferBuilder builder, float EndTime) { builder.AddFloat(2, EndTime, 0.0f); }
  public static void AddStartPos(FlatBufferBuilder builder, Offset<FBS.Data.Vector3> StartPosOffset) { builder.AddStruct(3, StartPosOffset.Value, 0); }
  public static void AddEndPos(FlatBufferBuilder builder, Offset<FBS.Data.Vector3> EndPosOffset) { builder.AddStruct(4, EndPosOffset.Value, 0); }
  public static Offset<Position> EndPosition(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Position>(o);
  }
};


}
