using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Common;

public static class DateTimeExtensions
{
    public static string ToPersian(this DateTime dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        DateTime dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,dateTime.Hour,dateTime.Minute,dateTime.Second); // یا DateTime.Now
        string PersianDate = string.Format("{0}/{1}/{2} | {3}:{4}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt),pc.GetHour(dt),pc.GetMinute(dt));

        return PersianDate;
    }

    public static string ToPersianWithoutTime(this DateTime dateTime)
    {
        PersianCalendar pc = new PersianCalendar();
        DateTime dt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second); // یا DateTime.Now
        string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));

        return PersianDate;
    }

    public static DateTime ToMiladi(this string dateTime)
    {
        Calendar persian = new PersianCalendar();
        string[] s = dateTime.Split("/");
        DateTime date = new DateTime(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), Convert.ToInt32(s[2]), persian);

        return date;
    }

	public static string getDay(DayOfWeek day)
	{
		string Result = "";
		switch (day)
		{
			case DayOfWeek.Friday:
				Result = "جمعه";
				break;
			case DayOfWeek.Monday:
				Result = "دوشنبه";
				break;
			case DayOfWeek.Saturday:
				Result = "شنبه";
				break;
			case DayOfWeek.Sunday:
				Result = "یکشنبه";
				break;
			case DayOfWeek.Thursday:
				Result = "پنج شنبه";
				break;
			case DayOfWeek.Tuesday:
				Result = "سه شنبه";

				break;
			case DayOfWeek.Wednesday:
				Result = "چهارشنبه";
				break;
			default:
				break;
		}
		return Result;
	}
	public static string GetMounth(int month)
	{
		string[] monthInYear = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
		return monthInYear[month - 1];
	}
	public static string GetHour(DateTime lastdate)
	{
		PersianCalendar pc = new PersianCalendar();
		string result = " " + (((pc.GetHour(lastdate)) < 10) ? ("0" + pc.GetHour(lastdate).ToString()) : (pc.GetHour(lastdate)).ToString()) + ":" + (((pc.GetMinute(lastdate)) < 10) ? ("0" + pc.GetMinute(lastdate).ToString()) : (pc.GetMinute(lastdate)).ToString());
		return result;
	}
	public static string ToPersianLastSeen(this DateTime dateTime)
	{
		PersianCalendar pc = new PersianCalendar();
		DateTime LastDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second); // یا DateTime.Now
		TimeSpan ts = DateTime.Now - LastDate;
		int DifferenceYear = DateTime.Now.Year - LastDate.Year;
		int DiffernceMounth = DateTime.Now.Month - LastDate.Month;
		if (DateTime.Now.Month > LastDate.Month)
			DiffernceMounth = DateTime.Now.Month - LastDate.Month;
		else
			DiffernceMounth = LastDate.Month - DateTime.Now.Month;
		int DifferenceDays = ts.Days;
		DifferenceYear = DifferenceDays / 365;
		DiffernceMounth = DifferenceDays / 30;

		StringBuilder Result = new System.Text.StringBuilder("");

		if (DifferenceYear > 0)
		{
			Result.Append(DifferenceYear.ToString() + " سال پیش" + " ، " + getDay(pc.GetDayOfWeek(LastDate)) + " " + pc.GetDayOfMonth(LastDate).ToString() + " " + GetMounth(pc.GetMonth(LastDate)) + " " + pc.GetYear(LastDate) + GetHour(LastDate));
		}
		else if (DiffernceMounth > 0)
		{
			Result.Append(DiffernceMounth.ToString() + " ماه پیش" + " ، " + getDay(pc.GetDayOfWeek(LastDate)) + " " + pc.GetDayOfMonth(LastDate).ToString() + " " + GetMounth(pc.GetMonth(LastDate)) + " " + pc.GetYear(LastDate) + GetHour(LastDate));
		}
		else if (DifferenceDays > 0)
			Result.Append(DifferenceDays.ToString() + " روز پیش" + " ، " + getDay(pc.GetDayOfWeek(LastDate)) + GetHour(LastDate));
		else if (DifferenceDays == 0 && DateTime.Now.DayOfYear - dateTime.DayOfYear == 1)
			Result.Append("1 روز پیش");
		else if (DifferenceDays == 0)
			Result.Append("امروز" + GetHour(LastDate));

		return Result.ToString();
	}
	public static string PersianToEnglishNumber(this string persianStr)
    {
        Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
        {
            ['۰'] = '0',
            ['۱'] = '1',
            ['۲'] = '2',
            ['۳'] = '3',
            ['۴'] = '4',
            ['۵'] = '5',
            ['۶'] = '6',
            ['۷'] = '7',
            ['۸'] = '8',
            ['۹'] = '9'
        };
        foreach (var item in persianStr)
        {
            persianStr = persianStr.Replace(item, LettersDictionary[item]);
        }
        return persianStr;
    }

	public static string toEnglishNumber(this string input)
	{
		string EnglishNumber = "";
		for (int i = 0; i < input.Length; i++)
		{
			if (Char.IsDigit(input[i]))
			{
				EnglishNumber += char.GetNumericValue(input, i);
			}
			else
			{
				EnglishNumber += input[i].ToString();
			}
		}

		return EnglishNumber;
	}

    public static string GetDayOfMounthName(int day)
    {
        string s = "";
        switch (day)
        {
            case 1:
                s="اول";
                break;
			case 2:
				s = "دوم";
				break;
			case 3:
				s = "سوم";
				break;
			case 4:
				s = "چهارم";
				break;
			case 5:
				s = "پنجم";
				break;
			case 6:
				s = "ششم";
				break;
			case 7:
				s = "هفتم";
				break;
			case 8:
				s = "هشتم";
				break;
			case 9:
				s = "نهم";
				break;
			case 10:
				s = "دهم";
				break;
			case 11:
				s = "یازدهم";
				break;
			case 12:
				s = "دوازدهم";
				break;
			case 13:
				s = "سیزدهم";
				break;
			case 14:
				s = "چهاردهم";
				break;
			case 15:
				s = "پانزدهم";
				break;
			case 16:
				s = "شانزدهم";
				break;
			case 17:
				s = "هفدهم";
				break;
			case 18:
				s = "هجدهم";
				break;
			case 19:
				s = "نوزدهم";
				break;
			case 20:
				s = "بیستم";
				break;
			case 21:
				s = "بیست و یکم";
				break;
			case 22:
				s = "بیست و دوم";
				break;
			case 23:
				s = "بیست و سوم";
				break;
			case 24:
				s = "بیست و چهارم";
				break;
			case 25:
				s = "بیست و پنجم";
				break;
			case 26:
				s = "بیست و ششم";
				break;
			case 27:
				s = "بیست و هفتم";
				break;
			case 28:
				s = "بیست و هشتم";
				break;
			case 29:
				s = "بیست و نهم";
				break;
			case 30:
				s = "سی ام";
				break;
			default:
                s="در بازه ماه رمضان نیستیم";
                break;
        }

        return s;
    }

	public static long MillisecondsTimestamp(this DateTime date)
	{
		DateTime baseDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (long)(date.ToUniversalTime() - baseDate).TotalMilliseconds;
	}

	public static Boolean IsValidNationalCode(this String nationalCode)
    {
        //در صورتی که کد ملی وارد شده تهی باشد

        if (String.IsNullOrEmpty(nationalCode))
            throw new Exception("لطفا کد ملی را صحیح وارد نمایید");


        //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
        if (nationalCode.Length != 10)
            throw new Exception("طول کد ملی باید ده کاراکتر باشد");

        //در صورتی که کد ملی ده رقم عددی نباشد
        var regex = new Regex(@"\d{10}");
        if (!regex.IsMatch(nationalCode))
            throw new Exception("کد ملی تشکیل شده از ده رقم عددی می‌باشد؛ لطفا کد ملی را صحیح وارد نمایید");

        //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
        var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
        if (allDigitEqual.Contains(nationalCode)) return false;


        //عملیات شرح داده شده در بالا
        var chArray = nationalCode.ToCharArray();
        var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
        var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
        var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
        var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
        var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
        var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
        var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
        var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
        var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
        var a = Convert.ToInt32(chArray[9].ToString());

        var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
        var c = b % 11;

        return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
    }

    public static bool IsItNumber(this string inputvalue)
    {
        var isnumber = new Regex("[^0-9]");
        return !isnumber.IsMatch(inputvalue);
    }
}
