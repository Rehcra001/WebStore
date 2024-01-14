namespace WebStore.WEB.Validators
{
    internal class CustomValidationRules
    {
        internal bool BeAValidEmailCharacter(string email)
        {
            if (email.Contains(';'))
            {
                return false;
            }
            if (email.Contains("--"))
            {
                return false;
            }
            return true;        
        }

        internal bool BeAValidName(string name)
        {
            name = name.Replace(" ", "");
            name = name.Replace("-","");

            return name.All(char.IsLetter);
        }
    }
}
