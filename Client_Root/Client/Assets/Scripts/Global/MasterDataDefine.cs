
namespace MasterDataDefine
{
    public class GameItem
    {
        public const int FirstID = 0;
        public const int LastID = 6;
    }

    public class BehaviorID
    {
        public const int IDLE = 0;
        public const int MOVE = 1;
        public const int DIE = 2;
        public const int ROTATION = 3;
        public const int JUMP = 4;
    }

    public class StateID
    {
        public const int BOOST = 0;
        public const int SHIELD = 1;
        public const int RESPAWN_INVINCIBLE = 2;
        public const int CHALLENGER_DISTURBING = 3;
        public const int FAINT = 4;
    }
}