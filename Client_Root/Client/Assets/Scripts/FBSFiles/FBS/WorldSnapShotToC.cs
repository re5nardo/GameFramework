// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS
{

using System;
using FlatBuffers;

public sealed class WorldSnapShotToC : Table {
  public static WorldSnapShotToC GetRootAsWorldSnapShotToC(ByteBuffer _bb) { return GetRootAsWorldSnapShotToC(_bb, new WorldSnapShotToC()); }
  public static WorldSnapShotToC GetRootAsWorldSnapShotToC(ByteBuffer _bb, WorldSnapShotToC obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public WorldSnapShotToC __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int Tick { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public float Time { get { int o = __offset(6); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0.0f; } }
  public EntityState GetEntityStates(int j) { return GetEntityStates(new EntityState(), j); }
  public EntityState GetEntityStates(EntityState obj, int j) { int o = __offset(8); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int EntityStatesLength { get { int o = __offset(8); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<WorldSnapShotToC> CreateWorldSnapShotToC(FlatBufferBuilder builder,
      int Tick = 0,
      float Time = 0.0f,
      VectorOffset EntityStatesOffset = default(VectorOffset)) {
    builder.StartObject(3);
    WorldSnapShotToC.AddEntityStates(builder, EntityStatesOffset);
    WorldSnapShotToC.AddTime(builder, Time);
    WorldSnapShotToC.AddTick(builder, Tick);
    return WorldSnapShotToC.EndWorldSnapShotToC(builder);
  }

  public static void StartWorldSnapShotToC(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddTick(FlatBufferBuilder builder, int Tick) { builder.AddInt(0, Tick, 0); }
  public static void AddTime(FlatBufferBuilder builder, float Time) { builder.AddFloat(1, Time, 0.0f); }
  public static void AddEntityStates(FlatBufferBuilder builder, VectorOffset EntityStatesOffset) { builder.AddOffset(2, EntityStatesOffset.Value, 0); }
  public static VectorOffset CreateEntityStatesVector(FlatBufferBuilder builder, Offset<EntityState>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartEntityStatesVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<WorldSnapShotToC> EndWorldSnapShotToC(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<WorldSnapShotToC>(o);
  }
};


}