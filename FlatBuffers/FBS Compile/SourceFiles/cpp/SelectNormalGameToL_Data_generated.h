// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_SELECTNORMALGAMETOLDATA_H_
#define FLATBUFFERS_GENERATED_SELECTNORMALGAMETOLDATA_H_

#include "flatbuffers/flatbuffers.h"

struct SelectNormalGameToL_Data;

struct SelectNormalGameToL_Data FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           verifier.EndTable();
  }
};

struct SelectNormalGameToL_DataBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  SelectNormalGameToL_DataBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  SelectNormalGameToL_DataBuilder &operator=(const SelectNormalGameToL_DataBuilder &);
  flatbuffers::Offset<SelectNormalGameToL_Data> Finish() {
    auto o = flatbuffers::Offset<SelectNormalGameToL_Data>(fbb_.EndTable(start_, 0));
    return o;
  }
};

inline flatbuffers::Offset<SelectNormalGameToL_Data> CreateSelectNormalGameToL_Data(flatbuffers::FlatBufferBuilder &_fbb) {
  SelectNormalGameToL_DataBuilder builder_(_fbb);
  return builder_.Finish();
}

#endif  // FLATBUFFERS_GENERATED_SELECTNORMALGAMETOLDATA_H_