// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_GAMESTARTTOC_FBS_H_
#define FLATBUFFERS_GENERATED_GAMESTARTTOC_FBS_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {

struct GameStartToC;

struct GameStartToC FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_TICKINTERVAL = 4,
    VT_RANDOMSEED = 6
  };
  float TickInterval() const { return GetField<float>(VT_TICKINTERVAL, 0.0f); }
  int32_t RandomSeed() const { return GetField<int32_t>(VT_RANDOMSEED, 0); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<float>(verifier, VT_TICKINTERVAL) &&
           VerifyField<int32_t>(verifier, VT_RANDOMSEED) &&
           verifier.EndTable();
  }
};

struct GameStartToCBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_TickInterval(float TickInterval) { fbb_.AddElement<float>(GameStartToC::VT_TICKINTERVAL, TickInterval, 0.0f); }
  void add_RandomSeed(int32_t RandomSeed) { fbb_.AddElement<int32_t>(GameStartToC::VT_RANDOMSEED, RandomSeed, 0); }
  GameStartToCBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  GameStartToCBuilder &operator=(const GameStartToCBuilder &);
  flatbuffers::Offset<GameStartToC> Finish() {
    auto o = flatbuffers::Offset<GameStartToC>(fbb_.EndTable(start_, 2));
    return o;
  }
};

inline flatbuffers::Offset<GameStartToC> CreateGameStartToC(flatbuffers::FlatBufferBuilder &_fbb,
    float TickInterval = 0.0f,
    int32_t RandomSeed = 0) {
  GameStartToCBuilder builder_(_fbb);
  builder_.add_RandomSeed(RandomSeed);
  builder_.add_TickInterval(TickInterval);
  return builder_.Finish();
}

}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_GAMESTARTTOC_FBS_H_
