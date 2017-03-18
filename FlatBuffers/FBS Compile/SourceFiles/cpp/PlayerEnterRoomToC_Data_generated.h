// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_PLAYERENTERROOMTOCDATA_H_
#define FLATBUFFERS_GENERATED_PLAYERENTERROOMTOCDATA_H_

#include "flatbuffers/flatbuffers.h"

struct PlayerEnterRoomToC_Data;

struct PlayerEnterRoomToC_Data FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_PLAYERINDEX = 4,
    VT_CHARACTERID = 6
  };
  int32_t PlayerIndex() const { return GetField<int32_t>(VT_PLAYERINDEX, 0); }
  const flatbuffers::String *CharacterID() const { return GetPointer<const flatbuffers::String *>(VT_CHARACTERID); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_PLAYERINDEX) &&
           VerifyField<flatbuffers::uoffset_t>(verifier, VT_CHARACTERID) &&
           verifier.Verify(CharacterID()) &&
           verifier.EndTable();
  }
};

struct PlayerEnterRoomToC_DataBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_PlayerIndex(int32_t PlayerIndex) { fbb_.AddElement<int32_t>(PlayerEnterRoomToC_Data::VT_PLAYERINDEX, PlayerIndex, 0); }
  void add_CharacterID(flatbuffers::Offset<flatbuffers::String> CharacterID) { fbb_.AddOffset(PlayerEnterRoomToC_Data::VT_CHARACTERID, CharacterID); }
  PlayerEnterRoomToC_DataBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  PlayerEnterRoomToC_DataBuilder &operator=(const PlayerEnterRoomToC_DataBuilder &);
  flatbuffers::Offset<PlayerEnterRoomToC_Data> Finish() {
    auto o = flatbuffers::Offset<PlayerEnterRoomToC_Data>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<PlayerEnterRoomToC_Data> CreatePlayerEnterRoomToC_Data(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t PlayerIndex = 0,
    flatbuffers::Offset<flatbuffers::String> CharacterID = 0) {
  PlayerEnterRoomToC_DataBuilder builder_(_fbb);
  builder_.add_CharacterID(CharacterID);
  builder_.add_PlayerIndex(PlayerIndex);
  return builder_.Finish();
}

inline flatbuffers::Offset<PlayerEnterRoomToC_Data> CreatePlayerEnterRoomToC_DataDirect(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t PlayerIndex = 0,
    const char *CharacterID = nullptr) {
  return CreatePlayerEnterRoomToC_Data(_fbb, PlayerIndex, CharacterID ? _fbb.CreateString(CharacterID) : 0);
}

#endif  // FLATBUFFERS_GENERATED_PLAYERENTERROOMTOCDATA_H_