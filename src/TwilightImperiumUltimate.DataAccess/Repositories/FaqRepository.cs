namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class FaqRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IFaqRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task AddNewFaq(Faq faq, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        await dbContext.Faqs.AddAsync(faq, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteFaq(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        var faq = await dbContext.Faqs.FirstAsync(x => x.Id == id, cancellationToken);
        dbContext.Faqs.Remove(faq);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Faq>> GetAllFaqs(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Faqs.ToListAsync(cancellationToken);
    }

    public async Task<Faq> GetFaqById(int id, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Faqs.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Faq>> GetFaqsByComponenet(string componenetName, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Faqs.Where(x => x.ComponentName == componenetName).ToListAsync(cancellationToken);
    }

    public async Task<Faq> UpdateFaq(Faq faq, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        dbContext.Faqs.Update(faq);
        await dbContext.SaveChangesAsync(cancellationToken);
        return faq;
    }
}
