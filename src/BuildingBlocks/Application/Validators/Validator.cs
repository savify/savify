using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation;

namespace App.BuildingBlocks.Application.Validators;

public class Validator<T> : AbstractValidator<T>
{
    private const string PhoneNumberRegex = "^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$";

    private const string UrlRegex = @"[(http(s)?):\/\/(www\.)?a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)";

    protected bool BeAValidEmail(string email)
    {
        return MailAddress.TryCreate(email, out _);
    }

    protected bool BeAValidPhoneNumber(string phoneNumber)
    {
        return new Regex(PhoneNumberRegex).IsMatch(phoneNumber);
    }

    protected bool BeAValidUrl(string? url)
    {
        return url == null || new Regex(UrlRegex).IsMatch(url);
    }
}
