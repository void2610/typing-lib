using UnityEngine;
using VContainer;
using VContainer.Unity;
using Void2610.TypingLib.Core.Models;
using Void2610.TypingLib.Extensions;

namespace Void2610.TypingGame
{
    public class TypingGameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            var settings = new TypingSessionSettings(true, true, false);
            builder.RegisterTypingLib(settings);
            
            builder.RegisterEntryPoint<TypingGamePresenter>();
        }
    }
}
