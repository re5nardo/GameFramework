// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.Data
{

using System;
using FlatBuffers;

public sealed class PlayerRankInfo : Struct {
  public PlayerRankInfo __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { return bb.GetInt(bb_pos + 0); } }
  public int Rank { get { return bb.GetInt(bb_pos + 4); } }
  public float Height { get { return bb.GetFloat(bb_pos + 8); } }

  public static Offset<PlayerRankInfo> CreatePlayerRankInfo(FlatBufferBuilder builder, int PlayerIndex, int Rank, float Height) {
    builder.Prep(4, 12);
    builder.PutFloat(Height);
    builder.PutInt(Rank);
    builder.PutInt(PlayerIndex);
    return new Offset<PlayerRankInfo>(builder.Offset);
  }
};


}
