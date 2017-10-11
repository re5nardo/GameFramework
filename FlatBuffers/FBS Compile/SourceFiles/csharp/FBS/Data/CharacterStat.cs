// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.Data
{

using System;
using FlatBuffers;

public sealed class CharacterStat : Struct {
  public CharacterStat __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int HP { get { return bb.GetInt(bb_pos + 0); } }
  public int MP { get { return bb.GetInt(bb_pos + 4); } }
  public float MPChargeRate { get { return bb.GetFloat(bb_pos + 8); } }
  public float RunSpeed { get { return bb.GetFloat(bb_pos + 12); } }
  public float DashSpeed { get { return bb.GetFloat(bb_pos + 16); } }
  public float Strength { get { return bb.GetFloat(bb_pos + 20); } }

  public static Offset<CharacterStat> CreateCharacterStat(FlatBufferBuilder builder, int HP, int MP, float MPChargeRate, float RunSpeed, float DashSpeed, float Strength) {
    builder.Prep(4, 24);
    builder.PutFloat(Strength);
    builder.PutFloat(DashSpeed);
    builder.PutFloat(RunSpeed);
    builder.PutFloat(MPChargeRate);
    builder.PutInt(MP);
    builder.PutInt(HP);
    return new Offset<CharacterStat>(builder.Offset);
  }
};


}
