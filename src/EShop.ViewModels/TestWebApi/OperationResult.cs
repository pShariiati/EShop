namespace EShop.ViewModels.TestWebApi
{
    public class OperationResult<TResult>
    {
        public OperationResult(bool isSuccess, TResult result)
        {
            IsSuccess = isSuccess;
            Result = result;
        }

        public bool IsSuccess { get; set; }

        public TResult Result { get; set; }
    }
}