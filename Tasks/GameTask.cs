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

        public Tasker.Conclusion SetCoverArtForMultipleGames(Tasker tasker, Object SyncObject = null)
        {
            tasker.SetTitle(Resources.ApplyChanges);
            tasker.SetProgress(0, 100, Tasker.State.Running, Resources.ApplyChanges);

            int i = 0, max = GamesChanged.Count;
            foreach(var pair in GamesChanged)
            {
                pair.Key.SetImageFile(pair.Value, ConfigIni.Instance.CompressCover);
                tasker.SetProgress(++i, max);
            }

            return Tasker.Conclusion.Success;
        }

        public Tasker.Conclusion RepairGames(Tasker tasker, Object SyncObject = null)
        {
            tasker.SetTitle(Resources.RepairGames);
            tasker.SetState(Tasker.State.Running);

            NesApplication.ParentForm = tasker.HostForm;
            int i = 0, max = Games.Count;
            foreach (var game in Games)
            {
                tasker.SetStatus(string.Format(Resources.RepairingGame, game.Name));
                bool success = game.Repair();
                Debug.WriteLine($"Repairing game \"{game.Name}\" was " + (success ? "successful" : "not successful"));
                tasker.SetProgress(++i, max);
            }

            return Tasker.Conclusion.Success;
        }
    }
}
