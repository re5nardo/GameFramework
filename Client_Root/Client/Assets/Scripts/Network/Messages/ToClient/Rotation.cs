// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.GameEvent
{

using System;
using FlatBuffers;

public sealed class Rotation : Table {
  public static Rotation GetRootAsRotation(ByteBuffer _bb) { return GetRootAsRotation(_bb, new Rotation()); }
  public static Rotation GetRootAsRotation(ByteBuffer _bb, Rotation obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public Rotation __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int EntityID { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public float StartTime { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0.0f; } }
  public float EndTime { get { int o = __offset(8); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0.0f; } }
  public FBSData.Vector3 StartRot { get { return GetStartRot(new FBSData.Vector3()); } }
  public FBSData.Vector3 GetStartRot(FBSData.Vector3 obj) { int o = __offset(10); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public FBSData.Vector3 EndRot { get { return GetEndRot(new FBSData.Vector3()); } }
  public FBSData.Vector3 GetEndRot(FBSData.Vector3 obj) { int o = __offset(12); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }

  public static void StartRotation(FlatBufferBuilder builder) { builder.StartObject(5); }
  public static void AddEntityID(FlatBufferBuilder builder, int EntityID) { builder.AddInt(0, EntityID, 0); }
  public static void AddStartTime(FlatBufferBuilder builder, float StartTime) { builder.AddFloat(1, StartTime, 0.0f); }
  public static void AddEndTime(FlatBufferBuilder builder, float EndTime) { builder.AddFloat(2, EndTime, 0.0f); }
  public static void AddStartRot(FlatBufferBuilder builder, Offset<FBSData.Vector3> StartRotOffset) { builder.AddStruct(3, StartRotOffset.Value, 0); }
  public static void AddEndRot(FlatBufferBuilder builder, Offset<FBSData.Vector3> EndRotOffset) { builder.AddStruct(4, EndRotOffset.Value, 0); }
  public static Offset<Rotation> EndRotation(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Rotation>(o);
  }
};


}
