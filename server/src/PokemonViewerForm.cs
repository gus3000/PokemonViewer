﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BizHawk.Client.Common;
using BizHawk.Client.EmuHawk;
using BizHawk.Common;
using DefaultNamespace;
using PokemonViewer.Memory;
using PokemonViewer.Memory.Global;
using PokemonViewer.Network;
using PokemonViewer.PokemonData;

namespace PokemonViewer
{
    [ExternalTool("PokemonViewer")]
    public sealed class PokemonViewerForm : ToolFormBase, IExternalToolForm
    {
        protected override string WindowTitleStatic => "Pokemon Viewer";

        public ApiContainer? _maybeAPIContainer { get; private set; }

        private ApiContainer APIs => _maybeAPIContainer!;

        public GameData? GameData { get; private set; }
        public Server Server { get; private set; }


        private Label labelInfo;
        private IList<Task> tasks;
        private StringBuilder currentStatus;

        public PokemonViewerForm()
        {
            ClientSize = new Size(480, 800);
            Server = new Server();
            tasks = new List<Task>();
            currentStatus = new StringBuilder();
            SuspendLayout();
            
            Controls.Add(labelInfo = new Label { AutoSize = true, MaximumSize = new Size(480,0)});
            ResumeLayout();
            Log.EnableDomain("NetworkDebug");
        }

        public override void Restart()
        {
            UpdateData();
            labelInfo.Text = string.Join("\n", Server.server.ToString());
        }

        protected override void UpdateAfter()
        {
            currentStatus.Clear();
            UpdateData();
            tasks.Add(Server.SendData(GameData));
            HandleTasks();
            currentStatus.AppendLine($"last served : {Server.lastServedTimestamp}");
            
            
            labelInfo.Text = currentStatus.ToString();
        }

        private void HandleTasks()
        {
            IList<int> toRemove = new List<int>();
            for(int i=0; i<tasks.Count; i++)
            {
                if (tasks[i].IsCompleted)
                {
                    toRemove.Add(i);
                }
            }

            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                tasks.RemoveAt(toRemove[i]);
            }

            currentStatus.AppendLine($"current server tasks : {tasks.Count}");
        }
        
        protected override async void OnClosing(CancelEventArgs e)
        {
            foreach (Task task in tasks)
            {
                await task;
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // Log.Error("NetworkDebug","coucou");
            labelInfo.Text = "return : " + Server.SendData(GameData);
            // labelInfo.Text = "clicked";
        }

        private void UpdateData()
        {
            GameData = new GameData(APIs.Memory);
        }

        private string ReadData()
        {
            return GameData.Serialize();
        }
    }
}