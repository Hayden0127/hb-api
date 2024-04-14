using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Database.Repositories;
using HB.Utilities;

namespace HB.Service
{
    public class RunningSequenceService : IRunningSequenceService
    {
        #region Fields
        private readonly IRunningSequenceRepository _runningSequenceRepository;

        #endregion

        #region Ctor
        public RunningSequenceService(IRunningSequenceRepository runningSequenceRepository)
        {
            _runningSequenceRepository = runningSequenceRepository ?? throw new Exception(nameof(runningSequenceRepository));
        }
        #endregion

        #region Methods

        public async Task<string> GetTransactionIdAsync()
        {
            string transactionId = string.Empty;
            var obj = _runningSequenceRepository.ToQueryable()?.Where(c => c.Type == SystemData.RunningSequenceNumber.TransactionId).FirstOrDefault() ?? throw new Exception("Sequence Type not found");

            int sequenceNumber = obj.SequenceNumber;
            obj.SequenceNumber += 1;
            await _runningSequenceRepository.UpdateAndSaveChangesAsync(obj);

            transactionId = obj.SequenceNumber.ToString("00000000");

            return transactionId;
        }

      
        #endregion
    }
}
