export interface tbTwoFactorAuthModel {
    Id: string;
    Name: string;
    SecretKey: string;
    Mode: string;
    CodeSize: number;
    Type: string;
    KeyId: string;
    isDeleted: boolean;
    CreatedDate: string;
    UpdatedDate: string | null;
    DeletedDate: string | null;
    ArrangePosition: number | null;
    Step: number;
}

export interface tbTwoFactorAuthModel_ADD {
    Name: string;
    SecretKey: string;
    Mode: string;
    CodeSize: number;
    Type: string;
    KeyId: string | null;
    Id: string | null;
    Step: number;
}

export interface TwoFAViewModel{
    Id: string,
    Code: string,
    Time: number,
    Step: number,
}