namespace Api.Common.Contracts.Entities
{
    public interface IBaseViewModel
    {
        Guid Id { get; set; }

        DateTime CreateDate { get; set; }

        DateTime? ModifiedDate { get; set; }

        int Version { get; set; }
    }
}