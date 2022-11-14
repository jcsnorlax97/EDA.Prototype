namespace EDA.Prototype.Infrastructure.CQS.Interfaces;

public interface IRequestHandler<TParameter, TResponse>
{
	public Task<TResponse> HandleAsync(TParameter parameter, CancellationToken cancellationToken);
}