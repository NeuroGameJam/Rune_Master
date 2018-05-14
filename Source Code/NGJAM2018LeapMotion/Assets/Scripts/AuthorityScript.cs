using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class AuthorityScript : NetworkBehaviour
    {
        [SyncVar]
        private float _totalScore;
        public float _scoreThisRound;
        private bool _betOnSelf;
        private bool _fail;
        private bool _chose;

        public bool HasAuthority()
        {
            return hasAuthority;
        }

        public void Choice(bool _bet)
        {
            _betOnSelf = _bet;
            StartCoroutine(IChoice());

        }

        private IEnumerator IChoice()
        {
            yield return new WaitForEndOfFrame();
            if (!_chose)
            {
                if (isServer)
                {
                    GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMChoice();
                }
                else
                {
                    CmdChoice();
                }
                _chose = true;
            }
        }

        [Command]
        public void CmdChoice()
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMChoice();
        }
    
        [Command]
        public void CmdSomething()
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMSuccess();
        }

        public void UpdateScore(float scoreUpdate)
        {
            if (_betOnSelf)
            {
                _totalScore += _scoreThisRound;
            }
            else
            {
                var players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var player in players)
                {
                    if (player == gameObject)
                    {
                        continue;
                    }
                    _totalScore = player.GetComponent<AuthorityScript>().ReturnScore();
                }
            }
            GameObject.Find("Text").GetComponent<Text>().text = "Score: " + _totalScore;
        }

        public float ReturnScore()
        {
            return _scoreThisRound;
        }

        public void SetFail(bool fail)
        {
            _fail = fail;
        }

        public void SetFailureState()
        {
            if (isServer)
            {
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMEnableUI(_fail);
            }
            else
            {
                CmdSendFailure(_fail);
            }
        }

        [Command]
        private void CmdSendFailure(bool fail)
        {
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMEnableUI(fail);
        }

        public void DisableUI()
        {
            if (isServer)
            {
                _chose = false;
                RpcChoseFalse();
                GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMDisableUI();
            }
            else
            {
                CmdDisableUI();
            }
        }

        [Command]
        private void CmdDisableUI()
        {
            _chose = false;
            RpcChoseFalse();
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().GMDisableUI();
        }

        [ClientRpc]
        private void RpcChoseFalse()
        {
            _chose = false;
            GameObject.Find("GameManager").GetComponent<GameManagerScript>().SetChoicesNull();
        }

        public void NetFunction()
        {
            if (isServer)
            {
                _chose = false;
                RpcFunction();
            }
            else
            {
                CmdFunction();
            }
        }

        [ClientRpc]
        private void RpcFunction()
        {
            _chose = false;
        }
        [Command]
        private void CmdFunction()
        {
            _chose = false;
            RpcFunction();
        }
    }
}
