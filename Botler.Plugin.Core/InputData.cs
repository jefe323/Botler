
namespace Botler.Plugin.Core
{
    public class InputData
    {
        /// <summary>
        /// Permission Level of the user
        /// </summary>
        public enum NickPermLevel { BotOp, ChanOp, Voiced, None };

        /// <summary>
        /// Where the command originated
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// Who sent the command
        /// </summary>
        public string Nick { get; set; }

        /// <summary>
        /// Permission level of the user who ran the command (BotOp, ChanOp, Voiced, None)
        /// </summary>
        public NickPermLevel Perm { get; set; }

        /// <summary>
        /// Where the command should go
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// The data recieved from the command
        /// </summary>
        public string[] DataArray { get; set; }
    }
}
