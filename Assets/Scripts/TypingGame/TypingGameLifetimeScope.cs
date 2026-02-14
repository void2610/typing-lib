using VContainer;
using VContainer.Unity;
using Void2610.ThockKit.Core.Models;
using Void2610.ThockKit.Extensions;

namespace Void2610.TypingGame
{
    public class TypingGameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var settings = new TypingSessionSettings(true, true, false);
            builder.RegisterThockKitJapanese(settings);

            builder.RegisterEntryPoint<TypingGamePresenter>();
        }
    }
}
