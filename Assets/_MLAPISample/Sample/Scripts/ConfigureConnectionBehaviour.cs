﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UTJ.MLAPISample
{
    // 接続設定や接続をするUIのコンポーネント
    public class ConfigureConnectionBehaviour : MonoBehaviour
    {
        /**
        // IPアドレス表示用
        public Text localIpInfoText;
        // Relayサーバー使用するかチェックボックス
        public Toggle useRelayToggle;
        // 接続先IP入力ボックス
        public InputField ipInputField;
        // 接続ポート入力フィールド
        public InputField portInputField;
        // Relay IP入力ボックス
        public InputField relayIpInputField;
        // Relay ポート入力フィールド
        public InputField relayPortInputField;
        // プレイヤー名入力フィールド
        public InputField playerNameInputFiled;
        // リセットボタン
        public Button resetButton;
        // ホストとして接続ボタン
        public Button hostButton;
        // クライアントボタン
        public Button clientButton;**/

        // ホスト（サーバー）立上げ後に諸々管理するマネージャー
        public ServerManager serverManager;
        // Client接続後に諸々管理するマネージャー
        public ClientManager clientManager;

        // 接続設定
        private ConnectInfo connectInfo;

        [SerializeField] bool IsClientAccount = false;


        // 一旦Player名の保存箇所です
        public static string playerName;

        // ローカルのIPアドレス
        private string localIPAddr;

        // 接続時
        void Awake()
        {
            // ターゲットフレームレートをちゃんと設定する
            // ※60FPSにしないと Headlessサーバーや、バッチクライアントでブン回りしちゃうんで…
            Application.targetFrameRate = 60;


            localIPAddr = NetworkUtility.GetLocalIP();
            //this.localIpInfoText.text = "あなたのIPアドレスは、" + localIPAddr;

            this.connectInfo = ConnectInfo.LoadFromFile();
            //ApplyConnectInfoToUI();

            //this.resetButton.onClick.AddListener(OnClickReset);
            //this.hostButton.onClick.AddListener(OnClickHost);
            //this.clientButton.onClick.AddListener(OnClickClient);
        }

        // 
        private void Start()
        {
            // サーバービルド時
#if UNITY_SERVER
            Debug.Log("Server Build.");
            ApplyConnectInfoToNetworkManager();
            this.serverManager.Setup(this.connectInfo, localIPAddr);
            // あと余計なものをHeadless消します
            NetworkUtility.RemoveUpdateSystemForHeadlessServer();

            // MLAPIでサーバーとして起動
            var tasks = MLAPI.NetworkingManager.Singleton.StartServer();
            this.serverManager.SetSocketTasks(tasks);
#elif ENABLE_AUTO_CLIENT
            if (NetworkUtility.IsBatchModeRun)
            {
                // バッチモードでは余計なシステム消します
                NetworkUtility.RemoveUpdateSystemForBatchBuild();
                OnClickClient();
            }
#endif
            if (IsClientAccount)
            {
                ConnectClient();
            }
        }

        // Hostとして起動ボタンを押したとき
        /**private void OnClickHost()
        {
            GenerateConnectInfoValueFromUI();
            ApplyConnectInfoToNetworkManager();
            this.connectInfo.SaveToFile();

            // 既にクライアントとして起動していたら、クライアントを止めます
            if( MLAPI.NetworkingManager.Singleton.IsClient){
                MLAPI.NetworkingManager.Singleton.StopClient();
            }
            // ServerManagerでMLAPIのコールバック回りを設定
            this.serverManager.Setup(this.connectInfo, localIPAddr);
            // MLAPIでホストとして起動
            var tasks = MLAPI.NetworkingManager.Singleton.StartHost();
            this.serverManager.SetSocketTasks(tasks);
        }**/

        // Clientとして起動ボタンを押したとき
        private void ConnectClient()
        {
            GenerateConnectInfoValueFromUI();
            ApplyConnectInfoToNetworkManager();
            this.connectInfo.SaveToFile();

            // ClientManagerでMLAPIのコールバック等を設定
            this.clientManager.Setup();
            // MLAPIでクライアントとして起動
            var tasks = MLAPI.NetworkingManager.Singleton.StartClient();
            this.clientManager.SetSocketTasks(tasks);

            Debug.LogWarning("ConnectClient");
        }

        // Resetボタンを押したとき
        public void OnClickReset()
        {
            this.connectInfo = ConnectInfo.GetDefault();
            //ApplyConnectInfoToUI();
        }

        // ロードした接続設定をUIに反映させます
        /**private void ApplyConnectInfoToUI()
        {
            this.useRelayToggle.isOn = this.connectInfo.useRelay;

            this.ipInputField.text = this.connectInfo.ipAddr;
            this.portInputField.text = this.connectInfo.port.ToString();

            this.relayIpInputField.text = this.connectInfo.relayIpAddr;
            this.relayPortInputField.text = this.connectInfo.relayPort.ToString();

            this.playerNameInputFiled.text = this.connectInfo.playerName;
        }**/
        // 接続設定をUIから構築します
        private void GenerateConnectInfoValueFromUI()
        {
            this.connectInfo.useRelay = false;
            this.connectInfo.ipAddr = "34.84.158.216";
            this.connectInfo.port = 7777;
            this.connectInfo.relayIpAddr = "34.84.158.216";
            this.connectInfo.relayPort = 8888;

            this.connectInfo.playerName = "test";
        }


        // 接続設定をMLAPIのネットワーク設定に反映させます
        private void ApplyConnectInfoToNetworkManager()
        {
            // NetworkManagerから通信実体のTransportを取得します
            var transport = MLAPI.NetworkingManager.Singleton.NetworkConfig.NetworkTransport;

            // ※UnetTransportとして扱います
            var unetTransport = transport as MLAPI.Transports.UNET.UnetTransport;
            if (unetTransport != null)
            {
                // relayサーバー使用するか？
                unetTransport.UseMLAPIRelay = false;

                if (this.connectInfo.useRelay)
                {
                    unetTransport.MLAPIRelayAddress = "34.84.158.216".Trim();
                    unetTransport.MLAPIRelayPort = 8888;
                }
                // 接続先アドレス指定(Client時)
                unetTransport.ConnectAddress = "34.84.158.216".Trim();
                // 接続ポート番号指定
                unetTransport.ConnectPort = 7777;
                // サーバー側でのポート指定
                unetTransport.ServerListenPort = 7777;
            }

            // あとPlayer名をStatic変数に保存しておきます
            playerName = "test";
        }
    }
}