using DG.Tweening;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Ball
{
    public class BallManager : NetworkBehaviour
    {
        public UnityEvent NextGameStartEvent;
        public UnityEvent<bool> GameEndEvent;
        
        [SerializeField] private TextMeshProUGUI p1ScoreTxt;
        [SerializeField] private TextMeshProUGUI p2ScoreTxt;
        [SerializeField] private Transform ballSpawnTrm1;
        [SerializeField] private Transform ballSpawnTrm2;
        [SerializeField] private Ball ballPrefab;

        [Header("Setting")]
        [SerializeField] private int forGameEndWinCount = 15;
        
        private int _p1Score = 0;
        private int _p2Score = 0;
        private Ball _ballObj;

        public override void OnNetworkSpawn()
        {
            SpawnBall();
        }

        public void SpawnBall()
        {
            if(!IsServer) return;
            _ballObj = Instantiate(ballPrefab);
            _ballObj.GetComponent<NetworkObject>().Spawn();
            _ballObj.EnterGroundEvent += HandleGameEnd;
        }

        private void HandleGameEnd(float x)
        {
            if(!IsServer) return;
            if (x > 0)
                _p1Score++;
            else
                _p2Score++;
            ScoreTxtChangeClientRpc(_p1Score, _p2Score);
            if (_p1Score >= forGameEndWinCount || _p2Score >= forGameEndWinCount)
            {
                GameEndEvent?.Invoke(_p1Score > _p2Score);
                return;
            }
            NextGameStartEvent?.Invoke();
            DOVirtual.DelayedCall(0.5f, () =>
                _ballObj.ResetBall(x > 0 ? ballSpawnTrm1.position : ballSpawnTrm2.position));

        }

        [ClientRpc]
        private void ScoreTxtChangeClientRpc(int score1, int score2)
        {
            p1ScoreTxt.text = score1.ToString();
            p2ScoreTxt.text = score2.ToString();
        }
    }
}