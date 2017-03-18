// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_PREPARATIONSTATETOCDATA_H_
#define FLATBUFFERS_GENERATED_PREPARATIONSTATETOCDATA_H_

#include "flatbuffers/flatbuffers.h"

struct PreparationStateToC_Data;

struct PreparationStateToC_Data FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_PLAYERINDEX = 4,
    VT_STATE = 6
  };
  int32_t PlayerIndex() const { return GetField<int32_t>(VT_PLAYERINDEX, 0); }
  float State() const { return GetField<float>(VT_STATE, 0.0f); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_PLAYERINDEX) &&
           VerifyField<float>(verifier, VT_STATE) &&
           verifier.EndTable();
  }
};

struct PreparationStateToC_DataBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_PlayerIndex(int32_t PlayerIndex) { fbb_.AddElement<int32_t>(PreparationStateToC_Data::VT_PLAYERINDEX, PlayerIndex, 0); }
  void add_State(float State) { fbb_.AddElement<float>(PreparationStateToC_Data::VT_STATE, State, 0.0f); }
  PreparationStateToC_DataBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  PreparationStateToC_DataBuilder &operator=(const PreparationStateToC_DataBuilder &);
  flatbuffers::Offset<PreparationStateToC_Data> Finish() {
    auto o = flatbuffers::Offset<PreparationStateToC_Data>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<PreparationStateToC_Data> CreatePreparationStateToC_Data(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t PlayerIndex = 0,
    float State = 0.0f) {
  PreparationStateToC_DataBuilder builder_(_fbb);
  builder_.add_State(State);
  builder_.add_PlayerIndex(PlayerIndex);
  return builder_.Finish();
}

#endif  // FLATBUFFERS_GENERATED_PREPARATIONSTATETOCDATA_H_