namespace ShopModels.Exceptions;

public class EntityNotFoundException(int entityId, string entityName) : Exception($"{entityName} with id {entityId} was not found.")
{
	public int EntityId => entityId; //for logging
}