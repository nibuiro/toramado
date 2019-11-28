using System;
using System.Threading.Tasks;

public class ReturnableAsyncCommand : AsyncCommandBase
{
    private readonly Func<Task> _command;

    public ReturnableAsyncCommand(Func<Task> command)
    {
        _command = command;
    }

    public override bool CanExecute(object parameter)
    {
        return true;
    }

    public override Task ExecuteAsync(object parameter)
    {
        return _command();
    }
}