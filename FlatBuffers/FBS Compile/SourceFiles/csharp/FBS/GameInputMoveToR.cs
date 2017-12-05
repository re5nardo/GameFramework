// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS
{

using System;
using FlatBuffers;

public sealed class GameInputMoveToR : Table {
  public static GameInputMoveToR GetRootAsGameInputMoveToR(ByteBuffer _bb) { return GetRootAsGameInputMoveToR(_bb, new GameInputMoveToR()); }
  public static GameInputMoveToR GetRootAsGameInputMoveToR(ByteBuffer _bb, GameInputMoveToR obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GameInputMoveToR __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public MoveDirection Direction { get { int o = __offset(6); return o != 0 ? (MoveDirection)bb.GetSbyte(o + bb_pos) : MoveDirection.Up; } }

  public static Offset<GameInputMoveToR> CreateGameInputMoveToR(FlatBufferBuilder builder,
      int PlayerIndex = 0,
      MoveDirection Direction = MoveDirection.Up) {
    builder.StartObject(2);
    GameInputMoveToR.AddPlayerIndex(builder, PlayerIndex);
    GameInputMoveToR.AddDirection(builder, Direction);
    return GameInputMoveToR.EndGameInputMoveToR(builder);
  }

  public static void StartGameInputMoveToR(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerIndex(FlatBufferBuilder builder, int PlayerIndex) { builder.AddInt(0, PlayerIndex, 0); }
  public static void AddDirection(FlatBufferBuilder builder, MoveDirection Direction) { builder.AddSbyte(1, (sbyte)Direction, 0); }
  public static Offset<GameInputMoveToR> EndGameInputMoveToR(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GameInputMoveToR>(o);
  }
};


}
