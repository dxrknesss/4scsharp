using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practice6
{
    internal class GameInfo
    {
        static GameInfo _instance;
        static GameState _gameState;
        public static Dictionary<GameState, string> AnnounceTexts = new Dictionary<GameState, string>()
        {
                        {GameState.WAIT_FOR_PLAYER, String.Join('\n', "wait for other player".ToCharArray())},
                        {GameState.ATTACK, String.Join('\n', "Attack enemy field".ToCharArray())},
                        {GameState.WAIT, String.Join('\n', "Wait for enemy".ToCharArray())},
                        {GameState.PLACE_SHIPS, String.Join('\n', "Place your ships".ToCharArray())}
        };

        public static byte PlayerCount;
        public static GameType CurrentGameType;
        public static Action<GameState> GameStateChange;
        public static GameState CurrentGameState 
        {
            get => _gameState;
            set
            {
                _gameState = value;
                GameStateChange?.Invoke(value);
            }
        } 

        public static GameInfo GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameInfo();
            }
            return _instance;
        }

        public enum GameState
        {
            WAIT_FOR_PLAYER, PLACE_SHIPS, ATTACK, WAIT
        }

        public enum GameType
        {
            SINGLE, MULTI
        }

        private GameInfo() { }
    }
}
