// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_CREATEROOMTORDATA_H_
#define FLATBUFFERS_GENERATED_CREATEROOMTORDATA_H_

#include "flatbuffers/flatbuffers.h"

struct CreateRoomToR_Data;

struct CreateRoomToR_Data FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_MATCHID = 4,
    VT_PLAYERS = 6
  };
  int32_t MatchID() const { return GetField<int32_t>(VT_MATCHID, 0); }
  const flatbuffers::Vector<flatbuffers::Offset<flatbuffers::String>> *Players() const { return GetPointer<const flatbuffers::Vector<flatbuffers::Offset<flatbuffers::String>> *>(VT_PLAYERS); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_MATCHID) &&
           VerifyField<flatbuffers::uoffset_t>(verifier, VT_PLAYERS) &&
           verifier.Verify(Players()) &&
           verifier.VerifyVectorOfStrings(Players()) &&
           verifier.EndTable();
  }
};

struct CreateRoomToR_DataBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_MatchID(int32_t MatchID) { fbb_.AddElement<int32_t>(CreateRoomToR_Data::VT_MATCHID, MatchID, 0); }
  void add_Players(flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<flatbuffers::String>>> Players) { fbb_.AddOffset(CreateRoomToR_Data::VT_PLAYERS, Players); }
  CreateRoomToR_DataBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  CreateRoomToR_DataBuilder &operator=(const CreateRoomToR_DataBuilder &);
  flatbuffers::Offset<CreateRoomToR_Data> Finish() {
    auto o = flatbuffers::Offset<CreateRoomToR_Data>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<CreateRoomToR_Data> CreateCreateRoomToR_Data(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t MatchID = 0,
    flatbuffers::Offset<flatbuffers::Vector<flatbuffers::Offset<flatbuffers::String>>> Players = 0) {
  CreateRoomToR_DataBuilder builder_(_fbb);
  builder_.add_Players(Players);
  builder_.add_MatchID(MatchID);
  return builder_.Finish();
}

inline flatbuffers::Offset<CreateRoomToR_Data> CreateCreateRoomToR_DataDirect(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t MatchID = 0,
    const std::vector<flatbuffers::Offset<flatbuffers::String>> *Players = nullptr) {
  return CreateCreateRoomToR_Data(_fbb, MatchID, Players ? _fbb.CreateVector<flatbuffers::Offset<flatbuffers::String>>(*Players) : 0);
}

#endif  // FLATBUFFERS_GENERATED_CREATEROOMTORDATA_H_
