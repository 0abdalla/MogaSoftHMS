namespace Hospital_MS.Core.Common.Consts;

public record AppRole(string Name, string Id, string ConcurrencyStamp);

public static class DefaultRoles
{
    public static readonly AppRole SystemAdmin = new(
        nameof(SystemAdmin),
        "0196ce06-a35d-764c-8e7f-5296e1c570a9",
        "0196ce08-5df8-732f-9f84-7024b78b0f4d"
    );

    public static readonly AppRole TopManagement = new(
        nameof(TopManagement),
        "0196ce06-a360-71e0-b597-893f262003e5",
        "0196ce08-5df8-732f-9f84-702596f53eb7"
    );

    public static readonly AppRole FinanceManager = new(
        nameof(FinanceManager),
        "0196ce06-a360-71e0-b597-89401c1a99a6",
        "0196ce08-5df8-732f-9f84-7026d200cb76"
    );

    public static readonly AppRole TechnicalManager = new(
        nameof(TechnicalManager),
        "0196ce06-a360-71e0-b597-8941805b5a7e",
        "0196ce08-5df8-732f-9f84-70271213664d"
    );

    public static readonly AppRole HRManager = new(
        nameof(HRManager),
        "0196ce06-a360-71e0-b597-89425e3c917f",
        "0196ce08-5df8-732f-9f84-7028b1911846"
    );

    public static readonly AppRole Accountant = new(
        nameof(Accountant),
        "0196ce06-a360-71e0-b597-89431c9fe61b",
        "0196ce08-5df8-732f-9f84-702986ab7a2b"
    );

    public static readonly AppRole StoreKeeper = new( // مسؤول المخازن
        nameof(StoreKeeper),
        "0196ce06-a360-71e0-b597-89441848d7b0",
        "0196ce08-5df8-732f-9f84-702a00ae8e82"
    );

    public static readonly AppRole ReservationEmployee = new( // موظف الحجز
        nameof(ReservationEmployee),
        "0196ce06-a360-71e0-b597-89455dd69d96",
        "0196ce08-5df8-732f-9f84-702b6aab3540"
    );

    public static readonly AppRole Cashier = new(
        nameof(Cashier),
        "0196ce06-a360-71e0-b597-894637b7ca29",
        "0196ce08-5df8-732f-9f84-702cb2a84a7b"
    );

    public static readonly AppRole Auditor = new( // المراجع
        nameof(Auditor),
        "0196ce0b-5aab-766e-9cb1-4e205c0013c7",
        "0196ce08-5df8-732f-9f84-702d6b892289"
    );

    public static IEnumerable<AppRole> All =>
    [
        SystemAdmin, TopManagement, FinanceManager, TechnicalManager, HRManager,
        Accountant, StoreKeeper, ReservationEmployee, Cashier, Auditor
    ];
}
