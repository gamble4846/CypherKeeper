export interface tbKeysModel {
    id: string;
    parentGroupId: string;
    name: string;
    userName: string;
    password: string;
    websiteId: string | null;
    notes: string;
    isDeleted: boolean;
    deletedDate: string | null;
    updatedDate: string | null;
    createdDate: string;
}