// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS
{

using System;
using FlatBuffers;

public sealed class GameEventDashToR : Table {
  public static GameEventDashToR GetRootAsGameEventDashToR(ByteBuffer _bb) { return GetRootAsGameEventDashToR(_bb, new GameEventDashToR()); }
  public static GameEventDashToR GetRootAsGameEventDashToR(ByteBuffer _bb, GameEventDashToR obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GameEventDashToR __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<GameEventDashToR> CreateGameEventDashToR(FlatBufferBuilder builder,
      int PlayerIndex = 0) {
    builder.StartObject(1);
    GameEventDashToR.AddPlayerIndex(builder, PlayerIndex);
    return GameEventDashToR.EndGameEventDashToR(builder);
  }

  public static void StartGameEventDashToR(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddPlayerIndex(FlatBufferBuilder builder, int PlayerIndex) { builder.AddInt(0, PlayerIndex, 0); }
  public static Offset<GameEventDashToR> EndGameEventDashToR(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GameEventDashToR>(o);
  }
};


}