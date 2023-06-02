namespace FrameWork.Application.Authentication.PasswordHashing
{
    public sealed class HashingOptions
    {
        public int Iterations { get; set; } = 10000;
    }
}