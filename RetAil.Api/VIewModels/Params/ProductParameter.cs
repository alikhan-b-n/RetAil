namespace RetAil.Api.VIewModels.Params;

public record ProductParameter(decimal Price, string Title, string Details, Guid CategoryId);