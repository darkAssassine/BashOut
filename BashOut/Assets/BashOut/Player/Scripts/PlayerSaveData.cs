using System;

namespace BashOut.Player
{
    [Serializable]
    public class PlayerSaveData
    {
        public string Name;
        public ControllerType Type;
    }

    public enum ControllerType
    {
        Player,
        AI,
    }
}
