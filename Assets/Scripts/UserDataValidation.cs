public static class UserDataValidation
{
    public static bool IsValidEmail(ref string email)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    public static bool IsValidPassword(ref string password)
    {
        return !string.IsNullOrEmpty(password) &&
               password.Length > 6 &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[A-Za-z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, @"[0-9]");
    }



    public static bool IsValidUsername(ref string username)
    {
        return !string.IsNullOrEmpty(username) && username.Length >= 4;
    }
}