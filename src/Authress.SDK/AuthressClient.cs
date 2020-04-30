using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Authress.SDK
{
    public class AuthressClient
    {
        /// <summary>
        /// The maximum log event size = 256 KB - 26 B
        /// </summary>
        public const int MaxLogEventSize = 262118;

        public static readonly TimeSpan ErrorBackoffStartingInterval = TimeSpan.FromMilliseconds(100);

        private readonly String cloudWatchClient;

        private readonly SemaphoreSlim syncObject = new SemaphoreSlim(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudWatchLogSink"/> class.
        /// </summary>
        /// <param name="cloudWatchClient">The cloud watch client.</param>
        /// <param name="options">The options.</param>
        public AuthressClient()
        {

        }

        /// <summary>
        /// Ensures the component is initialized.
        /// </summary>
        private async Task EnsureInitializedAsync()
        {

        }
    }
}
