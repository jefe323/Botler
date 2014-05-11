using System.Collections.Generic;

namespace Botler.Plugin.Core
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }
        string Version { get; }
        string Description { get; }
        List<string> Commands { get; }
    }
}
