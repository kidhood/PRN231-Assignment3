using BadmintonReservationData.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData
{
    public class UnitOfWork : IDisposable
    {
        private const string ErrorNotOpenTransaction = "You not open transaction yet!";
        private const string ErrorAlreadyOpenTransaction = "Transaction already open";
        private NET1711_231_1_BadmintonReservationContext _context;
        private bool isTransaction;
        private bool _disposed;


        private BookingRepository _bookingRepository;
        private CourtRepository _courtRepository;
        private PurchasedRepository _purchasedRepository;
        private BookingDetailRepository _bookingDetailRepository;
        private CustomFrameRepository _customFramRepository;
        private FrameRepository _frameRepository;
        private CustomerRepository _customerRepository;
        private PaymentRepository _paymentRepository;


        private DateTypeRepository _dateTypeRespository;
      	
 	 	public UnitOfWork()
        {
            _context = new NET1711_231_1_BadmintonReservationContext();
        }

        internal NET1711_231_1_BadmintonReservationContext Context { get { return _context; } }

        public bool IsTransaction
        {
            get
            {
                return this.isTransaction;
            }
        }


        public BookingRepository BookingRepository
        {
            get
            {
                return _bookingRepository ??= new BookingRepository(this);
            }
        }

        public CourtRepository CourtRepository 
        {
            get
            {
                return _courtRepository ??= new CourtRepository(this);
            }
        }

        public PurchasedRepository PurchasedRepository
        {
            get
            {
                return _purchasedRepository ??= new PurchasedRepository(this);
            }
        }

        public FrameRepository FrameRepository
        {
            get
            {
                return _frameRepository ??= new FrameRepository(this);
            }
        }

        public CustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new CustomerRepository(this);
            }
        }

        public CustomFrameRepository CustomFrameRepository
        {
            get
            {
                return _customFramRepository ??= new CustomFrameRepository(this);
            }
        }

        public PaymentRepository PaymentRepository
        {
            get
            {
                return _paymentRepository ??= new PaymentRepository(this);
            }
        }

        public DateTypeRepository DateTypeRepository
        {
            get
            {
                return _dateTypeRespository ??=new DateTypeRepository(this);
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (this.isTransaction)
            {
                throw new Exception(ErrorAlreadyOpenTransaction);
            }

            isTransaction = true;
        }

        public async Task CommitTransactionAsync()
        {
            if (!this.isTransaction)
            {
                throw new Exception(ErrorNotOpenTransaction);
            }

            await this._context.SaveChangeAsync().ConfigureAwait(false);
            this.isTransaction = false;
        }

        public void RollbackTransaction()
        {
            if (!this.isTransaction)
            {
                throw new Exception(ErrorNotOpenTransaction);
            }

            this.isTransaction = false;

            foreach (var entry in this._context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    this._context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public BookingDetailRepository BookingDetailRepository
        {
            get
            {
                return _bookingDetailRepository ??= new BookingDetailRepository(this);
            }
        }

    }
}
