using PKHeX.Core;
using System.Reflection;

namespace SVivillonPlugin {
  public class SVivillonPlugin : IPlugin {

    public string Name => nameof(SVivillonPlugin);
    public int Priority => 1; // Loading order, lowest is first.
    public ISaveFileProvider SaveFileEditor { get; private set; } = null!;
    public IPKMView PKMEditor { get; private set; } = null!;
    ToolStripMenuItem openFormButton = null!;

    private bool IsCompatibleSave {
      get { return SaveFileEditor.SAV is SAV9SV; }
    }

    public void Initialize(params object[] args) {
      Console.WriteLine($"Loading {Name}...");
      SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
      PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
      ToolStrip menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
      ToolStripDropDownItem tools = (ToolStripDropDownItem)menu.Items.Find("Menu_Tools", false)[0]!;
      openFormButton = new ToolStripMenuItem("Vivillon Form Changer") {
        Image = Properties.Resources.F18_Fancy
      };
      openFormButton.Click += (s, e) => new VivillonForm((SAV9SV)SaveFileEditor.SAV).ShowDialog();
      openFormButton.Available = IsCompatibleSave;
      tools.DropDownItems.Add(openFormButton);
    }

    public void NotifySaveLoaded() {
      Console.WriteLine($"{Name} was notified that a Save File was just loaded.");
      openFormButton.Available = IsCompatibleSave;
    }

    public bool TryLoadFile(string filePath) {
      Console.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
      return false; // no action taken
    }

  }
}
