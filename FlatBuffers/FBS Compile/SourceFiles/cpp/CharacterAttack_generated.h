// automatically generated by the FlatBuffers compiler, do not modify

#ifndef FLATBUFFERS_GENERATED_CHARACTERATTACK_FBS_GAMEEVENT_H_
#define FLATBUFFERS_GENERATED_CHARACTERATTACK_FBS_GAMEEVENT_H_

#include "flatbuffers/flatbuffers.h"

namespace FBS {
namespace GameEvent {

struct CharacterAttack;

struct CharacterAttack FLATBUFFERS_FINAL_CLASS : private flatbuffers::Table {
  enum {
    VT_ATTACKINGENTITYID = 4,
    VT_ATTACKEDENTITYID = 6,
    VT_DAMAGE = 8
  };
  int32_t AttackingEntityID() const { return GetField<int32_t>(VT_ATTACKINGENTITYID, 0); }
  int32_t AttackedEntityID() const { return GetField<int32_t>(VT_ATTACKEDENTITYID, 0); }
  int32_t Damage() const { return GetField<int32_t>(VT_DAMAGE, 0); }
  bool Verify(flatbuffers::Verifier &verifier) const {
    return VerifyTableStart(verifier) &&
           VerifyField<int32_t>(verifier, VT_ATTACKINGENTITYID) &&
           VerifyField<int32_t>(verifier, VT_ATTACKEDENTITYID) &&
           VerifyField<int32_t>(verifier, VT_DAMAGE) &&
           verifier.EndTable();
  }
};

struct CharacterAttackBuilder {
  flatbuffers::FlatBufferBuilder &fbb_;
  flatbuffers::uoffset_t start_;
  void add_AttackingEntityID(int32_t AttackingEntityID) { fbb_.AddElement<int32_t>(CharacterAttack::VT_ATTACKINGENTITYID, AttackingEntityID, 0); }
  void add_AttackedEntityID(int32_t AttackedEntityID) { fbb_.AddElement<int32_t>(CharacterAttack::VT_ATTACKEDENTITYID, AttackedEntityID, 0); }
  void add_Damage(int32_t Damage) { fbb_.AddElement<int32_t>(CharacterAttack::VT_DAMAGE, Damage, 0); }
  CharacterAttackBuilder(flatbuffers::FlatBufferBuilder &_fbb) : fbb_(_fbb) { start_ = fbb_.StartTable(); }
  CharacterAttackBuilder &operator=(const CharacterAttackBuilder &);
  flatbuffers::Offset<CharacterAttack> Finish() {
    auto o = flatbuffers::Offset<CharacterAttack>(fbb_.EndTable(start_, 3));
    return o;
  }
};

inline flatbuffers::Offset<CharacterAttack> CreateCharacterAttack(flatbuffers::FlatBufferBuilder &_fbb,
    int32_t AttackingEntityID = 0,
    int32_t AttackedEntityID = 0,
    int32_t Damage = 0) {
  CharacterAttackBuilder builder_(_fbb);
  builder_.add_Damage(Damage);
  builder_.add_AttackedEntityID(AttackedEntityID);
  builder_.add_AttackingEntityID(AttackingEntityID);
  return builder_.Finish();
}

}  // namespace GameEvent
}  // namespace FBS

#endif  // FLATBUFFERS_GENERATED_CHARACTERATTACK_FBS_GAMEEVENT_H_
