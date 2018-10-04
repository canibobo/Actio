namespace Actio.Common.Events
{
    public class AuthenticateUserRejected : IRejectedEvent
    {
        protected AuthenticateUserRejected(string email)
        {
            Email = email;
        }
        public AuthenticateUserRejected(string reason, string code, string email)
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
