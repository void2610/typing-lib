using VContainer;
using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Core.Models;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Extensions
{
    /// <summary>
    /// VContainer用拡張メソッド
    /// </summary>
    public static class VContainerExtensions
    {
        /// <summary>
        /// TypingLibのサービスをカスタム設定でDIコンテナに登録する
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        /// <param name="settings">セッション設定</param>
        /// <param name="sessionLifetime">セッションのライフタイム</param>
        /// <returns>コンテナビルダー（メソッドチェーン用）</returns>
        public static IContainerBuilder RegisterTypingLib(
            this IContainerBuilder builder,
            TypingSessionSettings settings,
            Lifetime sessionLifetime = Lifetime.Scoped)
        {
            builder.Register<EnglishInputValidator>(Lifetime.Singleton).As<IInputValidator>();
            builder.RegisterInstance(settings);
            builder.Register<TypingSession>(sessionLifetime).As<ITypingSession>();

            return builder;
        }
    }
}
