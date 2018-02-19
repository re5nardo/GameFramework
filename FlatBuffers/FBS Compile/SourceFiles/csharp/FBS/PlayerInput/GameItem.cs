// automatically generated by the FlatBuffers compiler, do not modify

namespace FBS.PlayerInput
{

using System;
using FlatBuffers;

public sealed class GameItem : Table {
  public static GameItem GetRootAsGameItem(ByteBuffer _bb) { return GetRootAsGameItem(_bb, new GameItem()); }
  public static GameItem GetRootAsGameItem(ByteBuffer _bb, GameItem obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public GameItem __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int PlayerIndex { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public int GameItemID { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<GameItem> CreateGameItem(FlatBufferBuilder builder,
      int PlayerIndex = 0,
      int GameItemID = 0) {
    builder.StartObject(2);
    GameItem.AddGameItemID(builder, GameItemID);
    GameItem.AddPlayerIndex(builder, PlayerIndex);
    return GameItem.EndGameItem(builder);
  }

  public static void StartGameItem(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddPlayerIndex(FlatBufferBuilder builder, int PlayerIndex) { builder.AddInt(0, PlayerIndex, 0); }
  public static void AddGameItemID(FlatBufferBuilder builder, int GameItemID) { builder.AddInt(1, GameItemID, 0); }
  public static Offset<GameItem> EndGameItem(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<GameItem>(o);
  }
};


}
