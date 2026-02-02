using VContainer;
using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Extensions
{
    /// <summary>
    /// VContainer用拡張メソッド
    /// </summary>
    public static class VContainerExtensions
    {
        /// <summary>
        /// TypingLibのサービスをDIコンテナに登録する
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        /// <returns>コンテナビルダー（メソッドチェーン用）</returns>
        public static IContainerBuilder RegisterTypingLib(this IContainerBuilder builder)
        {
            builder.Register<EnglishInputValidator>(Lifetime.Singleton).As<IInputValidator>();
            builder.Register<TypingSession>(Lifetime.Scoped).As<ITypingSession>();

            return builder;
        }

        /// <summary>
        /// TypingLibのサービスをカスタム設定でDIコンテナに登録する
        /// </summary>
        /// <param name="builder">コンテナビルダー</param>
        /// <param name="sessionLifetime">セッションのライフタイム</param>
        /// <param name="caseSensitive">大文字小文字を区別するかどうか</param>
        /// <returns>コンテナビルダー（メソッドチェーン用）</returns>
        public static IContainerBuilder RegisterTypingLib(
            this IContainerBuilder builder,
            Lifetime sessionLifetime,
            bool caseSensitive = true)
        {
            builder.Register<EnglishInputValidator>(Lifetime.Singleton)
                .WithParameter("IsCaseSensitive", caseSensitive)
                .As<IInputValidator>();

            builder.Register<TypingSession>(sessionLifetime).As<ITypingSession>();

            return builder;
        }
    }
}
