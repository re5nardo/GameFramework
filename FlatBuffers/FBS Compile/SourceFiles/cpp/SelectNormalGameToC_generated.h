// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_SELECTNORMALGAMETOC_FBS_H_
#define FLATBUFFERS_GENERATED_SELECTNORMALGAMETOC_FBS_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {

struct SelectNormalGameToC;

struct SelectNormalGameToC FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_RESULT = 4,
    VT_EXPECTEDTIME = 6
  };
  int32_t Result() const { return GetField<int32_t>(VT_RESULT, 0); }
  int32_t ExpectedTime() const { return GetField<int32_t>(VT_EXPECTEDTIME, 0); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_RESULT) &&
           VerifyField<int32_t>(verifier, VT_EXPECTEDTIME) &&
           verifier.EndTable();
  }
};

struct SelectNormalGameToCBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Result(int32_t Result) { fbb_.AddElement<int32_t>(SelectNormalGameToC::VT_RESULT, Result, 0); }
  void add_ExpectedTime(int32_t ExpectedTime) { fbb_.AddElement<int32_t>(SelectNormalGameToC::VT_EXPECTEDTIME, ExpectedTime, 0); }
  SelectNormalGameToCBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  SelectNormalGameToCBuilder &operator=(const SelectNormalGameToCBuilder &);
  flatbuffers::Offset<SelectNormalGameToC> Finish() {
    auto o = flatbuffers::Offset<SelectNormalGameToC>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<SelectNormalGameToC> CreateSelectNormalGameToC(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t Result = 0,
    int32_t ExpectedTime = 0) {
  SelectNormalGameToCBuilder builder_(_fbb);
  builder_.add_ExpectedTime(ExpectedTime);
  builder_.add_Result(Result);
  return builder_.Finish();
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_SELECTNORMALGAMETOC_FBS_H_
