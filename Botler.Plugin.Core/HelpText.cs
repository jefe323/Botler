
namespace Botler.Plugin.Core
{
    public class HelpText
    {
        /// <summary>
        /// Title of the help data (usually the name of the command)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A description of the command (what it's meant to do, etc...)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// How the command is used
        /// </summary>
        public string Usage { get; set; }

        /// <summary>
        /// If any flags are used, describe them here
        /// </summary>
        public string Flags { get; set; }

        /// <summary>
        /// An example of the command being used (not to be confused with Usage which is a general look at the command)
        /// </summary>
        public string Example { get; set; }

    }
}
