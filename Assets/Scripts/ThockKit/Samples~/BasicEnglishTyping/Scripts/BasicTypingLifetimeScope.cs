using UnityEngine;
using VContainer;
using VContainer.Unity;
using Void2610.ThockKit.Core.Models;
using Void2610.ThockKit.Extensions;

namespace Void2610.ThockKit.Samples.BasicEnglishTyping
{
    /// <summary>
    /// 基本的な英語タイピングサンプル用のLifetimeScope
    /// </summary>
    public class BasicTypingLifetimeScope : LifetimeScope
    {
        [SerializeField] private BasicTypingView view;

        protected override void Configure(IContainerBuilder builder)
        {
            // ThockKitの登録（英語入力用）
            var settings = TypingSessionSettings.Default;
            builder.RegisterThockKit(settings);

            // Viewの登録
            builder.RegisterComponent(view);

            // Presenterの登録
            builder.RegisterEntryPoint<BasicTypingPresenter>();
        }
    }
}
