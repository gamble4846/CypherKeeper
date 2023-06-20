export interface tbWebsitesModel {
    id: string;
    name: string;
    link: string | null;
    iconId: string | null;
    isDeleted: boolean;
    deletedDate: string | null;
    updatedDate: string | null;
    createdDate: string;
}

export interface tbWebsitesModel_ToAdd {
    Name: string;
    Link: string | null;
    IconId: string | null;
}