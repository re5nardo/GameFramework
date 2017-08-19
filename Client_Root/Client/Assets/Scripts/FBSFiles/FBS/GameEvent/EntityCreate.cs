// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.GameEvent
{

using System;
using FlatBuffers;

public sealed class EntityCreate : Table {
  public static EntityCreate GetRootAsEntityCreate(ByteBuffer _bb) { return GetRootAsEntityCreate(_bb, new EntityCreate()); }
  public static EntityCreate GetRootAsEntityCreate(ByteBuffer _bb, EntityCreate obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public EntityCreate __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int EntityID { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public int MasterDataID { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public FBS.Data.EntityType Type { get { int o = __offset(8); return o != 0 ? (FBS.Data.EntityType)bb.GetSbyte(o + bb_pos) : FBS.Data.EntityType.Character; } }
  public FBS.Data.Vector3 Pos { get { return GetPos(new FBS.Data.Vector3()); } }
  public FBS.Data.Vector3 GetPos(FBS.Data.Vector3 obj) { int o = __offset(10); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }

  public static void StartEntityCreate(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddEntityID(FlatBufferBuilder builder, int EntityID) { builder.AddInt(0, EntityID, 0); }
  public static void AddMasterDataID(FlatBufferBuilder builder, int MasterDataID) { builder.AddInt(1, MasterDataID, 0); }
  public static void AddType(FlatBufferBuilder builder, FBS.Data.EntityType Type) { builder.AddSbyte(2, (sbyte)Type, 0); }
  public static void AddPos(FlatBufferBuilder builder, Offset<FBS.Data.Vector3> PosOffset) { builder.AddStruct(3, PosOffset.Value, 0); }
  public static Offset<EntityCreate> EndEntityCreate(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<EntityCreate>(o);
  }
};


}