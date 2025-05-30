using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace BashOut.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Player playerPrefab;
        [SerializeField] private Player AiPrefab;
        [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
        [SerializeField] private int playerCount;
        [SerializeField] private int botCount;
        [SerializeField] private int Lifes;
        [SerializeField] private GameObject respawnVFX;
        private Dictionary<string, int> playerHealth = new Dictionary<string, int>();

        [SerializeField] private UnityEvent<string> GameOver;

        [SerializeField] private static List<Player> players = new List<Player>();
        private bool gameOver = false;

        void Awake()
        {
            players.Clear();
            players = new List<Player>();
            int i = 0;
            for (; i < playerCount; i++)
            {
                Player newPlayer = Instantiate(playerPrefab, spawnPoints[i].transform.position, Quaternion.identity);
                newPlayer.PlayerDied.AddListener(OnPlayerDied);
                newPlayer.Name = "Player " + (i + 1).ToString();
                playerHealth.Add(newPlayer.Name, Lifes);
                newPlayer.Initalize(Lifes);
                players.Add(newPlayer);
            }
            for (int j = 0; j < botCount; j++)
            {
                Player newBot = Instantiate(AiPrefab, spawnPoints[j + i].transform.position, Quaternion.identity);
                newBot.PlayerDied.AddListener(OnPlayerDied);
                newBot.Name = "Bot " + (j + 1).ToString();
                playerHealth.Add(newBot.Name, Lifes);
                newBot.Initalize(Lifes);
                players.Add(newBot);
            }
        }

        private void OnPlayerDied(Player _deadPlayer)
        {
            playerHealth[_deadPlayer.Name] -= 1;
            PlayerSaveData saveData = _deadPlayer.GetPlayerSaveData();
            players.Remove(_deadPlayer);

            if (playerHealth[_deadPlayer.Name] > 0)
            {
                StartCoroutine(Respawn(saveData));
            }
            else
            {
                CheckForLastPlayerAlive();
            }
        }

        private void CheckForLastPlayerAlive()
        {
            int aliveCount = 0;
            string alivePlayer = "";

            foreach (KeyValuePair<string, int> health in playerHealth)
            {
                if (health.Value > 0)
                {
                    aliveCount++;
                    alivePlayer = health.Key;
                }
            }
            if (aliveCount == 1)
            {
                if (gameOver == false)
                {
                    GameOver?.Invoke(alivePlayer);
                }
                gameOver = true;
            }
            else if (aliveCount < 1)
            {
                if (gameOver == false)
                {
                    GameOver?.Invoke("Nobody");
                }
                gameOver = true;
            }
        }

        private IEnumerator Respawn(PlayerSaveData saveData)
        {

            yield return new WaitForSeconds(1);

            Vector2 position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);
            Destroy(Instantiate(respawnVFX, position, rotation), 4);

            yield return new WaitForSeconds(2);

            if (saveData.Type == ControllerType.Player)
            {
                Player newPlayer = Instantiate(playerPrefab, position, Quaternion.identity);
                newPlayer.PlayerDied.AddListener(OnPlayerDied);
                newPlayer.Name = saveData.Name;
                newPlayer.Initalize(playerHealth[newPlayer.Name]);
                players.Add(newPlayer);
            }
            else
            {
                Player newPlayer = Instantiate(AiPrefab, position, Quaternion.identity);
                newPlayer.PlayerDied.AddListener(OnPlayerDied);
                newPlayer.Name = saveData.Name;
                newPlayer.Initalize(playerHealth[newPlayer.Name]);
                players.Add(newPlayer);
            }

        }

        public static Player GetNearestEnemy(Vector3 _pos)
        {
            float distance = float.MaxValue;

            Player nearestEnemy = null;

            if (players.Count < 1)
            {
                return null;
            }

            foreach (var player in players)
            {
                if (_pos != player.transform.position)
                {
                    float currentDistance = Vector2.Distance(player.transform.position, _pos);
                    if (currentDistance < distance)
                    {
                        nearestEnemy = player;
                        distance = currentDistance;
                    }
                }
            }
            return nearestEnemy;
        }
    }
}
