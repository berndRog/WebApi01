// namespace WebApi._3_Infrastructure._2_Persistence.Database;
//
// internal sealed class PaymentsDbContextEf(BankingDbContext db) : IPaymentsDbContext
// {
//    public IQueryable<Account> Accounts => db.Set<Account>();
//
//    public void Add<T>(T entity) where T : class => db.Set<T>().Add(entity);
//    public void Remove<T>(T entity) where T : class => db.Set<T>().Remove(entity);
// }