using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TuneVisualizer;

namespace UTJ.MLAPISample
{
    // クライアント接続した際に、MLAPIからのコールバックを管理して切断時等の処理をします
    public class ClientManager : MonoBehaviour
    {
        public Button stopButton;
        public GameObject configureObject;
        private bool previewConnected;
        private SyncTransform syncTransform;
        private GameObject head;
        private SyncTransform headST;
        private float headOffset = -0.347f;
        private GameObject left;
        private SyncTransform leftST;
        private GameObject right;
        private SyncTransform rightST;

        private MLAPI.Transports.Tasks.SocketTasks socketTasks;
        public void SetSocketTasks(MLAPI.Transports.Tasks.SocketTasks tasks)
        {
            socketTasks = tasks;
        }

        public void Setup()
        {
            MLAPI.NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnect;
            MLAPI.NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;
        }

        private void ReoveCallbacks()
        {
            MLAPI.NetworkingManager.Singleton.OnClientConnectedCallback -= OnClientConnect;
            MLAPI.NetworkingManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnect;
        }

        private void Disconnect()
        {
#if ENABLE_AUTO_CLIENT
            // クライアント接続時に切断したらアプリ終了させます
            if (NetworkUtility.IsBatchModeRun)
            {
                Application.Quit();
            }
#endif
            // UIを戻します
            configureObject.SetActive(true);
            stopButton.gameObject.SetActive(false);
            stopButton.onClick.RemoveAllListeners();
            // コールバックも削除します
            ReoveCallbacks();
        }

        private void OnClickStopButton()
        {
            MLAPI.NetworkingManager.Singleton.StopClient();
            Disconnect();
        }

        private void OnClientConnect(ulong clientId)
        {
            // 自身の接続の場合
            if (clientId == MLAPI.NetworkingManager.Singleton.LocalClientId)
            {
                configureObject.SetActive(false);

                stopButton.GetComponentInChildren<Text>().text = "Disconnect";
                stopButton.onClick.AddListener(this.OnClickStopButton);
                stopButton.gameObject.SetActive(true);

                //頭の設定
                /**head = GameObject.FindGameObjectWithTag("Head");
                headST = head.GetComponent<SyncTransform>();
                headST.parentTag = "MainCamera";
                headST.positionOffset = new Vector3(0, 0, -headOffset);
                headST.SyncTs();
                head.SetActive(false);

                //右手の設定
                right = GameObject.FindGameObjectWithTag("Right");
                rightST = right.GetComponent<SyncTransform>();
                rightST.parentTag = "RightController";
                rightST.SyncTs();
                right.SetActive(false);

                //左手の設定
                left = GameObject.FindGameObjectWithTag("Left");
                leftST = left.GetComponent<SyncTransform>();
                leftST.parentTag = "LeftController";
                leftST.SyncTs();
                left.SetActive(false);**/
            }

            Debug.Log("Connect Client:" + clientId + "::" + MLAPI.NetworkingManager.Singleton.LocalClientId);
        }
        private void OnClientDisconnect(ulong clientId)
        {
            // 自身が外された
            Debug.Log("Disconnect Client: " + clientId);
        }

        private void Update()
        {
            var netMgr = MLAPI.NetworkingManager.Singleton;
            // 3人以上接続時に切断が呼び出されないので対策
            if (!netMgr.IsConnectedClient && previewConnected)
            {
                Disconnect();
            }
            previewConnected = netMgr.IsConnectedClient;
        }
    }
}