namespace EDA.Prototype.Infrastructure.CQS.Interfaces;

public interface ICommandHandler<TParameter>
{
    public Task HandleAsync(TParameter parameter, CancellationToken cancellationToken);
}