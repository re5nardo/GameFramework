// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_PLAYERENTERROOMTOC_FBS_H_
#define FLATBUFFERS_GENERATED_PLAYERENTERROOMTOC_FBS_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {

struct PlayerEnterRoomToC;

struct PlayerEnterRoomToC FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
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

struct PlayerEnterRoomToCBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_PlayerIndex(int32_t PlayerIndex) { fbb_.AddElement<int32_t>(PlayerEnterRoomToC::VT_PLAYERINDEX, PlayerIndex, 0); }
  void add_CharacterID(flatbuffers::Offset<flatbuffers::String> CharacterID) { fbb_.AddOffset(PlayerEnterRoomToC::VT_CHARACTERID, CharacterID); }
  PlayerEnterRoomToCBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  PlayerEnterRoomToCBuilder &operator=(const PlayerEnterRoomToCBuilder &);
  flatbuffers::Offset<PlayerEnterRoomToC> Finish() {
    auto o = flatbuffers::Offset<PlayerEnterRoomToC>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<PlayerEnterRoomToC> CreatePlayerEnterRoomToC(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t PlayerIndex = 0,
    flatbuffers::Offset<flatbuffers::String> CharacterID = 0) {
  PlayerEnterRoomToCBuilder builder_(_fbb);
  builder_.add_CharacterID(CharacterID);
  builder_.add_PlayerIndex(PlayerIndex);
  return builder_.Finish();
}

inline flatbuffers::Offset<PlayerEnterRoomToC> CreatePlayerEnterRoomToCDirect(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t PlayerIndex = 0,
    const char *CharacterID = nullptr) {
  return CreatePlayerEnterRoomToC(_fbb, PlayerIndex, CharacterID ? _fbb.CreateString(CharacterID) : 0);
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_PLAYERENTERROOMTOC_FBS_H_
