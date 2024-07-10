using BadmintonReservationData.Base;

namespace BadmintonReservationData.Repository
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(UnitOfWork unitOfWork) : base(unitOfWork)
        {            
        }

        public Customer GetAccount(string phoneNumber, string password)
        {
            return this._dbSet.SingleOrDefault(x => x.PhoneNumber == phoneNumber && x.Password == password);
        }
    }
}
