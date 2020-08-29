namespace Eco
{
    using System;

    public interface IEntity
    {
        int Id { get; set; }

        bool IsDeleted { get; set; }

        DateTimeOffset CreatedDate { get; set; }

        DateTimeOffset? ModifiedDate { get; set; }

        string CreatedBy { get; set; }

        string ModifiedBy { get; set; }
    }
}
