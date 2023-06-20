export interface tbKeysModel {
    Id: string;
    ParentGroupId: string;
    Name: string;
    UserName: string;
    Password: string;
    WebsiteId: string | null;
    Notes: string;
    isDeleted: boolean;
    DeletedDate: string | null;
    UpdatedDate: string | null;
    CreatedDate: string;
}