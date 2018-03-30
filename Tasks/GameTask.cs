using com.clusterrr.hakchi_gui.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace com.clusterrr.hakchi_gui.Tasks
{
    public class GameTask
    {
        public List<NesApplication> Games
        {
            get; private set;
        }

        public Dictionary<NesApplication, string> GamesChanged
        {
            get; private set;
        }

        public GameTask()
        {
            Games = new List<NesApplication>();
            GamesChanged = new Dictionary<NesApplication, string>();
        }

        public TaskerForm.Conclusion SetCoverArtForMultipleGames(TaskerForm tasker, Object SyncObject = null)
        {
            tasker.SetTitle(Resources.ApplyChanges);
            tasker.SetProgress(0, 100, TaskerForm.State.Running, Resources.ApplyChanges);

            int i = 0, max = GamesChanged.Count;
            foreach(var pair in GamesChanged)
            {
                pair.Key.SetImageFile(pair.Value, ConfigIni.Instance.CompressCover);
                tasker.SetProgress(++i, max);
            }

            return TaskerForm.Conclusion.Success;
        }

        public TaskerForm.Conclusion RepairGames(TaskerForm tasker, Object SyncObject = null)
        {
            tasker.SetTitle(Resources.RepairGames);
            tasker.SetState(TaskerForm.State.Running);

            NesApplication.ParentForm = tasker;
            int i = 0, max = Games.Count;
            foreach (var game in Games)
            {
                tasker.SetStatus(string.Format(Resources.RepairingGame, game.Name));
                bool success = game.Repair();
                Debug.WriteLine($"Repairing game \"{game.Name}\" was " + (success ? "successful" : "not successful"));
                tasker.SetProgress(++i, max);
            }

            return TaskerForm.Conclusion.Success;
        }
    }
}
