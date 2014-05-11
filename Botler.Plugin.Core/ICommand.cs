using System.Collections.Generic;

namespace Botler.Plugin.Core
{
    public interface ICommand
    {
        string Command { get; }
        List<string> Aliases { get; }
        HelpText help { get; }
        string DoWork(string[] data);
    }
}
