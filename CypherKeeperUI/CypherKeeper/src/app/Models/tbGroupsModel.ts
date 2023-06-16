export interface tbGroupsModel {
    Id: string;
    Name: string;
    ParentGroupId: string | null;
    IconId: string | null;
    isDeleted: boolean;
    CreatedDate: string;
    UpdatedDate: string | null;
    DeletedDate: string | null;
}