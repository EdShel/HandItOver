using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.BLL.Services.Admin
{
    public abstract class HostedService : IHostedService
    {
        private Task executingTask = null!;

        private CancellationTokenSource cancellationTokenSource = null!;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            this.executingTask = ExecuteAsync(this.cancellationTokenSource.Token);

            return this.executingTask.IsCompleted ? this.executingTask : Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (this.executingTask == null)
            {
                return;
            }

            this.cancellationTokenSource.Cancel();

            await Task.WhenAny(this.executingTask, Task.Delay(-1, cancellationToken));

            cancellationToken.ThrowIfCancellationRequested();
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
