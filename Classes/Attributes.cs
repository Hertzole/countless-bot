using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace CountlessBot.Classes
{
    /// <summary>
    /// Ties a color to a module.
    /// </summary>
    public class ModuleColorAttribute : Attribute
    {
        // The color.
        public Color Color { get; }

        public ModuleColorAttribute(byte r, byte g, byte b)
        {
            Color = new Color(r, g, b);
        }

        public ModuleColorAttribute(float r, float g, float b)
        {
            Color = new Color(r, g, b);
        }

        public ModuleColorAttribute(int r, int g, int b)
        {
            Color = new Color(r, g, b);
        }
    }

    /// <summary>
    /// Makes the command hidden from the help commands.
    /// </summary>
    public class HiddenAttribute : Attribute { }

    /// <summary>
    /// Makes it so only the bot owner (probably you) can run this command.
    /// </summary>
    public class BotOwnerRequiredAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (CountlessBot.Instance.Credentials.OwnerIDs.Contains(context.User.Id))
                return Task.FromResult(PreconditionResult.FromSuccess());
            else
                return Task.FromResult(PreconditionResult.FromError("Only the bot owner/developer can run this command."));
        }
    }

    /// <summary>
    /// Disallows commands from being executed in a private chat.
    /// </summary>
    public class NoPrivateChatAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Guild == null)
                return Task.FromResult(PreconditionResult.FromError("This command can't be executed in a private chat."));
            else
                return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }

    /// <summary>
    /// Disallows commands from being executed outside a NSFW channel.
    /// </summary>
    public class NSFWCommandAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            if (context.Guild == null)
                return Task.FromResult(PreconditionResult.FromSuccess());

            return Task.FromResult(context.Channel.IsNsfw == true ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("This is a NSFW command and can only be executed in a NSFW text channel."));
        }
    }
}
