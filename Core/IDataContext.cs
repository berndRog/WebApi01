using WebApi.Core.DomainModel.Entities;
namespace WebApi.Core;

public interface IDataContext {
   public ICollection<Person> People { get; }
   public void SaveAllChanges();
}