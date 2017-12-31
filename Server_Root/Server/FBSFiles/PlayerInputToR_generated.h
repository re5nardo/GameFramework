// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_PLAYERINPUTTOR_FBS_H_
#define FLATBUFFERS_GENERATED_PLAYERINPUTTOR_FBS_H_

#include "flatbuffers/flatbuffers.h"

#include "PlayerInputData_generated.h"

namespace FBS {

struct PlayerInputToR;

struct PlayerInputToR FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_TYPE = 4,
    VT_DATA = 6
  };
  FBS::PlayerInputType Type() const { return static_cast<FBS::PlayerInputType>(GetField<int8_t>(VT_TYPE, 0)); }
  const flatbuffers::Vector<int8_t> *Data() const { return GetPointer<const flatbuffers::Vector<int8_t> *>(VT_DATA); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int8_t>(verifier, VT_TYPE) &&
           VerifyField<flatbuffers::uoffset_t>(verifier, VT_DATA) &&
           verifier.Verify(Data()) &&
           verifier.EndTable();
  }
};

struct PlayerInputToRBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Type(FBS::PlayerInputType Type) { fbb_.AddElement<int8_t>(PlayerInputToR::VT_TYPE, static_cast<int8_t>(Type), 0); }
  void add_Data(flatbuffers::Offset<flatbuffers::Vector<int8_t>> Data) { fbb_.AddOffset(PlayerInputToR::VT_DATA, Data); }
  PlayerInputToRBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  PlayerInputToRBuilder &operator=(const PlayerInputToRBuilder &);
  flatbuffers::Offset<PlayerInputToR> Finish() {
    auto o = flatbuffers::Offset<PlayerInputToR>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<PlayerInputToR> CreatePlayerInputToR(flatbuffers::FlatBufferBuilder &_fbb,
    FBS::PlayerInputType Type = FBS::PlayerInputType_Position,
    flatbuffers::Offset<flatbuffers::Vector<int8_t>> Data = 0) {
  PlayerInputToRBuilder builder_(_fbb);
  builder_.add_Data(Data);
  builder_.add_Type(Type);
  return builder_.Finish();
}

inline flatbuffers::Offset<PlayerInputToR> CreatePlayerInputToRDirect(flatbuffers::FlatBufferBuilder &_fbb,
    FBS::PlayerInputType Type = FBS::PlayerInputType_Position,
    const std::vector<int8_t> *Data = nullptr) {
  return CreatePlayerInputToR(_fbb, Type, Data ? _fbb.CreateVector<int8_t>(*Data) : 0);
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_PLAYERINPUTTOR_FBS_H_
