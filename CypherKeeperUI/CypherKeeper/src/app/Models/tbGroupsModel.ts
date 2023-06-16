export interface tbGroupsModel {
    id: string;
    name: string;
    parentGroupId: string | null;
    iconId: string | null;
    isDeleted: boolean;
    createdDate: string;
    updatedDate: string | null;
    deletedDate: string | null;
}