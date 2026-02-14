using VContainer;
using Void2610.ThockKit.Core.Interfaces;
using Void2610.ThockKit.Core.Models;
using Void2610.ThockKit.Services;

namespace Void2610.ThockKit.Extensions
{
    /// <summary>
    /// VContainer用拡張メソッド
    /// </summary>
    public static class VContainerExtensions
    {
        /// <summary>
        /// ThockKitのサービスをカスタム設定でDIコンテナに登録する（英語入力用）
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        /// <param name="settings">セッション設定</param>
        /// <param name="sessionLifetime">セッションのライフタイム</param>
        /// <returns>コンテナビルダー（メソッドチェーン用）</returns>
        public static IContainerBuilder RegisterThockKit(
            this IContainerBuilder builder,
            TypingSessionSettings settings,
            Lifetime sessionLifetime = Lifetime.Scoped)
        {
            builder.Register<EnglishInputValidator>(Lifetime.Singleton).As<IInputValidator>();
            builder.RegisterInstance(settings);
            builder.Register<TypingSession>(sessionLifetime).As<ITypingSession>();

            return builder;
        }

        /// <summary>
        /// ThockKitのサービスをカスタム設定でDIコンテナに登録する（日本語入力用）
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        /// <param name="settings">セッション設定</param>
        /// <param name="sessionLifetime">セッションのライフタイム</param>
        /// <returns>コンテナビルダー（メソッドチェーン用）</returns>
        public static IContainerBuilder RegisterThockKitJapanese(
            this IContainerBuilder builder,
            TypingSessionSettings settings,
            Lifetime sessionLifetime = Lifetime.Scoped)
        {
            builder.Register<JapaneseInputValidator>(Lifetime.Singleton)
                .As<IInputValidator>()
                .As<IJapaneseInputValidator>();
            builder.RegisterInstance(settings);
            builder.Register<TypingSession>(sessionLifetime).As<ITypingSession>();

            return builder;
        }
    }
}
