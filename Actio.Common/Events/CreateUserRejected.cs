namespace Actio.Common.Events
{
    public class CreateUserRejected : IRejectedEvent
    {
        protected CreateUserRejected(string email)
        {
            Email = email;
        }
        public CreateUserRejected(string reason, string code, string email)
        {
            Reason = reason;
            Code = code;
            Email = email;
        }

        public string Email { get; }
        public string Reason { get; }
        public string Code { get; }
    }
}
