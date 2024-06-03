namespace AssociationManagement.Core.Enums;
[Flags]
public enum Months {
    None = 0,
    Janvier = 1,
    Février = 2,
    Mars = 4,
    Avril = 8,
    Mai = 16,
    Juin = 32,
    Juillet = 64,
    Août = 128,
    Septembre = 256,
    Octobre = 512,
    Novembre = 1024,
    Decembre = 2048,
    Tout = Janvier | Février | Mars | Avril | Mai | Juin | Juillet | Août | Septembre | Octobre | Novembre | Decembre,
}
