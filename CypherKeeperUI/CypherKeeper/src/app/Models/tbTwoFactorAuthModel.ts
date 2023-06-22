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
}

export interface tbTwoFactorAuthModel_ADD {
    Name: string;
    SecretKey: string;
    Mode: string;
    CodeSize: number;
    Type: string;
    KeyId: string;
}