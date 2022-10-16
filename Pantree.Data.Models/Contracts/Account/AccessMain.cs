namespace Pantree.Data.Models.Contracts
{
    public class AccessMain
    {
        public SignInSubmit SignIn { get; set; }
        public SignUpSubmit SignUp { get; set; }

        public string? Redirect { get; set; }

        public AccessMain()
        {
            SignIn = new SignInSubmit();
            SignUp = new SignUpSubmit();
        }

        public AccessMain(string? redirect)
        {
            SignIn = new SignInSubmit();
            SignUp = new SignUpSubmit();
            Redirect = redirect;
        }
    }
}