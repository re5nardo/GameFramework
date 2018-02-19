using UnityEngine;
using FlatBuffers;

namespace PlayerInput
{
    public class GameItem : IPlayerInput
    {
        public int m_nPlayerIndex;
        public int m_nGameItemID;

        public override FBS.PlayerInputType GetPlayerInputType()
        {
            return FBS.PlayerInputType.GameItem;
        }

        public override byte[] Serialize()
        {
            FBS.PlayerInput.GameItem.StartGameItem(m_Builder);
            FBS.PlayerInput.GameItem.AddPlayerIndex(m_Builder, m_nPlayerIndex);
            FBS.PlayerInput.GameItem.AddGameItemID(m_Builder, m_nGameItemID);
            var data = FBS.PlayerInput.GameItem.EndGameItem(m_Builder);

            m_Builder.Finish(data.Value);

            return m_Builder.SizedByteArray();
        }

        public override bool Deserialize(byte[] bytes)
        {
            var buf = new ByteBuffer(bytes);

            var data = FBS.PlayerInput.GameItem.GetRootAsGameItem(buf);

            m_nPlayerIndex = data.PlayerIndex;
            m_nGameItemID = data.GameItemID;

            return true;
        }

        public override string ToString()
        {
            return string.Format("PlayerIndex : {0}, GameItemID : {1}", m_nPlayerIndex, m_nGameItemID);
        }
    }
}