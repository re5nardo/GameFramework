// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_JOINLOBBYTOC_FBS_H_
#define FLATBUFFERS_GENERATED_JOINLOBBYTOC_FBS_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {

struct JoinLobbyToC;

struct JoinLobbyToC FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_RESULT = 4
  };
  int32_t Result() const { return GetField<int32_t>(VT_RESULT, 0); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_RESULT) &&
           verifier.EndTable();
  }
};

struct JoinLobbyToCBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_Result(int32_t Result) { fbb_.AddElement<int32_t>(JoinLobbyToC::VT_RESULT, Result, 0); }
  JoinLobbyToCBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  JoinLobbyToCBuilder &operator=(const JoinLobbyToCBuilder &);
  flatbuffers::Offset<JoinLobbyToC> Finish() {
    auto o = flatbuffers::Offset<JoinLobbyToC>(fbb_.EndTable(start_, 1));
    return o;
  }
};

inline flatbuffers::Offset<JoinLobbyToC> CreateJoinLobbyToC(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t Result = 0) {
  JoinLobbyToCBuilder builder_(_fbb);
  builder_.add_Result(Result);
  return builder_.Finish();
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_JOINLOBBYTOC_FBS_H_
