
namespace Application.Common;

public static class MessageCreator
{
   // private static readonly IStringLocalizer _stringLocalizer = Localizer.CustomLocalizer;

    public static string GetMessage(params Enum[] messages)
    {
        switch (messages.Length)
        {
            case 1:
                {
                    return messages[0].ToString();
                }
            case 2:
                {
                    return messages[0].ToString() + " " +messages[1].ToString();
                }
            case 3:
                {
                    return messages[0].ToString() + "" +messages[1].ToString() + " " + messages[2].ToString();
                }
        }

        return string.Empty;
    }
}
