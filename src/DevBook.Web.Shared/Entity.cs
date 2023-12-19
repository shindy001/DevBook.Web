namespace DevBook.Web.Shared;

/// <summary>
/// Represent Entity with id - base for entities with identifier
/// </summary>
/// <param name="Id">Identifier</param>
public abstract record Entity(Guid Id);
