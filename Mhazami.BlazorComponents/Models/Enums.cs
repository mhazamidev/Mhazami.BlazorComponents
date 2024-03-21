using System.ComponentModel;

namespace Mhazami.BlazorComponents.Models;




public enum DatePickerType
{
    DateTime,
    Date,
    Time,
    Month,
    Day,
    Year
}
public enum CalendarType
{
    Hijri,
    Shamsi,
    Turkish,
    Gregorian,
    German
}
//Turkish
internal enum TurkishMonths : byte
{
    [Description("Ocak")]
    Ocak = 1,
    [Description("Şubat")]
    Subat = 2,
    [Description("Mart")]
    Mart = 3,
    [Description("Nisan")]
    Nisan = 4,
    [Description("Mayıs")]
    Mayis = 5,
    [Description("Haziran")]
    Haziran = 6,
    [Description("Temmuz")]
    Temmuz = 7,
    [Description("Ağustos")]
    Agustos = 8,
    [Description("Eylül")]
    Eylul = 9,
    [Description("Ekim")]
    Ekim = 10,
    [Description("Kasım")]
    Kasim = 11,
    [Description("Aralık")]
    Aralik = 12,
}
internal enum TurkishDays : byte
{
    [Description("Pazartesi")]
    Pazartesi = 1,
    [Description("Salı")]
    Salı = 2,
    [Description("Çarşamba")]
    Carsamba = 3,
    [Description("Perşembe")]
    Persembe = 4,
    [Description("Cuma")]
    Cuma = 5,
    [Description("Cmartesi")]
    Cmartesi = 6,
    [Description("Pazar")]
    Pazar = 7
}
internal enum TurkishMonthsShort : byte
{
    [Description("Oca")]
    Oca = 1,
    [Description("Şub")]
    Sub = 2,
    [Description("Mar")]
    Mar = 3,
    [Description("Nis")]
    Nis = 4,
    [Description("May")]
    May = 5,
    [Description("Haz")]
    Haz = 6,
    [Description("Tem")]
    Tem = 7,
    [Description("Ağu")]
    Agu = 8,
    [Description("Eyl")]
    Eyl = 9,
    [Description("Eki")]
    Eki = 10,
    [Description("Kas")]
    Kas = 11,
    [Description("Ara")]
    Ara = 12,
}
internal enum TurkishDaysShort : byte
{
    [Description("Pzt")]
    Pzt = 1,
    [Description("Sal")]
    Sal = 2,
    [Description("Çar")]
    Car = 3,
    [Description("Per")]
    Per = 4,
    [Description("Cum")]
    Cum = 5,
    [Description("Cmt")]
    Cmt = 6,
    [Description("Paz")]
    Paz = 7
}
//Shamsi
internal enum ShamsiMonths : byte
{
    [Description("فروردین")]
    Farvardin = 1,
    [Description("اردیبهشت")]
    Ordibehesht = 2,
    [Description("خرداد")]
    Khordad = 3,
    [Description("تیر")]
    Tir = 4,
    [Description("مرداد")]
    Mordad = 5,
    [Description("شهریور")]
    Shahrivar = 6,
    [Description("مهر")]
    Mehr = 7,
    [Description("آبان")]
    Aban = 8,
    [Description("آذر")]
    Azar = 9,
    [Description("دی")]
    Day = 10,
    [Description("بهمن")]
    Bahman = 11,
    [Description("اسفند")]
    Esfand = 12,
}
internal enum ShamsiDays : byte
{
    [Description("شنبه")]
    Shanbe = 1,
    [Description("یکشنبه")]
    YekShanbe = 2,
    [Description("دوشنبه")]
    DoShanbe = 3,
    [Description("سه شنبه")]
    SeShanbe = 4,
    [Description("چهارشنبه")]
    ChaharShanbe = 5,
    [Description("پنجشنبه")]
    PanjShanbe = 6,
    [Description("جمعه")]
    Jomeh = 7
}
internal enum ShamsiMonthsShort : byte
{
    [Description("فروردین")]
    Farvardin = 1,
    [Description("اردیبهشت")]
    Ordibehesht = 2,
    [Description("خرداد")]
    Khordad = 3,
    [Description("تیر")]
    Tir = 4,
    [Description("مرداد")]
    Mordad = 5,
    [Description("شهریور")]
    Shahrivar = 6,
    [Description("مهر")]
    Mehr = 7,
    [Description("آبان")]
    Aban = 8,
    [Description("آذر")]
    Azar = 9,
    [Description("دی")]
    Day = 10,
    [Description("بهمن")]
    Bahman = 11,
    [Description("اسفند")]
    Esfand = 12,
}
internal enum ShamsiDaysShort : byte
{
    [Description("ش")]
    Sh = 1,
    [Description("ی")]
    Yek = 2,
    [Description("د")]
    Do = 3,
    [Description("س")]
    Se = 4,
    [Description("چ")]
    Ch = 5,
    [Description("پ")]
    Pa = 6,
    [Description("ج")]
    Jo = 7
}
//Hijri
internal enum HijriMonths : byte
{
    [Description("محرم")]
    Moharam = 1,
    [Description("صفر")]
    Safar = 2,
    [Description("ربیع‌الاول")]
    RabiAval = 3,
    [Description("ربیع‌الثانی")]
    RabiSani = 4,
    [Description("جمادی‌الاول")]
    JamadiAval = 5,
    [Description("جمادی‌الثانی")]
    JamadiSani = 6,
    [Description("رجب")]
    Rajab = 7,
    [Description("شعبان")]
    Shaban = 8,
    [Description("رمضان")]
    Ramazan = 9,
    [Description("شوال")]
    Shaval = 10,
    [Description("ذیقعده")]
    Zighadeh = 11,
    [Description("ذیحجه")]
    Zihajeh = 12,
}
internal enum HijriDays : byte
{
    [Description("السبت")]
    AsSabt = 1,
    [Description("الأحد")]
    AlAhad = 2,
    [Description("الأثنين")]
    ALithNayn = 3,
    [Description("الثلاثاء")]
    AthThuLaTha = 4,
    [Description("الأربعاء")]
    Alarbaa = 5,
    [Description("الخميس")]
    Alkhamis = 6,
    [Description("الجمعه")]
    Aljumah = 7
}
internal enum HijriMonthsShort : byte
{
    [Description("محرم")]
    Moharam = 1,
    [Description("صفر")]
    Safar = 2,
    [Description("ربیع‌الاول")]
    RabiAval = 3,
    [Description("ربیع‌الثانی")]
    RabiSani = 4,
    [Description("جمادی‌الاول")]
    JamadiAval = 5,
    [Description("جمادی‌الثانی")]
    JamadiSani = 6,
    [Description("رجب")]
    Rajab = 7,
    [Description("شعبان")]
    Shaban = 8,
    [Description("رمضان")]
    Ramazan = 9,
    [Description("شوال")]
    Shaval = 10,
    [Description("ذیقعده")]
    Zighadeh = 11,
    [Description("ذیحجه")]
    Zihajeh = 12,
}
internal enum HijriDaysShort : byte
{
    [Description("السبت")]
    AsSabt = 1,
    [Description("الأحد")]
    AlAhad = 2,
    [Description("الأثنين")]
    ALithNayn = 3,
    [Description("الثلاثاء")]
    AthThuLaTha = 4,
    [Description("الأربعاء")]
    Alarbaa = 5,
    [Description("الخميس")]
    Alkhamis = 6,
    [Description("الجمعه")]
    Aljumah = 7
}
//Gregorian 
internal enum GregorianMonths : byte
{
    [Description("January")]
    January = 1,
    [Description("February")]
    February = 2,
    [Description("March")]
    March = 3,
    [Description("April")]
    April = 4,
    [Description("May")]
    May = 5,
    [Description("June")]
    June = 6,
    [Description("July")]
    Temmuz = 7,
    [Description("August")]
    August = 8,
    [Description("September")]
    September = 9,
    [Description("October")]
    October = 10,
    [Description("November")]
    November = 11,
    [Description("December")]
    December = 12,
}
internal enum GregorianDays : byte
{
    [Description("Monday")]
    Monday = 1,
    [Description("Tuesday")]
    Tuesday = 2,
    [Description("Wednesday")]
    Wednesday = 3,
    [Description("Thursday")]
    Thursday = 4,
    [Description("Friday")]
    Friday = 5,
    [Description("Saturday")]
    Saturday = 6,
    [Description("Sunday")]
    Sunday = 7

}
internal enum GregorianMonthsShort : byte
{
    [Description("Jan")]
    Jan = 1,
    [Description("Feb")]
    Feb = 2,
    [Description("Mar")]
    Mar = 3,
    [Description("Apr")]
    Apr = 4,
    [Description("May")]
    May = 5,
    [Description("Jun")]
    Jun = 6,
    [Description("Jul")]
    Jul = 7,
    [Description("Aug")]
    Aug = 8,
    [Description("Sep")]
    Sep = 9,
    [Description("Oct")]
    Oct = 10,
    [Description("Nov")]
    Nov = 11,
    [Description("Dec")]
    Dec = 12,
}
internal enum GregorianDaysShort : byte
{
    [Description("Mo")]
    Mo = 1,
    [Description("Tu")]
    Tu = 2,
    [Description("We")]
    We = 3,
    [Description("Th")]
    Th = 4,
    [Description("Fr")]
    Fr = 5,
    [Description("Sa")]
    Sa = 6,
    [Description("Su")]
    Su = 7
}
//German
internal enum GermanMonths : byte
{
    [Description("Januar")]
    Januar = 1,
    [Description("Februar")]
    Februar = 2,
    [Description("März")]
    Marz = 3,
    [Description("April")]
    April = 4,
    [Description("Mai")]
    Mai = 5,
    [Description("Juni")]
    Juni = 6,
    [Description("Juli")]
    Juli = 7,
    [Description("August")]
    August = 8,
    [Description("September")]
    September = 9,
    [Description("Oktober")]
    Oktober = 10,
    [Description("November")]
    November = 11,
    [Description("Dezember")]
    Dezember = 12,
}
internal enum GermanDays : byte
{
    [Description("Montag")]
    Montag = 1,
    [Description("Dienstag")]
    Dienstag = 2,
    [Description("Mittwoch")]
    Mittwoch = 3,
    [Description("Donnerstag")]
    Donnerstag = 4,
    [Description("Freitag")]
    Freitag = 5,
    [Description("Samstag")]
    Samstag = 6,
    [Description("Sonntag")]
    Sonntag = 7

}
internal enum GermanMonthsShort : byte
{
    [Description("Jan")]
    Jan = 1,
    [Description("Feb")]
    Feb = 2,
    [Description("Mär")]
    Mar = 3,
    [Description("Apr")]
    Apr = 4,
    [Description("Mai")]
    Mai = 5,
    [Description("Jun")]
    Jun = 6,
    [Description("Jul")]
    Jul = 7,
    [Description("Aug")]
    Aug = 8,
    [Description("Sep")]
    Sep = 9,
    [Description("Okt")]
    Okt = 10,
    [Description("Nov")]
    Nov = 11,
    [Description("Dez")]
    Dez = 12,
}
internal enum GermanDaysShort : byte
{
    [Description("Mo")]
    Mo = 1,
    [Description("Di")]
    Di = 2,
    [Description("Mi")]
    Mi = 3,
    [Description("Do")]
    Do = 4,
    [Description("Fr")]
    Fr = 5,
    [Description("Sa")]
    Sa = 6,
    [Description("So")]
    So = 7
}