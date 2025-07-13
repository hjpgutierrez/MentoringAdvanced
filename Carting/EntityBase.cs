namespace Carting
{
    /// <summary>
    /// This class is used to filter the collections/repositories by id in a generic manner.
    /// It should be created into a common layer, that's why it is in the root for this project.
    /// </summary>
    public class EntityBase
    {
        public required string Code { get; set; }
    }
}
