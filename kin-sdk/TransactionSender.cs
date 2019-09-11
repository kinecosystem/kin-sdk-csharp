using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Kin.Base;
using Kin.Base.responses;
using Kin.Base.requests;

namespace Kin.Sdk
{
    class TransactionSender
    {
        private readonly Server server;
        private readonly string appId;

        private const decimal ONE = 100000m; // Quarks in kin
        private const int MemoBytesLimit = 21;
        private const string MemoAppIdVersionPrefix = "1";
        private const string MemoDelimeter = "-";
        
        internal TransactionSender(Server server, string appId)
        {
            this.server = server;
            this.appId = appId;
        }

        // internal async Task<Transaction> BuildTransaction(KeyPair source, string destination, decimal amount, UInt32 fee, string memo=null)
        // {
        //     // Validate params
        //     source.ThrowIfNull("source");
        //     destination.ThrowIfNull("destination");
        //     amount.ThrowIfNull("amount");
        //     fee.ThrowIfNull("fee");

        //     await this.ValidateDestination(destination);

        //     ValidateAmount(amount);
        //     ValidateMemo(memo);
            
        //     if (!String.IsNullOrEmpty(memo))
        //     {
        //         memo = this.AddAppIdToMemo(memo);
        //     }

        //     KeyPair destinationKeyPair = KeyPair.FromAccountId(destination);
        //     AccountResponse sourceResponse = await this.LoadAccount(source.AccountId);

        // }

        // private Kin.Base.Transaction buildBaseTransaction(KeyPair source, AccountResponse accountResponse, string destination, decimal amount, UInt32 fee, string memo=null)
        // {
        //     Kin.Base.Transaction.Builder transactionBuilder = 
        //     new Kin.Base.Transaction.Builder(new Account(source.AccountId, accountResponse.SequenceNumber));

        //     transactionBuilder.SetFee((int) fee); // Change kin-base to get uint32
        //     if (memo != null)
        //     {
        //         transactionBuilder.AddMemo(Memo.Text(memo));
        //     }

        // }

        internal async Task<string> SendKin(KeyPair source, string destination, decimal amount, UInt32 fee, string memo=null)
        {
            AccountResponse sourceAccountResponse = await this.server.Accounts.Account(source.AccountId);
            Account sourceAccount = new Account(source.AccountId, sourceAccountResponse.SequenceNumber);
            Kin.Base.Transaction.Builder transactionBuilder = new Kin.Base.Transaction.Builder(sourceAccount);

            transactionBuilder.SetFee((int) fee); // Change kin-base to accept uint32
            if (!String.IsNullOrEmpty(memo) && this.appId != "")
            {
                memo = this.AddAppIdToMemo(memo);
                transactionBuilder.AddMemo(new MemoText(memo));
            }

            transactionBuilder.AddOperation(new PaymentOperation.Builder(KeyPair.FromAccountId(destination), 
                                            new Kin.Base.AssetTypeNative(), amount.ToString()).Build());

            Kin.Base.Transaction finalTransaction = transactionBuilder.Build();
            finalTransaction.Sign(source);

            try
            {
                SubmitTransactionResponse response = await this.server.SubmitTransaction(finalTransaction);
                if (response.IsSuccess())
                {
                    return response.Hash;
                }
                else
                {
                    throw new OperationFailedException($"Transaction failed : {response.Result.GetType().ToString()}");
                }
            }
            catch (Exception e)
            {
                throw new OperationFailedException("Failed to send transaction", e);
            }
            
        }
        
        private async Task ValidateDestination(string destination)
        {
            if (!Kin.Base.StrKey.IsValidEd25519PublicKey(destination))
            {
                throw new ArgumentException($"Destination {destination} is an invalid public key");
            }

            // Check that the destination exists
            await this.LoadAccount(destination);

        }

        private void ValidateAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative");
            }
            if ((amount * ONE) % 1 != 0)
            {
                throw new ArgumentException("Amount can't have more then 5 digits after the decimal point");
            }
        }

        private void ValidateMemo(string memo)
        {
            if (memo != null)
            {
                if (Encoding.UTF8.GetByteCount(memo) > MemoBytesLimit)
                {
                    throw new ArgumentException($"Memo cannot be longer than {MemoBytesLimit} bytes(UTF-8 charcters)");
                }
            }
        }

        private async Task<AccountResponse> LoadAccount(string publicAddress)
        {
            try
            {
                return await server.Accounts.Account(publicAddress);
            }
            catch (HttpResponseException e)
            {
                if (e.StatusCode == 404)
                {
                    throw new AccountNotFoundException(publicAddress);
                }
                else
                {
                    throw new OperationFailedException("Failed to retrive account info", e);
                }
            }
            catch (Exception e)
            {
                throw new OperationFailedException("Failed to retrive account info", e);
            }
        }

        private string AddAppIdToMemo(string memo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MemoAppIdVersionPrefix)
            .Append(MemoDelimeter).Append(this.appId).Append(MemoDelimeter).Append(memo);

            return sb.ToString();
        }
    }
}
