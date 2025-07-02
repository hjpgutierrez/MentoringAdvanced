using CleanArchitecture.Domain.Common;

namespace Catalog.Domain.Common
{
    public class DomainEventEqualityComparer : IEqualityComparer<BaseEvent>
    {
        public bool Equals(BaseEvent? x, BaseEvent? y)
        {
            if (x == null || y == null)
                return false;
            

            // Define equality logic (e.g., compare by type or unique properties)
            return x.GetType() == y.GetType();
        }

        public int GetHashCode(BaseEvent obj)
        {
            ArgumentNullException.ThrowIfNull(obj, nameof(obj));
            // Define hash code logic (combine type and unique properties)
            return HashCode.Combine(obj.GetType());
        }
    }
}
