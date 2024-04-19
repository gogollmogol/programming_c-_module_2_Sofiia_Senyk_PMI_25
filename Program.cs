using System;

public interface ITransaction
{
    void ExecuteTransaction();
    void CheckTransactionStatus();
}

public class FinancialTransaction : ITransaction
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }

    public void ExecuteTransaction()
    {
        Console.WriteLine("Transaction executed successfully.");
    }

    public void CheckTransactionStatus()
    {
        Console.WriteLine("Transaction status checked.");
    }
}

public class FinancialTransactionProcessor
{
    public delegate void TransactionDelegate();

    public event EventHandler<TransactionEventArgs> TransactionProcessed;

    public void ExecuteTransactionAsync(TransactionDelegate transactionDelegate)
    {
        transactionDelegate.BeginInvoke(TransactionCallback, null);
    }

    private void TransactionCallback(IAsyncResult result)
    {
        Console.WriteLine("Transaction processing finished.");
    }

    protected virtual void OnTransactionProcessed(TransactionEventArgs e)
    {
        TransactionProcessed?.Invoke(this, e);
    }
}

public class TransactionEventArgs : EventArgs
{
    public bool IsSuccess { get; }
    public string Message { get; }

    public TransactionEventArgs(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}

public class TransactionException : Exception
{
    public TransactionException(string message) : base(message)
    {
    }

    public TransactionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public string GetFullStackTrace()
    {
        return base.ToString();
    }

    public override string ToString()
    {
        return $"TransactionException: {Message}\n{GetFullStackTrace()}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var transaction = new FinancialTransaction
        {
            Amount = 100,
            Date = DateTime.Now,
            Description = "Test transaction"
        };

        Console.WriteLine("Financial Transaction Processing System");
        Console.WriteLine("1. Execute Transaction");
        Console.WriteLine("2. Check Transaction Status");
        Console.WriteLine("3. Exit");

        Console.Write("Enter your choice: ");
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
        {
            Console.WriteLine("Invalid input. Please enter a valid choice.");
            Console.Write("Enter your choice: ");
        }

        switch (choice)
        {
            case 1:
                ExecuteTransaction(transaction);
                break;
            case 2:
                CheckTransactionStatus(transaction);
                break;
            case 3:
                Environment.Exit(0);
                break;
        }
    }

    static void ExecuteTransaction(FinancialTransaction transaction)
    {
        try
        {
            Console.WriteLine("Transaction executed successfully.");
        }
        catch (TransactionException ex)
        {
            Console.WriteLine($"Transaction failed: {ex.Message}");
        }
    }

    static void CheckTransactionStatus(FinancialTransaction transaction)
    {
        transaction.CheckTransactionStatus();
    }
}
