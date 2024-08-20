namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IFaqRepository
{
    Task AddNewFaq(Faq faq, CancellationToken cancellationToken);

    Task DeleteFaq(int id, CancellationToken cancellationToken);

    Task<Faq> UpdateFaq(Faq faq, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Faq>> GetAllFaqs(CancellationToken cancellationToken);

    Task<Faq> GetFaqById(int id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Faq>> GetFaqsByComponenet(string componenetName, CancellationToken cancellationToken);
}
