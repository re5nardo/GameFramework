// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_ENTERROOMTOC_FBS_H_
#define FLATBUFFERS_GENERATED_ENTERROOMTOC_FBS_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {

struct PlayerInfo;

struct EnterRoomToC;

MANUALLY_ALIGNED_STRUCT(4) PlayerInfo FLATBUFFERS_FINAL_CLASS {
 private:
  int32_t PlayerIndex_;
  int32_t MasterDataID_;
  int32_t EntityID_;

 public:
  PlayerInfo() { memset(this, 0, sizeof(PlayerInfo)); }
  PlayerInfo(const PlayerInfo &_o) { memcpy(this, &_o, sizeof(PlayerInfo)); }
  PlayerInfo(int32_t _PlayerIndex, int32_t _MasterDataID, int32_t _EntityID)
    : PlayerIndex_(flatbuffers::EndianScalar(_PlayerIndex)), MasterDataID_(flatbuffers::EndianScalar(_MasterDataID)), EntityID_(flatbuffers::EndianScalar(_EntityID)) { }

  int32_t PlayerIndex() const { return flatbuffers::EndianScalar(PlayerIndex_); }
  int32_t MasterDataID() const { return flatbuffers::EndianScalar(MasterDataID_); }
  int32_t EntityID() const { return flatbuffers::EndianScalar(EntityID_); }
};
STRUCT_END(PlayerInfo, 12);

struct EnterRoomToC FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_RESULT = 4,
    VT_USERPLAYERINDEX = 6,
    VT_PLAYERS = 8
  };
  int32_t Result() const { return GetField<int32_t>(VT_RESULT, 0); }
  int32_t UserPlayerIndex() const { return GetField<int32_t>(VT_USERPLAYERINDEX, 0); }
  const flatbuffers::Vector<const PlayerInfo *> *Players() const { return GetPointer<const flatbuffers::Vector<const PlayerInfo *> *>(VT_PLAYERS); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_RESULT) &&
           VerifyField<int32_t>(verifier, VT_USERPLAYERINDEX) &&
           VerifyField<flatbuffers::uoffset_t>(verifier, VT_PLAYERS) &&
           verifier.Verify(Players()) &&
           verifier.EndTable();
  }
};

struct EnterRoomToCBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Result(int32_t Result) { fbb_.AddElement<int32_t>(EnterRoomToC::VT_RESULT, Result, 0); }
  void add_UserPlayerIndex(int32_t UserPlayerIndex) { fbb_.AddElement<int32_t>(EnterRoomToC::VT_USERPLAYERINDEX, UserPlayerIndex, 0); }
  void add_Players(flatbuffers::Offset<flatbuffers::Vector<const PlayerInfo *>> Players) { fbb_.AddOffset(EnterRoomToC::VT_PLAYERS, Players); }
  EnterRoomToCBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  EnterRoomToCBuilder &operator=(const EnterRoomToCBuilder &);
  flatbuffers::Offset<EnterRoomToC> Finish() {
    auto o = flatbuffers::Offset<EnterRoomToC>(fbb_.EndTable(start_, 3));
    return o;
  }
};

inline flatbuffers::Offset<EnterRoomToC> CreateEnterRoomToC(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t Result = 0,
    int32_t UserPlayerIndex = 0,
    flatbuffers::Offset<flatbuffers::Vector<const PlayerInfo *>> Players = 0) {
  EnterRoomToCBuilder builder_(_fbb);
  builder_.add_Players(Players);
  builder_.add_UserPlayerIndex(UserPlayerIndex);
  builder_.add_Result(Result);
  return builder_.Finish();
}

inline flatbuffers::Offset<EnterRoomToC> CreateEnterRoomToCDirect(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t Result = 0,
    int32_t UserPlayerIndex = 0,
    const std::vector<const PlayerInfo *> *Players = nullptr) {
  return CreateEnterRoomToC(_fbb, Result, UserPlayerIndex, Players ? _fbb.CreateVector<const PlayerInfo *>(*Players) : 0);
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_ENTERROOMTOC_FBS_H_
